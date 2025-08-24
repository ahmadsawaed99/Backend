using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Models
{
    public class Product
    {
        [BsonId]
        public required string Id { get; set; }
        public int GenderId { get; set; }
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Describtion { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
