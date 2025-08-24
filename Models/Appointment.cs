using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Models;

public class Appointment
{
    [BsonId]
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ServiceId { get; set; }
    public int CustomerId { get; set; }
    public int BusinessId { get; set; }
}
