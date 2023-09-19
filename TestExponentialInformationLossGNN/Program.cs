using DependencyInjector;
using Forntend;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestExponentialInformationLossGNN
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = InitializeServicesAndGetHost();

            var _GNNAndLambdaExponentialInformationLossFrontend = host.Services.GetService<IGNNAndLambdaExponentialInformationLossFrontend>();

            _GNNAndLambdaExponentialInformationLossFrontend.CreateGNNAndLambdaExponentialInformationLossPlot();
        }

        static IHost InitializeServicesAndGetHost()
        {
            var dependencyInjectionHandler = new DependencyInjectionHandler();

            var host = dependencyInjectionHandler.InjectDependenciesAndGetHost();

            return host;
        }
    }
}