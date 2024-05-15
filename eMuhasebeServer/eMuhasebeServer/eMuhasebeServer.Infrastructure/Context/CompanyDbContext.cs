using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eMuhasebeServer.Infrastructure.Context
{
	internal sealed class CompanyDbContext : DbContext, IUnitOfWorkCompany
	{
		#region Connection
		private string connectionString = string.Empty;

		public CompanyDbContext(Company company)
		{
			CreateConnectionStringWithCompany(company);
		}

		public CompanyDbContext(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
		{
			CreateConnectionString(httpContextAccessor, context);
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(connectionString);
		}

		private void CreateConnectionString(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
		{
			if (httpContextAccessor.HttpContext is null) return;

			string? companyId = httpContextAccessor.HttpContext.User.FindFirstValue("CompanyId");
			if (string.IsNullOrEmpty(companyId)) return;

			Company? company = context.Companies.Find(Guid.Parse(companyId));
			if (company is null) return;

			CreateConnectionStringWithCompany(company);
		}

		private void CreateConnectionStringWithCompany(Company company)
		{
			if (string.IsNullOrEmpty(company.Database.UserId))
			{
				connectionString =
				$"Host={company.Database.Server};" +
				$"Port={company.Database.Port};" +
				$"Database={company.Database.DatabaseName};" +
				"Username=postgres;" + // Varsayılan kullanıcı adı
				"Password=your_password_here;" + // Buraya PostgreSQL şifrenizi ekleyin
				"Pooling=true;";
			}
			else
			{
				connectionString =
				$"Host={company.Database.Server};" +
				$"Port={company.Database.Port};" +
				$"Database={company.Database.DatabaseName};" +
				$"Username={company.Database.UserId};" +
				$"Password={company.Database.Password};" +
				"Pooling=true;";
			}
		}
		#endregion

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

		}

	}
	
}
