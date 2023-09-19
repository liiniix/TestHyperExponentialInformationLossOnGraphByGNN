using DataUtility.DataModel;
using OriginalNodeName = System.String;
using TransformedNodeName = System.Int32;

namespace DataUtility.RawDatasets
{
    public class CiteseerGraphDataRetrieval : IGraphDataRetrieval
    {
        #region Fields
        private readonly ITextFileReadUoW _textFileReadUoW;
        #endregion

        #region CTor
        public CiteseerGraphDataRetrieval(ITextFileReadUoW textFileReadUoW)
        {
            _textFileReadUoW = textFileReadUoW;
        }
        #endregion

        #region Methods
        public GraphAdjacencyListAndDistinctNodeList GetGraphAdjacencyListAndDistinctNodeHashSet(string fileNameWithLocation)
        {
            var edgeList = _textFileReadUoW.GetLinesFromTextFileList(fileNameWithLocation);
            var originalToTransformedNodeNames = TransformNodeNameToIntegerFromOne(fileNameWithLocation);

            var graphAdjacencyList = new Dictionary<int, List<int>>();
            var distinctNodesHashSet = new HashSet<int>();

            foreach (var edge in edgeList)
            {

                var twoNodes = edge.Split('\t');

                int firstNodeInt = originalToTransformedNodeNames[twoNodes[0]];
                int secondNodeInt = originalToTransformedNodeNames[twoNodes[1]];

                distinctNodesHashSet.Add(firstNodeInt);
                distinctNodesHashSet.Add(secondNodeInt);

                bool isFirstNode_sAdjacencyListAlreadyInitialized = graphAdjacencyList.TryGetValue(firstNodeInt, out _);
                if (isFirstNode_sAdjacencyListAlreadyInitialized)
                {
                    graphAdjacencyList[firstNodeInt].Add(secondNodeInt);
                }
                else
                {
                    graphAdjacencyList[firstNodeInt] = new List<int> { secondNodeInt };
                }

                bool isSecondNode_sAdjacencyListAlreadyInitialized = graphAdjacencyList.TryGetValue(secondNodeInt, out _);
                if (isSecondNode_sAdjacencyListAlreadyInitialized)
                {
                    graphAdjacencyList[secondNodeInt].Add(firstNodeInt);
                }
                else
                {
                    graphAdjacencyList[secondNodeInt] = new List<int> { firstNodeInt };
                }
            }

            var graphAdjacencyListAndDistinctNodeList = new GraphAdjacencyListAndDistinctNodeList
            {
                AdjacencyListDict = graphAdjacencyList,
                DistinctNodeList = distinctNodesHashSet
            };

            return graphAdjacencyListAndDistinctNodeList;
        }
        #endregion

        #region Private Methods
        private Dictionary<OriginalNodeName, TransformedNodeName> TransformNodeNameToIntegerFromOne(string fileNameWithLocation)
        {
            TransformedNodeName transformedNodeName = 1;
            var edgeList = _textFileReadUoW.GetLinesFromTextFileList(fileNameWithLocation);
            var alreadyConvertedNodes = new HashSet<OriginalNodeName>();

            var originalToTransformedNodeName = new Dictionary<OriginalNodeName, TransformedNodeName>();
            foreach (var edge in edgeList)
            {

                var twoNodesOriginal = edge.Split('\t');

                var firstNodeOriginal = twoNodesOriginal[0];
                var secondNodeOriginal = twoNodesOriginal[1];

                if(!alreadyConvertedNodes.Contains(firstNodeOriginal))
                {
                    originalToTransformedNodeName[firstNodeOriginal] = transformedNodeName++;
                    alreadyConvertedNodes.Add(firstNodeOriginal);
                }

                if (!alreadyConvertedNodes.Contains(secondNodeOriginal))
                {
                    originalToTransformedNodeName[secondNodeOriginal] = transformedNodeName++;
                    alreadyConvertedNodes.Add(secondNodeOriginal);
                }
            }

            return originalToTransformedNodeName;
        }
        #endregion
    }
}
