// Created on 20/12/2021 23:15 by shell

namespace Lidemia.Core.Models.Base;

public class AbstractIdEntity<T> where T : notnull
{
    public T Id { get; set; } = default!;
}