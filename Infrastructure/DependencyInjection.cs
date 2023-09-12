using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        [Obsolete]
        public static void AddInfrastructure(this IServiceCollection services) { 
            services.AddQuartz(options => {
                options.UseMicrosoftDependencyInjectionJobFactory();
                var jobKey = JobKey.Create(nameof(LoggingBackgroundJob));
                options.AddJob<LoggingBackgroundJob>(jobKey)
                .AddTrigger(trigger => trigger
                                            .ForJob(jobKey)
                                            .WithSimpleSchedule(schedule => 
                                                schedule.WithIntervalInSeconds(5).RepeatForever()));
            });
            services.AddQuartzHostedService();
        }

    }
}