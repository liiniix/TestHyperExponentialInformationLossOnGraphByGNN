using Controller;
using Models;

namespace Forntend
{
    public class GNNAndLambdaExponentialInformationLossFrontend : IGNNAndLambdaExponentialInformationLossFrontend
    {
        #region Fields
        private readonly IGNNAndLambdaExponentialInformationLossController _gNNAndLambdaexponentialInformationLossController;
        #endregion

        #region Ctor
        public GNNAndLambdaExponentialInformationLossFrontend(IGNNAndLambdaExponentialInformationLossController exponentialInformationLossController)
        {
            _gNNAndLambdaexponentialInformationLossController = exponentialInformationLossController;
        }
        #endregion

        #region Methods
        public void CreateGNNAndLambdaExponentialInformationLossPlot()
        {
            var lambdaInformationLossParameterList = CreateLambdaInformationLossParameterList().ToList();

            var gNNAndLambdaExponentialInformationLossMasterModel = new GNNAndLambdaExponentialInformationLossMasterModel
            {
                LambdaInformationLoss = lambdaInformationLossParameterList,
            };

            _gNNAndLambdaexponentialInformationLossController.CreateGNNAndLambdaExponentialInformationLossPlot("zcv", "zvc", lambdaInformationLossParameterList, false);
        }
        #endregion

        #region Private Methods
        private IEnumerable<LambdaInformationLossModel> CreateLambdaInformationLossParameterList()
        {
            yield return new LambdaInformationLossModel
            {
                Lambda = .99f,
                Step = 20
            };
            yield return new LambdaInformationLossModel
            {
                Lambda = .98f,
                Step = 20
            };
            yield return new LambdaInformationLossModel
            {
                Lambda = .97f,
                Step = 20
            };
            yield return new LambdaInformationLossModel
            {
                Lambda = .96f,
                Step = 20
            };
            yield return new LambdaInformationLossModel
            {
                Lambda = .95f,
                Step = 20
            };
            yield return new LambdaInformationLossModel
            {
                Lambda = .9f,
                Step = 20
            };
            yield return new LambdaInformationLossModel
            {
                Lambda = .8f,
                Step = 20
            };
            yield return new LambdaInformationLossModel
            {
                Lambda = .7f,
                Step = 20
            };
            yield return new LambdaInformationLossModel
            {
                Lambda = .6f,
                Step = 20
            };
            yield return new LambdaInformationLossModel
            {
                Lambda = .5f,
                Step = 20
            };
            yield return new LambdaInformationLossModel
            {
                Lambda = .4f,
                Step = 20
            };
            yield return new LambdaInformationLossModel
            {
                Lambda = .3f,
                Step = 20
            };
            yield return new LambdaInformationLossModel
            {
                Lambda = .2f,
                Step = 20
            };
            yield return new LambdaInformationLossModel
            {
                Lambda = .1f,
                Step = 20
            };
        }
        #endregion

    }
}