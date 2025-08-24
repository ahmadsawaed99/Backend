using System;
using Backend.Models;

namespace Backend.Data;

public class Business
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string UserId { get; set; }
    public string? BusinessHours { get; set; }
    public virtual AppUser? User { get; set; }
    public virtual List<Customer>? Customers { get; set; } = [];
    public virtual List<Service> Services { get; set; } = [];
}
