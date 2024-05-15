﻿using eMuhasebeServer.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Companies.CreateCompany
{
	public sealed record CreateCompanyCommand(
	string Name,
	string FullAddress,
	string TaxDepartment,
	string TaxNumber,
	Database Database) : IRequest<Result<string>>;
}