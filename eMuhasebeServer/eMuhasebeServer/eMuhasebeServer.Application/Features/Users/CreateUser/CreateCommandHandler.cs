using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Events;
using eMuhasebeServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Users.CreateUser
{
	internal sealed class CreateCommandHandler(
		UserManager<AppUser> userManager,
		IMapper mapper,
		IMediator mediator,
		ICompanyUserRepository companyUserRepository,
		IUnitOfWork unitOfWork
		) :
		IRequestHandler<CreateUserCommand, Result<string>>
	{
		public async Task<Result<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
		{
			bool isUserNameExist = await userManager.Users.AnyAsync(x => x.FirstName == request.FirstName, cancellationToken);
			if (isUserNameExist) 
			{
				return Result<string>.Failure("Bu kullanıcı adı mevcuttur.");
			}

			bool isEmailExist= await userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

			if (isEmailExist) 
			{
				return Result<string>.Failure("Bu email başka bir kullanıcı tarafından kullanılmaktadır.");
			}
			AppUser appUser = mapper.Map<AppUser>(request);

			IdentityResult identityResult = await userManager.CreateAsync(appUser,request.Password);

			if (!identityResult.Succeeded)
			{
				return Result<string>.Failure(identityResult.Errors.Select(x=>x.Description).ToList());
			}

			List<CompanyUser> companyUsers = request.CompanyIds.Select(s => new CompanyUser
			{
				AppUserId = appUser.Id,
				CompanyId = s
			}).ToList();

			await companyUserRepository.AddRangeAsync(companyUsers, cancellationToken);
			await unitOfWork.SaveChangesAsync(cancellationToken);

			await mediator.Publish(new AppUserEvent(appUser.Id));
			return "Kullanıcı kaydı başarıyla tamamlandı";


		}
	}

}
