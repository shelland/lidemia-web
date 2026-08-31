// Created on 21/01/2021 14:44 by Andrey Laserson

using System.ComponentModel;
using System.Resources;

namespace Lidemia.Core.Attributes;

public class LocalizedDescriptionAttribute : DescriptionAttribute
{
    private readonly string resourceKey;
    private readonly ResourceManager resource;

    public LocalizedDescriptionAttribute(string resourceKey, Type resourceType)
    {
        this.resource = new ResourceManager(resourceType);
        this.resourceKey = resourceKey;
    }

    public override string Description
    {
        get
        {
            var displayName = this.resource.GetString(this.resourceKey);

            return string.IsNullOrEmpty(displayName)
                ? $"[[{this.resourceKey}]]"
                : displayName;
        }
    }
}