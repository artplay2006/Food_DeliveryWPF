using System;
using System.Collections.Generic;

namespace Food_DeliveryWPF.Models;

public partial class Food
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public double? Weight { get; set; }

    public double? Price { get; set; }

    public string? NameSeller { get; set; }

    public string? ImagePath { get; set; }
}
