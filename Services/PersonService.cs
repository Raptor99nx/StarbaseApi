using System.Net;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Interfaces;
using StargateAPI.Models;

namespace StargateAPI.Services
{
    public class PersonService : IPersonService
    {
        private readonly AstroActsContext _astroActsDbContext;

        public PersonService(AstroActsContext astroActsDbContext)
        {
            _astroActsDbContext = astroActsDbContext;
        }

        public async Task<List<Astronaut>> GetAllAsync()
        {
            return await (from p in _astroActsDbContext.People
                          join ad in _astroActsDbContext.AstronautDetails
                          on p.Id equals ad.PersonId into personAstronaut
                          from pa in personAstronaut.DefaultIfEmpty()
                          join dt in _astroActsDbContext.AstronautDuties
                          on p.Id equals dt.Id into personAstronautDuties
                          from pad in personAstronautDuties.DefaultIfEmpty()
                          select new Astronaut
                          {
                              Id = p.Id,
                              FirstName = p.FirstName,
                              LastName = p.LastName,
                              CareerStartDate = pa == null ? null : pa.CareerStartDate,
                              CareerEndDate = pa == null ? null : pa.CareerEndDate,
                              DutyTitle = pa == null ? string.Empty : pa.DutyTitle.Name,
                              Rank = pa == null ? string.Empty : pa.Rank.Name
                          }).AsNoTracking().ToListAsync();
        }

        public async Task<Astronaut> GetByNameAsync(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                List<string> missingParameters = new List<string>();

                if (string.IsNullOrWhiteSpace(firstName))
                    missingParameters.Add("FirstName");

                if (string.IsNullOrWhiteSpace(lastName))
                    missingParameters.Add("LastName");

                throw new ApiException("Missing Parameter" + (missingParameters.Count > 1 ? "s: " : ": ") + string.Join(", ", missingParameters), HttpStatusCode.BadRequest);
            }

            Astronaut? astroPerson = await (from p in _astroActsDbContext.People
                                            join ad in _astroActsDbContext.AstronautDetails
                                            on p.Id equals ad.PersonId into personAstronaut
                                            from pa in personAstronaut.DefaultIfEmpty()
                                            join dt in _astroActsDbContext.AstronautDuties
                                            on p.Id equals dt.Id into personAstronautDuties
                                            from pad in personAstronautDuties.DefaultIfEmpty()
                                            where p.FirstName.ToLower() == firstName.ToLower()
                                            where p.LastName.ToLower() == lastName.ToLower()
                                            select new Astronaut
                                            {
                                                Id = p.Id,
                                                FirstName = p.FirstName,
                                                LastName = p.LastName,
                                                CareerStartDate = pa == null ? null : pa.CareerStartDate,
                                                CareerEndDate = pa == null ? null : pa.CareerEndDate,
                                                DutyTitle = pa == null ? string.Empty : pa.DutyTitle.Name,
                                                Rank = pa == null ? string.Empty : pa.Rank.Name
                                            }).AsNoTracking().FirstOrDefaultAsync();

            if (astroPerson is null)
                throw new ApiException("Person Not Found.", HttpStatusCode.NoContent);

            return astroPerson;
        }

        public async Task<Person> AddAsync(Person person)
        {
            if (string.IsNullOrWhiteSpace(person.FirstName) || string.IsNullOrWhiteSpace(person.LastName))
            {
                List<string> missingParameters = new List<string>();

                if (string.IsNullOrWhiteSpace(person.FirstName))
                    missingParameters.Add("FirstName");

                if (string.IsNullOrWhiteSpace(person.LastName))
                    missingParameters.Add("LastName");

                throw new ApiException("Missing Parameter" + (missingParameters.Count > 1 ? "s: " : ": ") + string.Join(", ", missingParameters), HttpStatusCode.BadRequest);
            }

            if (person.Id == 0 && await GetByNameAsync(person.FirstName, person.LastName) is null)
            {
                _astroActsDbContext.Add(person);

                await _astroActsDbContext.SaveChangesAsync();

                return person;
            }
            else
            {
                return await UpdateAsync(person);
            }
        }

        public async Task<Person> UpdateAsync(Person person)
        {
            if (string.IsNullOrWhiteSpace(person.FirstName) || string.IsNullOrWhiteSpace(person.LastName))
            {
                List<string> missingParameters = new List<string>();

                if (string.IsNullOrWhiteSpace(person.FirstName))
                    missingParameters.Add("FirstName");

                if (string.IsNullOrWhiteSpace(person.LastName))
                    missingParameters.Add("LastName");

                throw new ApiException("Missing Parameter" + (missingParameters.Count > 1 ? "s: " : ": ") + string.Join(", ", missingParameters), HttpStatusCode.BadRequest);
            }

            if (person.Id > 0)
            {
                _astroActsDbContext.Update(person);

                await _astroActsDbContext.SaveChangesAsync();

                return person;
            }
            else
            {
                return await AddAsync(person);
            }
        }
    }
}
