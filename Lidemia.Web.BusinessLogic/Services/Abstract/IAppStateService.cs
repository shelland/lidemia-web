// Created on 29/12/2021 22:05 by shell

using Lidemia.Core.Models.Misc;

namespace Lidemia.Web.BusinessLogic.Services.Abstract;

public interface IAppStateService
{
    string GetPrefixedKey();

    AppIdModel GetAppId();
}