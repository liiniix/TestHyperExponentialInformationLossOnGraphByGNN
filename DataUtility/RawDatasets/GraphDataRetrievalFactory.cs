using DataUtility.DataModel;

namespace DataUtility.RawDatasets
{
    public class GraphDataRetrievalFactory : IGraphDataRetrievalFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public GraphDataRetrievalFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IGraphDataRetrieval GetGraphDataRetrievalSystem(AvailableGraphDataEnum availableGraphDataEnum)
        {
            switch(availableGraphDataEnum)
            {
                case AvailableGraphDataEnum.Cora:
                    return (IGraphDataRetrieval)_serviceProvider.GetService(typeof(CoraGraphDataRetrieval));
                case AvailableGraphDataEnum.Citeseer:
                    return (IGraphDataRetrieval)_serviceProvider.GetService(typeof(CiteseerGraphDataRetrieval));
                default:
                    throw new ArgumentException(nameof(availableGraphDataEnum), $"Shape of {availableGraphDataEnum} is not supported.");

            }
        }
    }
}