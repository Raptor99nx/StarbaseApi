using System;
using System.Collections.Generic;

namespace StargateAPI.Models;

public partial class AstronautDetail
{
    public int Id { get; set; }

    public int PersonId { get; set; }

    public int RankId { get; set; }

    public int DutyTitleId { get; set; }

    public DateOnly CareerStartDate { get; set; }

    public DateOnly? CareerEndDate { get; set; }

    public virtual DutyTitle DutyTitle { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;

    public virtual Rank Rank { get; set; } = null!;
}
