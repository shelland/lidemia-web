// Created on 14/12/2021 23:46 by Andrey Laserson

using System.Security.Cryptography;
using Lidemia.Common.BusinessLogic.Services.App.Abstract;
using Lidemia.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.App;

[ServiceDescriptor<IOrderNumberGenerator>(ServiceLifetime.Singleton)]
public class OrderNumberGenerator : IOrderNumberGenerator
{
    public async Task<string> Generate()
    {
        var orderPrefix = RandomNumberGenerator.GetString(SecurityHelper.CapitalChars, 1);
        var orderNumericValue = RandomNumberGenerator.GetInt32(100_000_000, 999_999_999);

        var number = $"{orderPrefix}{orderNumericValue}";
        return await Task.FromResult(number);
    }
}