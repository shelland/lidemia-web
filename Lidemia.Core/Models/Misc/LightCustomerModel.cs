// Created on 30/10/2026 19:54 by Laserson

namespace Lidemia.Core.Models.Misc;

public record LightCustomerModel
(
    long Id,
    string FirstName,
    string LastName
)
{
    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
};