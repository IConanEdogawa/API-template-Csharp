using App.Application.UseCases.UserCase.Commands;
using App.Application.UseCases.UserCase.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _memoryCache;

        public UserController(IMediator mediator, IMemoryCache memoryCache)
        {
            _mediator = mediator;
            _memoryCache = memoryCache;
        }

        [HttpPost]
        public async Task<IActionResult> Login(CreateTgUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            // Qalesiz ustoz mana shetta man tekshirib olyapman userlarim bormi yo`qmi.
            if (_memoryCache.TryGetValue("AllUsers", out List<User> users))
            {
                // Bor bo`sa qaytaradi. aslida background service hisobiga doim malumot bo`ladi lekin baribir xato bo`masligi uchun.
                return Ok(users);
            }
            else
            {
                // Если данные отсутствуют в кэше, выполняем запрос к базе данных
                // bag`iga  qoldiraman comment lardi o`z xolatida. 
                var result = await _mediator.Send(new GetAllTgUsersQuery());

                if (result is not null && result.Any())
                {
                    // Если данные получены из базы данных, сохраняем их в кэше на 5 минут
                    _memoryCache.Set("AllUsers", result, TimeSpan.FromMinutes(5));
                    return Ok(result);
                }
                else
                {
                    return NoContent();
                }
            }
        }
    }
}
