﻿using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Enums;
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


		public DbSet<CashRegister> CashRegisters { get; set; }
		public DbSet<CashRegisterDetail> CashRegisterDetails { get; set; }
		public DbSet<Bank> Banks { get; set; }
		public DbSet<BankDetail> BankDetails { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<CustomerDetail> CustomerDetails { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductDetail> ProductDetails { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			#region CashRegister
			modelBuilder.Entity<CashRegister>().Property(p => p.DepositAmount).HasColumnType("money");
			modelBuilder.Entity<CashRegister>().Property(p => p.WithdrawalAmount).HasColumnType("money");
			modelBuilder.Entity<CashRegister>()
				.Property(p => p.CurrencyType)
				.HasConversion(type => type.Value, value => CurrencyTypeEnum.FromValue(value));
			modelBuilder.Entity<CashRegister>().HasQueryFilter(filter => !filter.IsDeleted);
			modelBuilder.Entity<CashRegister>()
				.HasMany(p => p.Details)
				.WithOne()
				.HasForeignKey(p => p.CashRegisterId);
			#endregion

			#region CashRegisterDetail
			modelBuilder.Entity<CashRegisterDetail>().Property(p => p.DepositAmount).HasColumnType("money");
			modelBuilder.Entity<CashRegisterDetail>().Property(p => p.WithdrawalAmount).HasColumnType("money");
			#endregion



			#region Bank
			modelBuilder.Entity<Bank>().Property(p => p.DepositAmount).HasColumnType("money");
			modelBuilder.Entity<Bank>().Property(p => p.WithdrawalAmount).HasColumnType("money");
			modelBuilder.Entity<Bank>()
				.Property(p => p.CurrencyType)
				.HasConversion(type => type.Value, value => CurrencyTypeEnum.FromValue(value));
			modelBuilder.Entity<Bank>().HasQueryFilter(filter => !filter.IsDeleted);
			modelBuilder.Entity<Bank>()
			   .HasMany(p => p.Details)
			   .WithOne()
			   .HasForeignKey(p => p.BankId);
			#endregion

			#region BankDetail
			modelBuilder.Entity<BankDetail>().Property(p => p.DepositAmount).HasColumnType("money");
			modelBuilder.Entity<BankDetail>().Property(p => p.WithdrawalAmount).HasColumnType("money");
			#endregion

			#region Customer
			modelBuilder.Entity<Customer>().Property(p => p.DepositAmount).HasColumnType("money");
			modelBuilder.Entity<Customer>().Property(p => p.WithdrawalAmount).HasColumnType("money");
			modelBuilder.Entity<Customer>().Property(p => p.Type)
				.HasConversion(type => type.Value, value => CustomerTypeEnum.FromValue(value));
			modelBuilder.Entity<Customer>().HasQueryFilter(filter => !filter.IsDeleted);
			#endregion

			#region CustomerDetail
			modelBuilder.Entity<CustomerDetail>().Property(p => p.DepositAmount).HasColumnType("money");
			modelBuilder.Entity<CustomerDetail>().Property(p => p.WithdrawalAmount).HasColumnType("money");
			modelBuilder.Entity<CustomerDetail>().Property(p => p.Type)
			   .HasConversion(type => type.Value, value => CustomerDetailTypeEnum.FromValue(value));
			#endregion

			#region ProductDetail
			modelBuilder.Entity<ProductDetail>().Property(p => p.Deposit).HasColumnType("decimal(7,2)");
			modelBuilder.Entity<ProductDetail>().Property(p => p.Withdrawal).HasColumnType("decimal(7,2)");
			modelBuilder.Entity<ProductDetail>().Property(p => p.Price).HasColumnType("money");
			#endregion

			#region Product
			modelBuilder.Entity<Product>().HasQueryFilter(filter => !filter.IsDeleted);
			modelBuilder.Entity<Product>().Property(p => p.Deposit).HasColumnType("decimal(7,2)");
			modelBuilder.Entity<Product>().Property(p => p.Withdrawal).HasColumnType("decimal(7,2)");
			#endregion
		}

	}
	
}
