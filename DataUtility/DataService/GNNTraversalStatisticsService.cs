using DataUtility.DataModel;
using DataUtility.RawDatasets;
using Node = System.Int32;
using Level = System.Int32;
using Depth = System.Int32;
using IsAlreadyTraversed = System.Boolean;
using System.Text.RegularExpressions;

namespace DataUtility.DataService
{
    public class GNNTraversalStatisticsService: IGNNTraversalStatisticsService
    {
        #region Fields
        private readonly IGraphDataRetrievalFactory _graphDataRetrievalRactory;
        private readonly IGraphDataRetrieval _graphDataRetrieval;
        #endregion

        #region CTor
        public GNNTraversalStatisticsService(IGraphDataRetrievalFactory graphDataRetrievalRactory)
        {
            _graphDataRetrievalRactory = graphDataRetrievalRactory;
            _graphDataRetrieval = _graphDataRetrievalRactory.GetGraphDataRetrievalSystem(AvailableGraphDataEnum.Cora);
        }
        #endregion

        #region Methods
        public List<GNNLevelwiseNodeTraversalStatistics> GetGNNLevelwiseNodeTraversalStatisticsEnumerable(string filenameWithLocation,
                                                                                                          int maximumTraversalLevel,
                                                                                                          bool isGetSummerizedNodeTraversal)
        {
            
            var levelToTraversalStatisticsOfAllNodesList = ProceedToGetAllLocalLevelToTraversalCountList(filenameWithLocation,
                                                                                                         maximumTraversalLevel,
                                                                                                         isGetSummerizedNodeTraversal);

            if (isGetSummerizedNodeTraversal)
            {
                var summerizedLevelwiseToTraversalStatisticsOfAllNodesList = SummerizeLevelToNodeTraversalStatisticsOfAllNodes(levelToTraversalStatisticsOfAllNodesList,
                                                                                                                               maximumTraversalLevel);
                return summerizedLevelwiseToTraversalStatisticsOfAllNodesList;
            }

            return levelToTraversalStatisticsOfAllNodesList;
        }

        public List<GNNLevelwiseNodeTraversalStatistics> ProceedToGetAllLocalLevelToTraversalCountList(string filenameWithLocation,
                                                                                                       int maximumTraversalLevel,
                                                                                                       bool isGetSummerizedNodeTraversal)
        {
            var levelToTraversalStatisticsOfAllNodesList = new List<GNNLevelwiseNodeTraversalStatistics>();

            var graphEnum = Regex.IsMatch(filenameWithLocation, @"\.*cora\.*") ? AvailableGraphDataEnum.Cora : AvailableGraphDataEnum.Citeseer;

            var coraGraphAdjacencyListAndDistinctNodeList = _graphDataRetrievalRactory
                                                                    .GetGraphDataRetrievalSystem(graphEnum)
                                                                    .GetGraphAdjacencyListAndDistinctNodeHashSet(filenameWithLocation);

            int currentlyProcessingNodeCount = 0;
            foreach (var node in coraGraphAdjacencyListAndDistinctNodeList.DistinctNodeList)
            {
                currentlyProcessingNodeCount++;
                if (currentlyProcessingNodeCount % 20 == 0)
                {
                    Console.WriteLine($"{currentlyProcessingNodeCount}/{coraGraphAdjacencyListAndDistinctNodeList.DistinctNodeList.Count}");
                }

                Dictionary<Node, Level> levelOfNodeFromCurrentNodeDict = new Dictionary<Node, Level>();

                GetLocalizedNodeToLevel(node,
                                        coraGraphAdjacencyListAndDistinctNodeList.AdjacencyListDict,
                                        remainingLevel: maximumTraversalLevel,
                                        currentLevel: 0,
                                        levelOfNodeFromCurrentNodeDict);

                Dictionary<(Node, Depth), List<Node>> leafNodesParameterizedByNodeAndCallingDepth = new Dictionary<(Node, Depth), List<Node>>();
                var leafNodesOfCurrentNode = TraverseANodeLikeGNNAndGetLeafNodesParameterizedByNodeAndCallingDepth(node,
                                                         coraGraphAdjacencyListAndDistinctNodeList.AdjacencyListDict,
                                                         remainingLevel: maximumTraversalLevel,
                                                         currentLevel: 0,
                                                         leafNodesParameterizedByNodeAndCallingDepth);

                var localLevelToTraversalCountForANodeList = GetLocalLevelToTraversalCountList(leafNodesOfCurrentNode, levelOfNodeFromCurrentNodeDict);
                
                List<GNNLevelwiseNodeTraversalStatistics> filledUpAllPossibleLocalLevelToTraversalCountList = new List<GNNLevelwiseNodeTraversalStatistics>();
                if (!isGetSummerizedNodeTraversal)
                {
                    var sortedLocalLevelToTraversalCountForANodeList = localLevelToTraversalCountForANodeList
                                                                                        .OrderBy(x => x.Level)
                                                                                        .ToList();

                    filledUpAllPossibleLocalLevelToTraversalCountList = GetFilledUpAllPossibleLocalLevelToTraversalCountList(sortedLocalLevelToTraversalCountForANodeList,
                                                                                                                                 maximumTraversalLevel);
                }

                levelToTraversalStatisticsOfAllNodesList.AddRange(isGetSummerizedNodeTraversal ? localLevelToTraversalCountForANodeList
                                                                                                 : filledUpAllPossibleLocalLevelToTraversalCountList);
            }

            return levelToTraversalStatisticsOfAllNodesList;
        }

