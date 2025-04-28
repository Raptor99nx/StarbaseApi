namespace StargateAPI.Models
{
    public class Astronaut : Person
    {
        public string Rank { get; set; } = null!;

        public string DutyTitle { get; set; } = null!;

        public DateOnly? CareerStartDate { get; set; }

        public DateOnly? CareerEndDate { get; set; }
    }
}
