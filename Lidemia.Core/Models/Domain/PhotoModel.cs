// Created on 04/09/2026 18:29 by Laserson

using Lidemia.Core.Models.Base;

namespace Lidemia.Core.Models.Domain;

public class PhotoModel : AbstractEntity<long>
{
    public string? LargeThumbPath { get; set; }

    public string? MediumThumbPath { get; set; }

    public string? SmallThumbPath { get; set; }

    public string? OriginalFilePath { get; set; }
}