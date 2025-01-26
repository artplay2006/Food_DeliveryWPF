using System;
using System.Collections.Generic;

namespace Food_DeliveryWPF.Models;

public partial class ContentOrder
{
    public int? IdOrder { get; set; }

    public int? CountFood { get; set; }

    public double? PriceDish { get; set; }

    public int? IdFood { get; set; }

    public int Id { get; set; }
}
