// Created on 14/12/2021 23:46 by Andrey Laserson

namespace Lidemia.Common.BusinessLogic.Services.App.Abstract;

public interface IOrderNumberGenerator
{
    Task<string> Generate();
}