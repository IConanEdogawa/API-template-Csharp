using App.Domain.Entities.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.UseCases.UserCase.Commands
{
    public class CreateTgUserCommand : IRequest<ResponseModel>
    {
        public long TgId { get; set; }
        public long UserName { get; set;}
    }
}
