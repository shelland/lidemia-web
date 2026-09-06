// Created on 04/09/2026 19:46 by Laserson

using Lidemia.Common.BusinessLogic.Services.App.Abstract;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Mapping;
using Lidemia.Core.Enums;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Repository.Abstract;

namespace Lidemia.Common.BusinessLogic.Services.Data;

public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    private readonly ISecurityService securityService;

    public UserService(IUserRepository userRepository, ISecurityService securityService)
    {
        this.userRepository = userRepository;
        this.securityService = securityService;
    }

    public async Task<UserModel?> FindUser(string email, string password, EntityType role, CancellationToken cancellationToken)
    {
        var user = await this.userRepository.FindUser(email, role, cancellationToken);

        if (user == null)
        {
            return null;
        }

        var isValidPassword = this.securityService.ValidatePassword(password, user.Password, user.PasswordSalt);
        return isValidPassword ? user.ToModel() : null;
    }
}