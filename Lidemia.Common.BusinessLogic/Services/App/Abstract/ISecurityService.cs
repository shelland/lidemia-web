// Created on 19/2/2024 22:30 by Laserson

using Lidemia.Core.Models.Misc;

namespace Lidemia.Common.BusinessLogic.Services.App.Abstract;

public interface ISecurityService
{
    HashedPasswordModel HashPassword(string password);

    bool ValidatePassword(string clearTextPassword, string hashedPassword, string salt);
}