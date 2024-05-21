using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace eMuhasebeServer.Application.Features.CashRegisters.UpdateCashRegisters
{
	public sealed record UpdateCashRegisterCommand(
	Guid Id,
	string Name,
	int CurrencyTypeValue) : IRequest<Result<string>>;
}
