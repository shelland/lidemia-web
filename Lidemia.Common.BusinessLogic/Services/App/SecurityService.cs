// Created on 19/2/2024 22:31 by Laserson

using System.Security.Cryptography;
using Lidemia.Common.BusinessLogic.Services.App.Abstract;
using Lidemia.Core.Models.Misc;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.App;

[ServiceDescriptor<ISecurityService>(ServiceLifetime.Singleton)]
public class SecurityService : ISecurityService
{
    private const int KeySize = 64;
    private const int Iterations = 350_000;

    private readonly HashAlgorithmName algorithmName = HashAlgorithmName.SHA512;

    public HashedPasswordModel HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(KeySize);
        var hash = CalculateHash(password, salt);

        return new HashedPasswordModel(
            Hash: Convert.ToHexString(hash),
            Salt: Convert.ToHexString(salt)
        );
    }

    public bool ValidatePassword(string clearTextPassword, string hashedPassword, string salt)
    {
        var hashToCompare = CalculateHash(clearTextPassword,Convert.FromHexString(salt));
        return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hashedPassword));
    }

    private byte[] CalculateHash(string clearTextPassword, byte[] salt)
    {
        return Rfc2898DeriveBytes.Pbkdf2(clearTextPassword, salt, Iterations, algorithmName, KeySize);
    }
}