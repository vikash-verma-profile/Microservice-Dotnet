using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Consul
{
    public static class AppExtensions
    {
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(config =>
            {
                var address = configuration["Consul:ConsulAddress"];
                config.Address = new Uri(address);

            }));
            return services;
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app,IConfiguration configuration)
        {
            var consulClient=app.ApplicationServices.GetRequiredService<IConsulClient>();
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("AppExtensions");
            var lifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();

            var registartion = new AgentServiceRegistration()
            {
                ID = configuration["Consul:ServiceId"],
                Name = configuration["Consul:ServiceName"],
                Address = configuration["Consul:ServiceHost"],
                Port = int.Parse(configuration["Consul:ServicePort"])
            };
            logger.LogInformation("Registration with consul");
            consulClient.Agent.ServiceDeregister(registartion.ID).ConfigureAwait(true);
            consulClient.Agent.ServiceRegister(registartion).ConfigureAwait(true);
            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Unregistering from consul");
            });
            return app;
        }

    }
}
