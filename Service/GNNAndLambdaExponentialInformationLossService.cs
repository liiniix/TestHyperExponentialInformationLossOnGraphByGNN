using DataUtility.DataService;
using Models;
using PlottingService;
using System.Text.RegularExpressions;

namespace Service
{
    public class GNNAndLambdaExponentialInformationLossPlottingService : IGNNAndLambdaExponentialInformationLossService
    {
        #region Fields
        private readonly IGNNAndLambdaExponentialInformationLossPlottingService _gNNAndLambdaExponentialInformationLossPlottingByScottPlotService;
        private readonly IGNNTraversalStatisticsService _gNNTraversalStatisticsService;
        #endregion

        #region CTor
        public GNNAndLambdaExponentialInformationLossPlottingService(IGNNAndLambdaExponentialInformationLossPlottingService exponentialInformationLossPlottingByScottPlotService,
                                                                     IGNNTraversalStatisticsService gNNTraversalStatisticsService)
        {
            _gNNAndLambdaExponentialInformationLossPlottingByScottPlotService = exponentialInformationLossPlottingByScottPlotService;
            _gNNTraversalStatisticsService = gNNTraversalStatisticsService;
        }
        #endregion

        #region Methods
        public void CreateGNNAndLambdaExponentialInformationLossPlot(string outputFilePathOfPlotImage,
                                                                     string outputFileNameOfPlotImage,
                                                                     IEnumerable<LambdaInformationLossModel> lambdaInformationLossParameterList,
                                                                     bool isGetSummerizedNodeTraversal)
        {
            CreateLambdaInformationLossPlot(lambdaInformationLossParameterList);
            CreateExponentialInformationLossByGNNPlot(isGetSummerizedNodeTraversal);
        }

        public void CreateGNNAndLambdaExponentialInformationLossPlot()
        {

        }
        #endregion

        #region Private Methods
        private void CreateLambdaInformationLossPlot(IEnumerable<LambdaInformationLossModel> lambdaInformationLossParameterList)
        {
            var discountingFactorInfoList = lambdaInformationLossParameterList.Select(item => new LambdaInformationLossModel
                                                                                        {
                                                                                            Lambda = item.Lambda,
                                                                                            Step = item.Step,
                                                                                            DiscountingWeight =
                                                                                            CreateDiscountingWeightEnumerableForLambdaInformationLoss(item.Lambda, item.Step) ?? Enumerable.Empty<double>()
                                                                                        })
                                                                                        ??
                                                                                        Enumerable.Empty<LambdaInformationLossModel>();

            _gNNAndLambdaExponentialInformationLossPlottingByScottPlotService.CreateCombinedLambdaInformationLossPlot(discountingFactorInfoList);
        }

        private void CreateExponentialInformationLossByGNNPlot(bool isGetSummerizedNodeTraversal)
        {
            string[] graphDatasetNamesList = GetAvaialbleGraphDatasetNamesList();

            foreach(var graphDatasetName in graphDatasetNamesList)
            {
                int maximumTraversalLevel = 3;
                var gNNLevelwiseNodeTraversalStatisticsList = _gNNTraversalStatisticsService.GetGNNLevelwiseNodeTraversalStatisticsEnumerable(graphDatasetName, maximumTraversalLevel, isGetSummerizedNodeTraversal);
                string filenameForGraphImage = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-tt") + "_"
                                                            + 
                                                            (Regex.IsMatch(graphDatasetName, @"\.*cora\.*") ? "Cora" : "Citeseer") + "_"
                                                            +
                                                            maximumTraversalLevel
                                                            +".png";

                _gNNAndLambdaExponentialInformationLossPlottingByScottPlotService.CreateGNNLevelwiseNodeTraversalStatisticsPlot(gNNLevelwiseNodeTraversalStatisticsList,
                                                                                                                                filenameForGraphImage,
                                                                                                                                isGetSummerizedNodeTraversal,
                                                                                                                                maximumTraversalLevel);
            }
        }

        private IEnumerable<double> CreateDiscountingWeightEnumerableForLambdaInformationLoss(float lambda, int step)
        {
            for (int i = 0; i < step; ++i)
            {
                double currentDiscountingWeightAccordingToLambda = Math.Pow(lambda, i);

                yield return currentDiscountingWeightAccordingToLambda;
            }
        }

        private string[] GetAvaialbleGraphDatasetNamesList()
        {
            return new string[] {
                //@"C:\Users\thaki\Documents\PersonalProject\TestExponentialInformationLossGNN\DataUtility\RawDatasets\Cora\download.tsv.subelj_cora\subelj_cora\cora_out_10.txt",
                //@"C:\Users\thaki\Documents\PersonalProject\TestExponentialInformationLossGNN\DataUtility\RawDatasets\Cora\download.tsv.subelj_cora\subelj_cora\cora_out_500.txt",
                //@"C:\Users\thaki\Documents\PersonalProject\TestExponentialInformationLossGNN\DataUtility\RawDatasets\Cora\download.tsv.subelj_cora\subelj_cora\cora_out_1000.txt",
                //@"C:\Users\thaki\Documents\PersonalProject\TestExponentialInformationLossGNN\DataUtility\RawDatasets\Cora\download.tsv.subelj_cora\subelj_cora\out.subelj_cora_cora",
                //@"C:\Users\thaki\Documents\PersonalProject\TestExponentialInformationLossGNN\DataUtility\RawDatasets\cora\cora\cora.cites",
                //@"C:\Users\thaki\Documents\PersonalProject\TestExponentialInformationLossGNN\DataUtility\RawDatasets\citeseer-doc-classification\citeseer-doc-classification\citeseer.cites",
                @"C:\Users\thaki\Documents\PersonalProject\TestExponentialInformationLossGNN\DataUtility\RawDatasets\cora\cora\test_linear_dataset.txt"
            };
        }
        #endregion
    }
}