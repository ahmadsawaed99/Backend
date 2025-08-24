using Backend.Data;

namespace Backend.Models;

public class Customer
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Mobile { get; set; }
    public virtual List<Business>? Businesses { get;} = [];
}
