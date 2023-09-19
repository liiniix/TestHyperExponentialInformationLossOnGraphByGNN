using Controller;
using DataUtility;
using DataUtility.DataService;
using DataUtility.RawDatasets;
using Forntend;
using Microsoft.Extensions.DependencyInjection;
using PlottingService;
using Service;

namespace DependencyInjector
{
    public static class DependencyRegistration
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<IGNNAndLambdaExponentialInformationLossFrontend, GNNAndLambdaExponentialInformationLossFrontend>();
            services.AddScoped<IGNNAndLambdaExponentialInformationLossController, GNNAndLambdaExponentialInformationLossController>();
            services.AddScoped<IGNNAndLambdaExponentialInformationLossService, GNNAndLambdaExponentialInformationLossPlottingService>();
            services.AddScoped<IGNNAndLambdaExponentialInformationLossPlottingService, GNNAndLambdaExponentialInformationLossPlottingByScottPlotService>();
            services.AddScoped<ITextFileReadUoW, TextFileReadUoW>();

            services.AddScoped<IGraphDataRetrievalFactory, GraphDataRetrievalFactory>();

            services.AddScoped<CoraGraphDataRetrieval>();
            services.AddScoped<IGraphDataRetrieval, CoraGraphDataRetrieval>(s=>s.GetService<CoraGraphDataRetrieval>());

            services.AddScoped<CiteseerGraphDataRetrieval>();
            services.AddScoped<IGraphDataRetrieval, CiteseerGraphDataRetrieval>(s => s.GetService<CiteseerGraphDataRetrieval>());

            services.AddScoped<IGNNTraversalStatisticsService, GNNTraversalStatisticsService>();

            return services;
        }
    }
}
