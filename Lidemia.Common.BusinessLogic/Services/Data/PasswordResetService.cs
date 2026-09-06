// Created on 03/09/2026 18:49 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<IPasswordResetService>(ServiceLifetime.Scoped)]
public class PasswordResetService : IPasswordResetService
{
    private readonly IPasswordResetRepository passwordResetRepository;

    public PasswordResetService(IPasswordResetRepository passwordResetRepository)
    {
        this.passwordResetRepository = passwordResetRepository;
    }
}