using App.Application.Abstractions;
using App.Application.UseCases.UserCase.Queries;
using App.Domain.Entities.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.UseCases.UserCase.Handlers
{
    public class GetAllTgUsersQueryHandler : IRequestHandler<GetAllTgUsersQuery, List<UserModel>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetAllTgUsersQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<UserModel>> Handle(GetAllTgUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await _appDbContext.Users.ToListAsync(cancellationToken);
            return result;
        }
    }
}
