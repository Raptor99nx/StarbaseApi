namespace StargateAPI.Models
{
    public class AstronautDutyTour
    {
        public int Id { get; set; }

        public string Rank { get; set; } = string.Empty;

        public string DutyTitle { get; set; } = string.Empty;

        public DateOnly StartDate { get; set; }

        public DateOnly? EndDate { get; set; }
    }
}
