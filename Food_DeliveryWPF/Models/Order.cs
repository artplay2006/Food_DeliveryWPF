using System;
using System.Collections.Generic;

namespace Food_DeliveryWPF.Models;

public partial class Order
{
    public int Id { get; set; }

    public double? Paid { get; set; }

    public string? ToDelivery { get; set; }

    public string? OrderTime { get; set; }

    public string? OrderComing { get; set; }

    public string? LoginBuyer { get; set; }
}
