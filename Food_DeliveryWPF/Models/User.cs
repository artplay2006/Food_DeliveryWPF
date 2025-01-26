using System;
using System.Collections.Generic;

namespace Food_DeliveryWPF.Models;

public partial class User
{
    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Role { get; set; }
}
