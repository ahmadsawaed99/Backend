using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;



namespace Backend.Services
{
    public interface IMongoDBService<T>
    {
        Task<string> AddToDatabase(T t, string collectioName);
        List<T> GetAll(string collectioName);
    }
    public class MongoDBService<T> : IMongoDBService<T>
    {
        private readonly IConfiguration _config;

        public MongoDBService(IConfiguration config)
        {
            _config = config;
        }

        public IMongoDatabase DbConnection()
        {
            MongoClient mongoClient;
            var connectionString = _config["MongoDb:ConnectionString"]!;

            mongoClient = new MongoClient(connectionString);
            var db = mongoClient.GetDatabase(_config["MongoDb:DatabaseName"]!);
            return db;
        }

        public async Task<string> AddToDatabase(T t, string collectioName)
        {
            try
            {
                var collection = DbConnection().GetCollection<T>(collectioName);

                await collection.InsertOneAsync(t);

                var _id = t!.GetType().GetProperty("Id")!.GetValue(t)!.ToString();
                return _id!;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<T> GetAll(string collectioName)
        {
            var collection = DbConnection().GetCollection<T>(collectioName);
            var filter = Builders<T>.Filter.Empty;


            var items = collection.Find(filter).ToList();

            return items;

        }
    }
}
