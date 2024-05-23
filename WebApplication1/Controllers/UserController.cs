using App.Application.UseCases.UserCase.Commands;
using App.Application.UseCases.UserCase.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public UserController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }


        [HttpPost]
        public async Task<IActionResult> Login(CreateTgUserCommand command)
        {
            var result = await _mediatr.Send(command);

            if(result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);


        }


        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {

            var result = await _mediatr.Send(new GetAllTgUsersQuery());

            if(result is not null)
            {
                return Ok(result);
            }
            return NoContent();
        }


    }
}
