using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;



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
            string template = _config["MongoDb:Template"]!;          // e.g. "mongodb://{0}:{1}@{2}/?tls=true&replicaSet=rs0&readPreference={3}&retryWrites=false"
            string username = _config["MongoDb:Username"]!;
            string password = _config["MongoDb:Password"]!;
            string readPreference = _config["MongoDb:ReadPreference"]!;    // e.g. "secondaryPreferred"
            string clusterEndpoint = _config["MongoDb:ClusterEndpoint"]!;   // e.g. "myapp-dev-docdb.cluster-xxxx.us-east-1.docdb.amazonaws.com:27017"
            string connectionString = string.Format(template, username, password, clusterEndpoint, readPreference);

            string pathToCAFile = _config["MongoDb:PathToCAFile"]!;      // e.g. "global-bundle.pem" or "rds-combined-ca-bundle.pem"

            var settings = MongoClientSettings.FromConnectionString(connectionString);

            // Validate server cert against AWS CA bundle (no Keychain writes)
            settings.SslSettings = new SslSettings
            {
                ServerCertificateValidationCallback = (sender, cert, chain, errors) =>
                {
                    // Fast-path if OS already trusts it
                    if (errors == SslPolicyErrors.None) return true;

                    // Build a chain including the provided CA bundle
                    var bundle = new X509Certificate2Collection();
                    bundle.Import(pathToCAFile);

                    using var xchain = new X509Chain();
                    xchain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                    xchain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
                    foreach (var c in bundle) xchain.ChainPolicy.ExtraStore.Add(c);

                    return xchain.Build(new X509Certificate2(cert!));
                }
            };

            // Optional hardening / stability
            settings.ConnectTimeout = TimeSpan.FromSeconds(10);
            settings.ServerSelectionTimeout = TimeSpan.FromSeconds(10);
            settings.ApplicationName = "Backend";

            var client = new MongoClient(settings);
            return client.GetDatabase(_config["MongoDb:DatabaseName"]!);
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
