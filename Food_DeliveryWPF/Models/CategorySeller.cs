using System;
using System.Collections.Generic;

namespace Food_DeliveryWPF.Models;

public partial class CategorySeller
{
    public string? Category { get; set; }

    public string? NameSeller { get; set; }

    public virtual Seller? NameSellerNavigation { get; set; }
}
