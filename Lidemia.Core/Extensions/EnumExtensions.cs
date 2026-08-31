// Created on 21/01/2021 14:42 by Andrey Laserson

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lidemia.Core.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum enumValue)
    {
        var fi = enumValue.GetType().GetField(enumValue.ToString());

        if (fi == null)
        {
            return string.Empty;
        }

        var attribute = fi.GetCustomAttribute<DisplayAttribute>(false);

        if (attribute == null)
        {
            return enumValue.ToString();
        }

        var manager = new ResourceManager(attribute.ResourceType.NotNull());
        return manager.GetString(attribute.Name.NotNull()) ?? enumValue.ToString();
    }

    public static IEnumerable<SelectListItem> GetValues<TEnum>(Enum? selected = null) where TEnum : struct, Enum
    {
        return Enum.GetValues<TEnum>().Select(item => new SelectListItem(item.GetDescription(), Convert.ToInt32(item).ToString(), selected?.Equals(item) == true));
    }
}