// Created on 24/09/2023 19:26 by shell

namespace Lidemia.Core.Models.Dto.External;

public record GeoIpApiServiceResponseModel(
    double Lat,
    double Lon,
    string? City,
    string? RegionName,
    string? Country,
    int? Offset
);