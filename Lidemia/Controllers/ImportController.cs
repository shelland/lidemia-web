// Created on 12/10/2026 20:29 by Laserson

using CsvHelper;
using CsvHelper.Configuration.Attributes;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Lidemia.Controllers;

[Route("Import")]
public class ImportController : Controller
{
    [HttpPost("Cities")]
    public async Task Cities([FromServices] LidemiaDbContext context, [FromForm] IFormFile file)
    {
        // var file = Request.Form.Files.FirstOrDefault()!;
        await using var fileStream = file.OpenReadStream();
        using var streamReader = new StreamReader(fileStream);
        using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

        var records = csvReader.GetRecords<Town>().ToList();

        var country = new CountryEntity
        {
            Name = "Россия",
            Id = Guid.CreateVersion7()
        };

        context.Countries.Add(country);
        await context.SaveChangesAsync();

        var regions = records.GroupBy(x => x.RegionName);

        foreach (var gr in regions)
        {
            var region = new RegionEntity
            {
                Name = gr.Key,
                Id = Guid.CreateVersion7(),
                Area = gr.First().District,
                CountryId = country.Id
            };

            context.Regions.Add(region);
            await context.SaveChangesAsync();
        }

        foreach (var city in records)
        {
            var region = await context.Regions.FirstOrDefaultAsync(x => x.Name.Equals(city.RegionName));

            context.Cities.Add(new CityEntity
            {
                Id = Guid.CreateVersion7(),
                Latitude = city.Latitude,
                Longitude = city.Longitude,
                Name = city.City,
                IsActive = true,
                RegionId = region!.Id
            });
        }



        //foreach (var rec in records)
        //{
        //    var regExists = await context.Regions.FirstOrDefaultAsync(x => x.Name.Equals(rec.RegionName));

        //    if (regExists == null)
        //    {
        //        regExists = new RegionEntity
        //        {
        //            Name = rec.RegionName,
        //            Area = rec.District,
        //            CountryId = country.Id,
        //            Id = Guid.CreateVersion7()
        //        };
        //        regExists.Cities.Add(new CityEntity
        //        {
        //            Latitude = rec.Latitude,
        //            Longitude = rec.Longitude,
        //            RegionId = regExists.Id,
        //            Id = Guid.CreateVersion7(),
        //            Name = rec.City
        //        });


        //        country.Regions.Add(regExists);
        //    }
        //    else
        //    {
        //        regExists.Cities.Add(new CityEntity
        //        {
        //            Latitude = rec.Latitude,
        //            Longitude = rec.Longitude,
        //            RegionId = regExists.Id,
        //            Id = Guid.CreateVersion7(),
        //            Name = rec.City
        //        });
        //    }


        //}

        await context.SaveChangesAsync();

        return;
    }
}

public class Town
{
    [Name("city")] public string City { get; set; }

    [Name("region_name")] public string RegionName { get; set; }

    [Name("lat")] public double Latitude { get; set; }

    [Name("lon")] public double Longitude { get; set; }

    [Name("federal_district")] public string District { get; set; }
}