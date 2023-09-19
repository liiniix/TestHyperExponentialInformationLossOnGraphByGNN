using Controller;
using Forntend;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DependencyInjector
{
    
    public class DependencyInjectionHandler
    {
        #region CTor
        public DependencyInjectionHandler()
        {
            
        }
        #endregion

        #region Methods
        public IHost InjectDependenciesAndGetHost()
        {
            var _host = Host.CreateDefaultBuilder().ConfigureServices(
                services =>
                {
                    services.RegisterDependencies();
                })
                .Build();

            return _host;        }
        #endregion
    }
}