        private List<GNNLevelwiseNodeTraversalStatistics> GetFilledUpAllPossibleLocalLevelToTraversalCountList(List<GNNLevelwiseNodeTraversalStatistics> localLevelToTraversalCountForANodeList,
                                                                                                                int maximumTraversalLevel)
        {
            var alreadyFilledUpLevel = localLevelToTraversalCountForANodeList
                                                                .Max(x => x.Level);

            for(int i = alreadyFilledUpLevel + 1; i <= maximumTraversalLevel; i++)
            {
                localLevelToTraversalCountForANodeList.Add(new GNNLevelwiseNodeTraversalStatistics
                {
                    Level = i,
                    TraversalCount = 0
                });
            }

            return localLevelToTraversalCountForANodeList;
        }

        private List<GNNLevelwiseNodeTraversalStatistics> GetLocalLevelToTraversalCountList(List<Node> leafNodesOfCurrentNode, Dictionary<Node, Level> levelOfNodeFromCurrentNodeDict)
        {
            return leafNodesOfCurrentNode.GroupBy(item => item)
                                  .Select(group => new GNNLevelwiseNodeTraversalStatistics
                                  {
                                      Level = levelOfNodeFromCurrentNodeDict[group.Key],
                                      TraversalCount = group.Count()
                                  }).ToList();
        }

        public void GetGNNLevelwiseNormalisedNodeTraversalStatisticsEnumerable()
        {

        }
        #endregion

        #region Private Methods
        private void TraverseANodeLikeGNNAndGetStatistics(int rootNode,
                                                         Dictionary<int, List<int>> coraAdjacencyList,
                                                         int remainingLevel,
                                                         int currentLevel,
                                                         Dictionary<int, int> levelOfNodeFromARootDict,
                                                         Dictionary<int, double> traversalCountOFANodeFromRoot,
                                                         double currentWeight)
        {
            List<int> neighbourhoodNodes = coraAdjacencyList[rootNode];
            var neighbourhoodNodesWithSelfNode = neighbourhoodNodes.Union(new List<int> { rootNode }).ToList();
            

            bool levelOfCurrentNodeAlreadySaved = levelOfNodeFromARootDict.TryGetValue(rootNode, out _);

            if (!levelOfCurrentNodeAlreadySaved)
            {
                levelOfNodeFromARootDict[rootNode] = currentLevel;
            }

            if (remainingLevel == 0)
            {
                var level = levelOfNodeFromARootDict[rootNode];

                bool levelwiseNodeTraversalCountAlreadyInitialized = traversalCountOFANodeFromRoot.TryGetValue(rootNode, out _);
                if (levelwiseNodeTraversalCountAlreadyInitialized)
                {
                    traversalCountOFANodeFromRoot[rootNode] += 1;// currentWeight;
                    return;
                }

                traversalCountOFANodeFromRoot[rootNode] = 1;

                return;
            }

            currentWeight *= 1.0 / Convert.ToDouble(neighbourhoodNodesWithSelfNode.Count);

            foreach (var neighbourhoodNode in neighbourhoodNodesWithSelfNode.ToList())
            {
                TraverseANodeLikeGNNAndGetStatistics(neighbourhoodNode,
                                                     coraAdjacencyList,
                                                     remainingLevel - 1,
                                                     currentLevel + 1,
                                                     levelOfNodeFromARootDict,
                                                     traversalCountOFANodeFromRoot,
                                                     currentWeight);
            }
        }

