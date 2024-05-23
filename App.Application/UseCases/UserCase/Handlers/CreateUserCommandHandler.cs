using App.Application.Abstractions;
using App.Application.UseCases.UserCase.Commands;
using App.Domain.Entities.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.UseCases.UserCase.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponseModel>
    {
        private readonly IAppDbContext _appDbContext;

        public CreateUserCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponseModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if(request == null)
            {
                return new ResponseModel()
                {
                    Message = "NoContentException!?",
                    StatusCode = 204,
                    IsSuccess = true,
                };
            }

            else
            {
                var user = new UserModel()
                {
                    FullName = request.FullName,
                    UserName = request.UserName,
                    Age = request.Age,
                    Email = request.Email,
                    Password = request.Password
                };
                await _appDbContext.Users.AddAsync(user);
                await _appDbContext.SaveChangesAsync(cancellationToken);

                return new ResponseModel()
                {
                    Message = "User Created !",
                    StatusCode = 200,
                    IsSuccess = true,
                };
            }
        }
    }
}
