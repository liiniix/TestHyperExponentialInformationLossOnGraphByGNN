using Models;
using PlottingService;
using Service;

namespace Controller
{
    public class GNNAndLambdaExponentialInformationLossController : IGNNAndLambdaExponentialInformationLossController
    {
        #region Fields
        private readonly IGNNAndLambdaExponentialInformationLossService _gNNAndLambdaExponentialInformationLossService;
        #endregion

        #region CTor
        public GNNAndLambdaExponentialInformationLossController(IGNNAndLambdaExponentialInformationLossService exponentialInformationLossService)
        {
            _gNNAndLambdaExponentialInformationLossService = exponentialInformationLossService;
        }
        #endregion

        #region Methods
        public void CreateGNNAndLambdaExponentialInformationLossPlot(string outputFilePathOfPlotImage,
                                                                     string outputFileNameOfPlotImage,
                                                                     List<LambdaInformationLossModel> lambdaInformationLossParameterList,
                                                                     bool isGetSummerizedNodeTraversal)
        {
            _gNNAndLambdaExponentialInformationLossService.CreateGNNAndLambdaExponentialInformationLossPlot(outputFilePathOfPlotImage,
                                                                                                            outputFileNameOfPlotImage,
                                                                                                            lambdaInformationLossParameterList,
                                                                                                            isGetSummerizedNodeTraversal);
        }
        #endregion
    }
}