// Created on 20/04/2021 15:49 by Andrey Laserson

using System.Diagnostics.CodeAnalysis;

namespace Lidemia.Core.Models.Service;

public class CreateEntityResultModel<T>
{
    public CreateEntityResultModel(T entity)
    {
        IsSuccess = true;
        Entity = entity;
    }

    public CreateEntityResultModel(List<string> errors)
    {
        ErrorCodes = errors;
    }

    [MemberNotNullWhen(true, nameof(IsSuccess))]
    public T? Entity { get; set; }

    public bool IsSuccess { get; set; }

    public List<string> ErrorCodes { get; set; } = new();

    public ResultInfo Result
    {
        get
        {
            if (IsSuccess)
            {
                return new ResultInfo<T>(true)
                {
                    Data = Entity
                };
            }

            return new ErrorResultInfo(ErrorCodes);
        }
    }
}