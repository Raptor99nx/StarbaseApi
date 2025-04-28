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
    public class AstronautDutyController : ControllerBase
    {
        private readonly ILogger<AstronautDutyController> _logger;
        private readonly IAstronautDutyService _astronautDutyService;

        public AstronautDutyController(ILogger<AstronautDutyController> logger, IAstronautDutyService astronautDutyService)
        {
            _logger = logger;
            _astronautDutyService = astronautDutyService;
        }

        [HttpGet("{FirstName}/{LastName}")]
        public async Task<ActionResult<BaseApiResponse>> GetAstronautDutiesByName(string firstName, string lastName)
        {
            try
            {
                List<AstronautDutyTour>? astronautDuties = await _astronautDutyService.GetAllByNameAsync(firstName, lastName);

                if (astronautDuties is null || astronautDuties.Count == 0)
                {
                    _logger.LogInformation($"GetAstronautDutiesByName ({firstName} {lastName}): No Data Found.");

                    return new BaseApiResponse { Successful = true, HttpStatusCode = HttpStatusCode.NoContent, Message = $"{firstName} {lastName} does not have any Astronaut Duties." };
                }
                else
                {
                    return new BaseApiResponse { Successful = true, HttpStatusCode = HttpStatusCode.OK, Result = astronautDuties };
                }
            }
            catch (ApiException apiException)
            {
                string fullExceptionMessage = UtilityService.GetFullExceptionMessage(apiException);

                _logger.LogError(apiException, $"GetAstronautDutiesByName ({firstName} {lastName}) ApiException: {fullExceptionMessage}");

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

                _logger.LogError(exception, $"GetAstronautDutiesByName ({firstName} {lastName}) Exception: {fullExceptionMessage}");

                return new BaseApiResponse 
                { 
                    Successful = false, 
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Message = $"EXCEPTION: {fullExceptionMessage}"
                };
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseApiResponse>> AddAstronautDuty(AstronautDuty astronautDuty)
        {
            try
            {
                return new BaseApiResponse 
                { 
                    Successful = true, 
                    HttpStatusCode = HttpStatusCode.Created, 
                    Result = await _astronautDutyService.Add(astronautDuty) 
                };
            }
            catch (ApiException apiException)
            {
                string fullExceptionMessage = UtilityService.GetFullExceptionMessage(apiException);

                _logger.LogError(apiException, $"AddAstronautDuty ({JsonSerializer.Serialize(astronautDuty)}) ApiException: {fullExceptionMessage}");

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

                _logger.LogError(exception, $"AddAstronautDuty ({JsonSerializer.Serialize(astronautDuty)}) Exception: {fullExceptionMessage}");

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
