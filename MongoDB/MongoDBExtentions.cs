using System;
using Backend.Models;
using Backend.Models.HelpersModels;
using Backend.Services;

namespace Backend.MongoDB;

public static class MongoDBExtentions
{
    public static void AddMongoDbDependencies(this IServiceCollection services)
    {
       services.AddTransient<IMongoDBService<Product>, MongoDBService<Product>>();
       services.AddTransient<IMongoDBService<Otp>, MongoDBService<Otp>>();
    }
}
