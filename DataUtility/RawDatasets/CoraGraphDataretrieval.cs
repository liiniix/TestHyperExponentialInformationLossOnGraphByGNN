using DataUtility.DataModel;

namespace DataUtility.RawDatasets
{
    public class CoraGraphDataRetrieval : IGraphDataRetrieval
    {
        #region Fields
        private readonly ITextFileReadUoW _textFileReadUoW;
        #endregion

        #region CTor
        public CoraGraphDataRetrieval(ITextFileReadUoW textFileReadUoW)
        {
            _textFileReadUoW = textFileReadUoW;
        }
        #endregion

        #region Methods
        public GraphAdjacencyListAndDistinctNodeList GetGraphAdjacencyListAndDistinctNodeHashSet(string fileNameWithLocation)
        {
            var edgeList = _textFileReadUoW.GetLinesFromTextFileList(fileNameWithLocation);
            
            var graphAdjacencyList = new Dictionary<int, List<int>>();
            var distinctNodesHashSet = new HashSet<int>();

            //int currentLineCount = 0;

            foreach (var edge in edgeList)
            {
                //currentLineCount++;
                //if (currentLineCount <= 2) { continue; }

                var twoNodes = edge.Split('\t');

                int firstNodeInt = int.Parse(twoNodes[0]);
                int secondNodeInt = int.Parse(twoNodes[1]);

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
    }
}