        private void GetLocalizedNodeToLevel(int rootNode,
                                                        Dictionary<int, List<int>> coraAdjacencyList,
                                                        int remainingLevel,
                                                        int currentLevel,
                                                        Dictionary<Node, Level> levelOfNodeFromARootDict)
        {
            
            if (remainingLevel < 0)
            {
                return;
            }

            bool isNodeAlreadyTraversed = levelOfNodeFromARootDict.ContainsKey(rootNode);

            levelOfNodeFromARootDict[rootNode] = isNodeAlreadyTraversed ? levelOfNodeFromARootDict[rootNode] : currentLevel;

            List<int> neighbourhoodNodes = coraAdjacencyList.TryGetValue(rootNode, out _) ? coraAdjacencyList[rootNode] : new List<int>();

            foreach (var neighbourhoodNode in neighbourhoodNodes)
            {
                GetLocalizedNodeToLevel(rootNode: neighbourhoodNode,
                                        coraAdjacencyList,
                                        remainingLevel: remainingLevel - 1,
                                        currentLevel + 1,
                                        levelOfNodeFromARootDict);
            }
        }

        private List<Node> TraverseANodeLikeGNNAndGetLeafNodesParameterizedByNodeAndCallingDepth(int rootNode,
                                                         Dictionary<int, List<int>> coraAdjacencyList,
                                                         int remainingLevel,
                                                         int currentLevel,
                                                         Dictionary<(Node, Depth), List<Node>> leafNodesParameterizedByNodeAndCallingDepth)
        {
            if (remainingLevel == 0)
            {
                return new List<Node> { rootNode };
            }

            bool leafNodesAlreadyExist = leafNodesParameterizedByNodeAndCallingDepth.ContainsKey((rootNode, remainingLevel));

            if(leafNodesAlreadyExist)
            {
                return leafNodesParameterizedByNodeAndCallingDepth[(rootNode, remainingLevel)];
            }
            
            var neighbourhoodNodes = coraAdjacencyList.TryGetValue(rootNode, out _) ? coraAdjacencyList[rootNode] : new List<int>();
            var neighbourhoodNodesWithSelfNode = neighbourhoodNodes.Union(new List<int> { rootNode }).ToList();

            var leafNodesOfCurrentNode = new List<int>();

            if (remainingLevel == 1)
            {
                leafNodesOfCurrentNode = neighbourhoodNodesWithSelfNode;
                leafNodesParameterizedByNodeAndCallingDepth[(rootNode, remainingLevel)] = leafNodesOfCurrentNode;
            }
            else
            {
                foreach (var neighbourhoodNode in neighbourhoodNodesWithSelfNode.ToList())
                {
                    var leafNodesOfCurrentNeighbout = TraverseANodeLikeGNNAndGetLeafNodesParameterizedByNodeAndCallingDepth(neighbourhoodNode,
                                                                                                                            coraAdjacencyList,
                                                                                                                            remainingLevel - 1,
                                                                                                                            currentLevel + 1,
                                                                                                                            leafNodesParameterizedByNodeAndCallingDepth);

                    leafNodesOfCurrentNode.AddRange(leafNodesOfCurrentNeighbout);
                }

                leafNodesParameterizedByNodeAndCallingDepth[(rootNode, remainingLevel)] = leafNodesOfCurrentNode;
            }
            
            return leafNodesOfCurrentNode;
        }

        private List<GNNLevelwiseNodeTraversalStatistics> SummerizeLevelToNodeTraversalStatisticsOfAllNodes(List<GNNLevelwiseNodeTraversalStatistics> gNNLevelwiseNodeTraversalStatisticsList,
                                                                                                            int maximumTraversalLevel)
        {
            List<GNNLevelwiseNodeTraversalStatistics> summerizedLevelToNodeTraversalStatisticsOfAllNodes = new List<GNNLevelwiseNodeTraversalStatistics>();

            for (int currentLevel = 0; currentLevel <= maximumTraversalLevel; ++currentLevel)
            {
                var countOfCurrentLevelNodes = gNNLevelwiseNodeTraversalStatisticsList
                    .Where(item => item.Level == currentLevel)
                    .Count();

                var currentLevelSumTraversal = gNNLevelwiseNodeTraversalStatisticsList
                    .Where(item => item.Level == currentLevel)
                    .Select(x => x.TraversalCount)
                    .Sum();

                summerizedLevelToNodeTraversalStatisticsOfAllNodes.Add(
                    new GNNLevelwiseNodeTraversalStatistics
                    {
                        Level = currentLevel,
                        TraversalCount = countOfCurrentLevelNodes == 0 ? 0 : currentLevelSumTraversal / countOfCurrentLevelNodes
                    }
                );
            }

            return summerizedLevelToNodeTraversalStatisticsOfAllNodes;
        }
        #endregion
    }
}
