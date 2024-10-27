using DeliveryService.Application.Interfaces.Repositories;
using DeliveryService.Application.Interfaces.Services;
using DeliveryService.Application.Services;
using DeliveryService.Infrastructure.FileStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

namespace DeliveryService.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var logDirectory = GetOrCreateLogDirectory();

            ConfigureLogging(logDirectory);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var serviceProvider = ConfigureServices(configuration);
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            var orderService = serviceProvider.GetRequiredService<IOrderService>();

            if (!ValidateInputArguments(args, logger,
                out var cityDistrict, out var firstDeliveryDateTime)) return;

            FilterAndSaveOrders(configuration, logger,
                orderService, cityDistrict, firstDeliveryDateTime);

            Log.CloseAndFlush();
        }

        public static bool ValidateInputArguments(string[] args, ILogger<Program> logger,
            out string cityDistrict, out DateTime firstDeliveryDateTime)
        {
            cityDistrict = null;
            firstDeliveryDateTime = default;

            if (args.Length < 2)
            {
                logger.LogError("Необходимо указать район и время первой доставки.");
                return false;
            }

            cityDistrict = args[0];

            if (!DateTime.TryParse(args[1], out firstDeliveryDateTime))
            {
                logger.LogError("Некорректный формат времени. Убедитесь, что время указано в формате: гггг-ММ-дд ЧЧ:мм:сс.");
                return false;
            }

            return true;
        }

        public static void FilterAndSaveOrders(IConfigurationRoot configuration, ILogger<Program> logger,
            IOrderService orderService, string cityDistrict, DateTime firstDeliveryDateTime)
        {
            try
            {
                var filteredOrders = orderService.FilterOrders(cityDistrict, firstDeliveryDateTime);

                var outputFilePath = configuration["FilteredOrdersFilePath"];

                if (!File.Exists(outputFilePath))
                {
                    using (File.Create(outputFilePath)) { }
                }

                orderService.SaveFilteredOrders(filteredOrders, outputFilePath);

                logger.LogInformation($"Отфильтрованные заказы сохранены в {outputFilePath}.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Произошла ошибка при обработке заказов: {ex.Message}");
            }
        }

        private static void ConfigureLogging(string logDirectory)
        {
            string logFilePath = Path.Combine(logDirectory, "delivery.log");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        private static string GetOrCreateLogDirectory()
        {
            string logDirectory = "logs";
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            return logDirectory;
        }

        private static ServiceProvider ConfigureServices(IConfiguration configuration)
        {
            return new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddSerilog();
                    builder.SetMinimumLevel(LogLevel.Debug);
                })
                .AddSingleton<IOrderRepository>(new OrderRepository(
                    configuration["OrdersFilePath"], configuration["FilteredOrdersFilePath"]))
                .AddTransient<IOrderService, OrderService>()
                .BuildServiceProvider();
        }
    }
}