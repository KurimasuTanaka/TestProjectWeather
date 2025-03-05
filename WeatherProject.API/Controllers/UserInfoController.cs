using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherProject.Db.DataTransferObjects;
using WeatherProject.Db.Repositories;

namespace WeatherProject.API.Controllers
{
    [Route("/users")]
    [ApiController]
    public class UserInfoController(
        IWeatherHistoryRepository _requestsHistoryRepository, 
        IUserRepository _userRepository,
        ILogger<UserInfoController> _logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(long userId)
        {
            _logger.LogInformation($"Getting user info about a user with id {userId} ...");

            var returnObject =  new 
            {
                User = await _userRepository.Get(userId),
                RequestsHistory = await _requestsHistoryRepository.GetAllByUserId(userId),
            };

            _logger.LogInformation($"User info about a user with id {userId} was successfully received!");

            return Ok(returnObject);
        }

    }
}
