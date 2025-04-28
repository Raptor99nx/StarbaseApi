using System;
using System.Collections.Generic;

namespace StargateAPI.Models;

public partial class AstronautDuty
{
    public int Id { get; set; }

    public int PersonId { get; set; }

    public int RankId { get; set; }

    public int DutyTitleId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; } = null;

    public virtual DutyTitle? DutyTitle { get; set; } = null;

    public virtual Person? Person { get; set; } = null;

    public virtual Rank? Rank { get; set; } = null;
}
