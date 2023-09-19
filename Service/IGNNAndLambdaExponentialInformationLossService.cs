using Models;

namespace Service
{
    public interface IGNNAndLambdaExponentialInformationLossService
    {
        void CreateGNNAndLambdaExponentialInformationLossPlot(string outputFilePathOfPlotImage, string outputFileNameOfPlotImage, IEnumerable<LambdaInformationLossModel> discountingFactorInfoParameterList, bool isGetSummerizedNodeTraversal);
    }
}