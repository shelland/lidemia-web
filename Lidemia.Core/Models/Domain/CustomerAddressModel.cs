// Created on 12/10/2026 22:40 by Laserson

namespace Lidemia.Core.Models.Domain;

public class CustomerAddressModel : AddressModel
{
    public string? Comment { get; set; }

    public long CustomerId { get; set; }

    public override string ToString()
    {
        return $"{this.Country.Name}, {this.Region.Name}, {this.City.Name}, {this.StreetName}, {this.HouseNumber}";
    }
}