// Created on 30/11/2021 10:48 by Andrey Laserson

namespace Lidemia.DataAccess.Abstract;

public interface IHasMetadata<T>
{
    T Metadata { get; set; }
}