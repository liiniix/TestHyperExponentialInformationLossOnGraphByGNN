using DataUtility.DataModel;
using Models;

namespace PlottingService
{
    public interface IGNNAndLambdaExponentialInformationLossPlottingService
    {
        public void CreateCombinedLambdaInformationLossPlot(IEnumerable<LambdaInformationLossModel> discountingFactorInfoList);
        public void CreateGNNLevelwiseNodeTraversalStatisticsPlot(List<GNNLevelwiseNodeTraversalStatistics> gNNLevelwiseNodeTraversalStatisticsList, string filename, bool isGetSummerizedNodeTraversal, int maximumTraversalLevel);
    }
}