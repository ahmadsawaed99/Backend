using System;
using Backend.Models;

namespace Backend.Data;

public class BusinessCustomer
{
    public int BusinessId { get; set; }
    public int CustomerId { get; set; }
    public virtual Business business { get; set; } = null!;
    public virtual Customer Customer { get; set; } = null!;
}
