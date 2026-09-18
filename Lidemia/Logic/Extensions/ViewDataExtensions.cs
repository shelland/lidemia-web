// Created on 17/09/2026 23:06 by Laserson

using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Lidemia.Logic.Extensions;

public static class ViewDataExtensions
{
    public static void SetTitle(this ViewDataDictionary viewData, string title)
    {
        viewData["Title"] = title;
    }

    public static void SetBigTitle(this ViewDataDictionary viewData, string title, string? subItem = null)
    {
        viewData["LargeTitle"] = title;
        viewData["LargeTitleSubitem"] = subItem;
        viewData.SetTitle(title);
    }

    public static string? GetTitle(this ViewDataDictionary viewData)
    {
        return viewData.TryGetValue("Title", out var title) ? title?.ToString() : null;
    }

    public static string? GetBigTitle(this ViewDataDictionary viewData)
    {
        return viewData.TryGetValue("LargeTitle", out var title) ? title?.ToString() : null;
    }

    public static string? GetSubTitle(this ViewDataDictionary viewData)
    {
        return viewData.TryGetValue("LargeTitleSubitem", out var title) ? title?.ToString() : null;
    }
}