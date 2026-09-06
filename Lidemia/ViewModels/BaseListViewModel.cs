// Created on 06/03/2021 12:13 by Andrey Laserson

using Lidemia.Core.Models.Misc;

namespace Lidemia.ViewModels;

public class BaseListViewModel<TModel, TFilter> : BasePagedListModel<TModel>
{
    public TFilter? Filter { get; set; }
}