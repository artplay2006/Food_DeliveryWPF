using System;
using System.Collections.Generic;

namespace Food_DeliveryWPF.Models;

public partial class Seller
{
    public string Name { get; set; } = null!;

    public string? Addres { get; set; }

    public string? Category { get; set; }

    public string? ImagePath { get; set; }
}
