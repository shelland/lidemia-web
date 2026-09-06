// Created on 02/09/2026 19:59 by Laserson

using Lidemia.Core.Models.Base;

namespace Lidemia.Core.Models.Domain;

public class CustomerModel : AbstractEntity<long>
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public UserModel User { get; set; } = null!;
}