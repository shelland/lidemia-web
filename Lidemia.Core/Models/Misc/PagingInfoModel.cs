// Created on 14/04/2020 13:03 by Andrey Laserson

namespace Lidemia.Core.Models.Misc;

public class PagingInfoModel
{
    public PagingInfoModel()
    {
        Page = 1;
    }

    public PagingInfoModel(int page)
    {
        Page = page;
    }

    public PagingInfoModel(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    public int Page { get; set; }

    public int PageSize { get; set; } = 20;
}