﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMuhasebeServer.Domain.DTOs
{
	public sealed record InvoiceDetailDto
	{
		public Guid ProductId { get; set; }
		public decimal Quantity { get; set; }
		public decimal Price { get; set; }
	}
}
