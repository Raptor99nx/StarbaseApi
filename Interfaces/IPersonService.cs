using StargateAPI.Models;

namespace StargateAPI.Interfaces
{
    public interface IPersonService
    {
        Task<Person> AddAsync(Person person);
        Task<List<Astronaut>> GetAllAsync();
        Task<Astronaut> GetByNameAsync(string firstName, string lastName);
        Task<Person> UpdateAsync(Person person);
    }
}