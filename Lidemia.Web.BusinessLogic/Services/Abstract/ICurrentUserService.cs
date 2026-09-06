// Created on 17/03/2020 14:48 by Andrey Laserson

using Lidemia.Core.Models.Misc;

namespace Lidemia.Web.BusinessLogic.Services.Abstract;

public interface ICurrentUserService
{
    Task<CurrentEntityModel?> GetUser();
}