using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using DeliveryService.Application.Interfaces.Repositories;
using DeliveryService.Core.Entities;

namespace DeliveryService.Infrastructure.FileStorage
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _inputFilePath;
        private readonly string _outputFilePath;

        public OrderRepository(string inputFilePath, string outputFilePath)
        {
            _inputFilePath = inputFilePath;
            _outputFilePath = outputFilePath;
        }

        public List<Order> GetOrders()
        {
            try
            {
                
                using var reader = new StreamReader(_inputFilePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                return csv.GetRecords<Order>().ToList();
            }
            catch (FileNotFoundException)
            {
                throw new Exception($"Файл {_inputFilePath} не найден.");
            }
            catch (UnauthorizedAccessException)
            {
                throw new Exception($"Нет доступа к файлу {_inputFilePath}.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Произошла ошибка при чтении файла: {ex.Message}");
            }
        }

        public void SaveOrders(List<Order> orders)
        {
            using var writer = new StreamWriter(_outputFilePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(orders);
        }
    }
}