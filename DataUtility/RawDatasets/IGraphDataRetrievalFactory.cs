using DataUtility.DataModel;

namespace DataUtility.RawDatasets
{
    public enum AvailableGraphDataEnum
    {
        Cora,
        Citeseer,
        Pubmed
    }

    public interface IGraphDataRetrievalFactory
    {
        public IGraphDataRetrieval GetGraphDataRetrievalSystem(AvailableGraphDataEnum availableGraphDataEnum);
    }
}