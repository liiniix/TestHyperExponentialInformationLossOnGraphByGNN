using Models;

namespace Controller
{
    public interface IGNNAndLambdaExponentialInformationLossController
    {
        void CreateGNNAndLambdaExponentialInformationLossPlot(string outputFilePathOfPlotImage, string outputFileNameOfPlotImage, List<LambdaInformationLossModel> discountingFactorInfoParameterList, bool isGetSummerizedNodeTraversal);
    }
}