// Created on 31/01/2021 18:32 by Andrey Laserson

namespace Lidemia.Core.Models.Misc;

public class BasePagedListModel<T>
{
    public IEnumerable<T> Items { get; set; } = null!;

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public int CurrentPage { get; set; } = 1;
}