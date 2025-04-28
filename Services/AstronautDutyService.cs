using System.Net;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Interfaces;
using StargateAPI.Models;

namespace StargateAPI.Services
{
    public class AstronautDutyService : IAstronautDutyService
    {
        private readonly AstroActsContext _astroActsDbContext;

        public AstronautDutyService(AstroActsContext astroActsDbContext)
        {
            _astroActsDbContext = astroActsDbContext;
        }

        public async Task<List<AstronautDutyTour>> GetAllByNameAsync(string firstName, string lastName)
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

            List<AstronautDutyTour> astronautDuties = await _astroActsDbContext.AstronautDuties
                .Where(row => row.Person.FirstName.ToLower() == firstName.ToLower() && row.Person.LastName.ToLower() == lastName.ToLower())
                .Select(ad => new AstronautDutyTour
                {
                    Id = ad.Id,
                    DutyTitle = ad.DutyTitle.Name,
                    Rank = ad.Rank.Name,
                    StartDate = ad.StartDate,
                    EndDate = ad.EndDate
                })
                .OrderBy(row => row.StartDate)
                .AsNoTracking()
                .ToListAsync();

            return astronautDuties;
        }

        public async Task<AstronautDuty> Add(AstronautDuty astronautDuty)
        {
            if (astronautDuty.DutyTitleId <= 0 || astronautDuty.PersonId <= 0 || astronautDuty.RankId <= 0)
            {
                List<string> missingParameters = new List<string>();

                if (astronautDuty.DutyTitleId <= 0)
                    missingParameters.Add("DutyTitle");

                if (astronautDuty.PersonId <= 0)
                    missingParameters.Add("Person");

                if (astronautDuty.RankId <= 0)
                    missingParameters.Add("Rank");

                throw new ApiException("Missing Parameter" + (missingParameters.Count > 1 ? "s: " : ": ") + string.Join(", ", missingParameters), HttpStatusCode.BadRequest);
            }

            Person? person = await _astroActsDbContext.People.AsNoTracking().FirstOrDefaultAsync(p => p.Id == astronautDuty.PersonId);

            if (person is null)
                throw new ApiException("The Person associated with the AstronautDuty does not exist.", HttpStatusCode.BadRequest);

            // Check for a previous AstronautDuty record.
            AstronautDuty? previousAstronautDuty = await _astroActsDbContext.AstronautDuties
                .Where(row => row.PersonId == astronautDuty.PersonId)
                .OrderByDescending(row => row.Id)
                .FirstOrDefaultAsync();

            // If exists, update the EndDate to one day before StartDate of incoming AstronautDuty.
            if (previousAstronautDuty is not null)
            {
                // Do not add the incoming AstronautDuty if the StartDate is before or the same as the previous AstronautDuty.
                if (previousAstronautDuty.StartDate >= astronautDuty.StartDate)
                    throw new ApiException("The Astronaut Duty StartDate cannot be before the previous AstronautDuty StartDate.", HttpStatusCode.BadRequest);

                previousAstronautDuty.EndDate = astronautDuty.StartDate.AddDays(-1);
            }

            // Ensure the current EndDate is null.
            astronautDuty.EndDate = null;

            _astroActsDbContext.AstronautDuties.Add(astronautDuty);

            // Check for an AccountDetail record related to the Person.
            AstronautDetail? astronautDetail = await _astroActsDbContext.AstronautDetails
                .FirstOrDefaultAsync(row => row.Person.Id == astronautDuty.PersonId);

            // If not exists, create a new AccountDetail record and set the CareerStartDate.
            if (astronautDetail is null)
            {
                astronautDetail = new AstronautDetail { CareerStartDate = astronautDuty.StartDate };

                _astroActsDbContext.AstronautDetails.Add(astronautDetail);
            }

            astronautDetail.PersonId = astronautDuty.PersonId;
            astronautDetail.RankId = astronautDuty.RankId;
            astronautDetail.DutyTitleId = astronautDuty.DutyTitleId;

            // If the incoming AstronautDuty is Retire,
            // update the AstronautDetail CareerEndDate to one day before StartDate of incoming AstronautDuty.
            if (astronautDuty.DutyTitleId == _astroActsDbContext.DutyTitles.First(row => row.Name.ToUpper() == "RETIRED").Id)
            {
                astronautDetail.CareerEndDate = astronautDuty.StartDate.AddDays(-1);
            }

            await _astroActsDbContext.SaveChangesAsync();

            return astronautDuty;
        }
    }
}
