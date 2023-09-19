using DataUtility.DataModel;
using Models;
using System.Linq;

namespace PlottingService
{
    public class GNNAndLambdaExponentialInformationLossPlottingByScottPlotService : IGNNAndLambdaExponentialInformationLossPlottingService
    {
        #region Ctor
        public GNNAndLambdaExponentialInformationLossPlottingByScottPlotService()
        {

        }
        #endregion

        #region Methods
        public void CreateCombinedLambdaInformationLossPlot(IEnumerable<LambdaInformationLossModel> discountingFactorInfoList)
        {
            var lambdaInformationLossPlot = new ScottPlot.Plot(1920, 1080);

            foreach (var discountingFactorInfo in discountingFactorInfoList)
            {
                var discreteLambdas = Enumerable.Range(0, discountingFactorInfo.Step).Select(item => Convert.ToDouble(item)).ToArray();
                var discountingWeights = discountingFactorInfo.DiscountingWeight.ToArray();

                lambdaInformationLossPlot.AddScatter(discreteLambdas, discountingWeights, label: $"Lambda: {discountingFactorInfo.Lambda}");
            }

            lambdaInformationLossPlot.Legend();
            lambdaInformationLossPlot.SaveFig("quickstart.png");
        }

        public void CreateGNNLevelwiseNodeTraversalStatisticsPlot(List<GNNLevelwiseNodeTraversalStatistics> gNNLevelwiseNodeTraversalStatisticsList,
                                                                  string filename,
                                                                  bool isGetSummerizedNodeTraversal,
                                                                  int maximumTraversalLevel)
        {
            var gNNLevelwiseNodeTraversalStatisticsPlot = new ScottPlot.Plot(1920, 1080);

            double[] countList;
            double[] levelList;

            countList = gNNLevelwiseNodeTraversalStatisticsList.Select(item => Convert.ToDouble(item.TraversalCount)).ToArray();

            if (isGetSummerizedNodeTraversal)
            {
                levelList = gNNLevelwiseNodeTraversalStatisticsList.Select(item => Convert.ToDouble(item.Level)).ToArray();
                gNNLevelwiseNodeTraversalStatisticsPlot.AddScatter(levelList, countList);

                gNNLevelwiseNodeTraversalStatisticsPlot.Legend();
                gNNLevelwiseNodeTraversalStatisticsPlot.SaveFig(filename);
                return;

            }
            int chunkSize = maximumTraversalLevel + 1;
            for(int i = 0; i < countList.Length / chunkSize + 1; ++i)
            {
                int sampleRate = 1;
                var chunkedCountList = countList
                                        .Skip(i * chunkSize)
                                        .Take(chunkSize)
                                        .ToArray();

                gNNLevelwiseNodeTraversalStatisticsPlot.AddSignal(chunkedCountList, sampleRate);

                gNNLevelwiseNodeTraversalStatisticsPlot.Legend();

                bool intervalOrLast = (i + 1) % 10 == 0 || i == countList.Length / chunkSize;

             
                gNNLevelwiseNodeTraversalStatisticsPlot.SaveFig("chunk_" + i + "_" + filename);

                gNNLevelwiseNodeTraversalStatisticsPlot.Clear();

            }
        }
        #endregion

    }
}