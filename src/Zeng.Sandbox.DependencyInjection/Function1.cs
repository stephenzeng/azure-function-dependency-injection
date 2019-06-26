using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Zeng.Sandbox.DependencyInjection.Startup))]
namespace Zeng.Sandbox.DependencyInjection
{
    public class Function1
    {
        private readonly IEnumerable<IProjectionWorker> _projectionWorkers;

        public Function1(IEnumerable<IProjectionWorker> projectionWorkers)
        {
            _projectionWorkers = projectionWorkers;
        }

        [FunctionName("Function1")]
        public async Task Run([TimerTrigger("05:00:00", RunOnStartup = true)]TimerInfo myTimer)
        {
            foreach (var projectionWorker in _projectionWorkers)
            {
                await projectionWorker.Execute();
            }
        }
    }

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped(typeof(IBookmark<>), typeof(Bookmark<>));
            builder.Services.AddScoped(typeof(IDocumentWriter<>), typeof(DocumentWriter<>));
            builder.Services.AddSingleton<ITransactionExecutor>(s => new SqlTransactionExecutor("TestConnectionString"));

            builder.Services.Scan(s => s.FromAssemblies(typeof(IProjectionWorker).Assembly)
                .AddClasses(c => c.AssignableTo<IProjectionWorker>())
                .AsImplementedInterfaces());
        }
    }
}
