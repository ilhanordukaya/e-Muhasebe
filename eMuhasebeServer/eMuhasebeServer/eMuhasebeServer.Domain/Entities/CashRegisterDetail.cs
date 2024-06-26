﻿using eMuhasebeServer.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMuhasebeServer.Domain.Entities
{
	public sealed class CashRegisterDetail : Entity
	{
		public Guid CashRegisterId { get; set; }
		public DateOnly Date { get; set; }
		public string Description { get; set; } = string.Empty;
		public decimal DepositAmount { get; set; } //Giriş
		public decimal WithdrawalAmount { get; set; } //Çıkış
		public Guid? CashRegisterDetailId { get; set; }
		public Guid? BankDetailId { get; set; }
		public Guid? CustomerDetailId { get; set; }
	}
}
