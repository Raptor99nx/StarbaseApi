using StargateAPI.Models;

namespace StargateAPI.Interfaces
{
    public interface IAstronautDutyService
    {
        Task<AstronautDuty> Add(AstronautDuty astronautDuty);
        Task<List<AstronautDutyTour>> GetAllByNameAsync(string firstName, string lastName);
    }
}