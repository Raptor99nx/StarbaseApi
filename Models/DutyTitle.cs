using System;
using System.Collections.Generic;

namespace StargateAPI.Models;

public partial class DutyTitle
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual AstronautDetail? AstronautDetails { get; set; }

    public virtual ICollection<AstronautDuty> AstronautDuties { get; set; } = new List<AstronautDuty>();
}
