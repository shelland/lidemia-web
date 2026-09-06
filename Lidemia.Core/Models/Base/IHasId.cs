// Created on 27/11/2021 16:39 by Andrey Laserson

namespace Lidemia.Core.Models.Base;

public interface IHasId<T> where T : notnull
{
    T Id { get; set; }
}