using DataUtility.DataModel;

namespace DataUtility.RawDatasets
{
    public interface IGraphDataRetrieval
    {
        public GraphAdjacencyListAndDistinctNodeList GetGraphAdjacencyListAndDistinctNodeHashSet(string fileNameWithLocation);
    }
}
