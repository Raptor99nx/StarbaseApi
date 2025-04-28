using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Interfaces;
using StargateAPI.Models;
using StargateAPI.Services;

namespace StargateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonService _personService;

        public PersonController(ILogger<PersonController> logger, IPersonService personService)
        {
            _logger = logger;
            _personService = personService;
        }

        [HttpGet("{All}")]
        public async Task<ActionResult<BaseApiResponse>> GetAllPeople()
        {
            try
            {
                List<Astronaut> astroPeople = await _personService.GetAllAsync();

                return new BaseApiResponse 
                { 
                    Successful = true, 
                    HttpStatusCode = HttpStatusCode.OK, 
                    Result = astroPeople 
                };
            }
            catch (ApiException apiException)
            {
                string fullExceptionMessage = UtilityService.GetFullExceptionMessage(apiException);

                _logger.LogError(apiException, $"GetAllPeople ApiException: {fullExceptionMessage}");

                return new BaseApiResponse 
                { 
                    Successful = false, 
                    HttpStatusCode = apiException.StatusCode,
                    Message = $"EXCEPTION: {fullExceptionMessage}"
                };
            }
            catch (Exception exception)
            {
                string fullExceptionMessage = UtilityService.GetFullExceptionMessage(exception);

                _logger.LogError(exception, $"GetAllPeople Exception: {fullExceptionMessage}");

                return new BaseApiResponse 
                { 
                    Successful = false, 
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Message = $"EXCEPTION: {fullExceptionMessage}"
                };
            }
        }

        [HttpGet("{FirstName}/{LastName}")]
        public async Task<ActionResult<BaseApiResponse>> GetPersonByName(string firstName, string lastName)
        {
            try
            {
                Astronaut astroPerson = await _personService.GetByNameAsync(firstName, lastName);

                if (astroPerson is null)
                {
                    _logger.LogInformation($"GetPersonByName ({firstName} {lastName}): Person Not Found.");

                    return new BaseApiResponse 
                    { 
                        Successful = true, 
                        HttpStatusCode = HttpStatusCode.NoContent, 
                        Message = $"The Person with name {firstName} {lastName} was not found." 
                    };
                }
                else
                {
                    return new BaseApiResponse 
                    { 
                        Successful = true, 
                        HttpStatusCode = HttpStatusCode.OK, 
                        Result = astroPerson 
                    };
                }
            }
            catch (ApiException apiException)
            {
                string fullExceptionMessage = UtilityService.GetFullExceptionMessage(apiException);

                _logger.LogError(apiException, $"GetPersonByName ({firstName} {lastName}) ApiException: {fullExceptionMessage}");

                return new BaseApiResponse 
                { 
                    Successful = false, 
                    HttpStatusCode = apiException.StatusCode,
                    Message = $"EXCEPTION: {fullExceptionMessage}"
                };
            }
            catch (Exception exception)
            {
                string fullExceptionMessage = UtilityService.GetFullExceptionMessage(exception);

                _logger.LogError(exception, $"GetPersonByName ({firstName} {lastName}) Exception: {fullExceptionMessage}");

                return new BaseApiResponse 
                { 
                    Successful = false, 
                    HttpStatusCode = HttpStatusCode.InternalServerError, 
                    Message = $"EXCEPTION: {fullExceptionMessage}"
                };
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseApiResponse>> AddPerson(Person person)
        {
            try
            {
                return new BaseApiResponse 
                { 
                    Successful = true, 
                    HttpStatusCode = HttpStatusCode.Created, 
                    Result = await _personService.AddAsync(person) 
                };
            }
            catch (ApiException apiException)
            {
                string fullExceptionMessage = UtilityService.GetFullExceptionMessage(apiException);

                _logger.LogError(apiException, $"AddPerson ({JsonSerializer.Serialize(person)}) ApiException: {fullExceptionMessage}");

                return new BaseApiResponse 
                { 
                    Successful = false, 
                    HttpStatusCode = apiException.StatusCode,
                    Message = $"EXCEPTION: {fullExceptionMessage}"
                };
            }
            catch (Exception exception)
            {
                string fullExceptionMessage = UtilityService.GetFullExceptionMessage(exception);

                _logger.LogError(exception, $"AddPerson ({JsonSerializer.Serialize(person)}) Exception: {fullExceptionMessage}");

                return new BaseApiResponse 
                { 
                    Successful = false, 
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Message = $"EXCEPTION: {fullExceptionMessage}"
                };
            }
        }

        [HttpPut]
        public async Task<ActionResult<BaseApiResponse>> UpdatePerson(Person person)
        {
            try
            {
                return new BaseApiResponse 
                { 
                    Successful = true, 
                    HttpStatusCode = HttpStatusCode.Accepted, 
                    Result = await _personService.UpdateAsync(person) 
                };
            }
            catch (ApiException apiException)
            {
                string fullExceptionMessage = UtilityService.GetFullExceptionMessage(apiException);

                _logger.LogError(apiException, $"UpdatePerson ({JsonSerializer.Serialize(person)}) ApiException: {fullExceptionMessage}");

                return new BaseApiResponse 
                { 
                    Successful = false, 
                    HttpStatusCode = apiException.StatusCode,
                    Message = $"EXCEPTION: {fullExceptionMessage}"
                };
            }
            catch (Exception exception)
            {
                string fullExceptionMessage = UtilityService.GetFullExceptionMessage(exception);

                _logger.LogError(exception, $"UpdatePerson ({JsonSerializer.Serialize(person)}) Exception: {fullExceptionMessage}");

                return new BaseApiResponse 
                { 
                    Successful = false, 
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Message = $"EXCEPTION: {fullExceptionMessage}"
                };
            }
        }
    }
}
