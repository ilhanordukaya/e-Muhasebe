using eMuhasebeServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Users.GetAllUsers
{
	internal sealed class GetAllUsersQueryHandler(UserManager<AppUser> userManager) : IRequestHandler<GetAllUsersQuery, Result<List<AppUser>>>
	{
		public async Task<Result<List<AppUser>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
		{
			List<AppUser> users= await userManager.Users
				.Include(p=>p.CompanyUsers!)
				.ThenInclude(s=>s.Company)
				.OrderBy(x=>x.FirstName)
				.ToListAsync(cancellationToken);

			return users;
		}
	}


}
