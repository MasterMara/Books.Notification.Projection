using Book.Events.V1.Book;
using Books.Notification.Projection.Consumers.Book;
using Books.Notification.Projection.Handlers;
using Books.Notification.Projection.Logging;
using Books.Notification.Projection.Service;
using Books.Notification.Projection.Settings;
using Books.Notification.Projection.Settings.BusSettings;
using GreenPipes;
using MassTransit;
using MassTransit.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureHostConfiguration(configHost => { configHost.AddEnvironmentVariables(prefix: "DOTNET_"); })
            .ConfigureServices((hostContext, services) =>
            {
                //Configs
                services.Configure<BusSettings>(
                    hostContext.Configuration.GetSection(nameof(BusSettings)));

                services.Configure<ConsoleLoggerSettings>(
                    hostContext.Configuration.GetSection(nameof(ConsoleLoggerSettings)));

                //DI Injections
                services.AddTransient<CorrelationIdHandler>();
                services.AddTransient<OriginDelegationHandler>();
                services.AddSingleton<IConsoleLogger, ConsoleLogger>();

                services.AddOptions();

                services.AddSingleton(serviceProvider =>
                {
                    var busSettings = serviceProvider.GetService<IOptions<BusSettings>>()!.Value;
                    return Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        cfg.UseRetry(r => r.Immediate(busSettings.RetryLimit));

                        cfg.Host(new Uri(busSettings.HostAddress), hostConfigurator =>
                        {
                            hostConfigurator.Username(busSettings.Username);
                            hostConfigurator.Password(busSettings.Password);
                            hostConfigurator.Heartbeat(busSettings.Heartbeat);
                            hostConfigurator.UseCluster(clusterConfigurator =>
                            {
                                foreach (var clusterMember in busSettings.ClusterMembers)
                                    clusterConfigurator.Node(clusterMember);
                            });
                        });

                        #region Book

                        cfg.ReceiveEndpoint("Events.V1.Book:Created", ec =>
                        {
                            ec.ConfigureConsumeTopology = false;

                            ec.Bind<Created>(x =>
                            {
                                x.ExchangeType = ExchangeType.Topic;
                                x.RoutingKey = "Book.*";
                            });

                            ec.Consumer(() =>
                                new BookCreatedConsumer(serviceProvider.GetService<IConsoleLogger>()!
                                ));
                        });
                        
                        cfg.ReceiveEndpoint("Events.V1.Book:Placed", ec =>
                        {
                            ec.ConfigureConsumeTopology = false;

                            ec.Bind<Created>(x =>
                            {
                                x.ExchangeType = ExchangeType.Topic;
                                x.RoutingKey = "Book.*";
                            });

                            ec.Consumer(() =>
                                new BookPlacedConsumer(serviceProvider.GetService<IConsoleLogger>()!
                                ));
                        });
                        
                        cfg.ReceiveEndpoint("Events.V1.Book:Printed", ec =>
                        {
                            ec.ConfigureConsumeTopology = false;

                            ec.Bind<Created>(x =>
                            {
                                x.ExchangeType = ExchangeType.Topic;
                                x.RoutingKey = "Book.*";
                            });

                            ec.Consumer(() =>
                                new BookPrintedConsumer(serviceProvider.GetService<IConsoleLogger>()!
                                ));
                        });
                        
                        cfg.ReceiveEndpoint("Events.V1.Book:Deleted", ec =>
                        {
                            ec.ConfigureConsumeTopology = false;

                            ec.Bind<Created>(x =>
                            {
                                x.ExchangeType = ExchangeType.Topic;
                                x.RoutingKey = "Book.*";
                            });

                            ec.Consumer(() =>
                                new BookDeletedConsumer(serviceProvider.GetService<IConsoleLogger>()!
                                ));
                        });
                        
                        cfg.ReceiveEndpoint("Events.V1.Book:Published", ec =>
                        {
                            ec.ConfigureConsumeTopology = false;

                            ec.Bind<Created>(x =>
                            {
                                x.ExchangeType = ExchangeType.Topic;
                                x.RoutingKey = "Book.*";
                            });

                            ec.Consumer(() =>
                                new BookPublishedConsumer(serviceProvider.GetService<IConsoleLogger>()!
                                ));
                        });
                        

                        #endregion
                    });
                });

                services.AddHostedService<MassTransitBusControlService>();
            })
            .ConfigureLogging((hostContext, logging) =>
            {
                logging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                logging.ClearProviders();
            });


        await builder.Build().RunAsync();
    }
}