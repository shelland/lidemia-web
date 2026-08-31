// Created on 18/07/2023 20:47 by shell

using Lidemia.Core.Models.Misc;

namespace Lidemia.Common.Logic.Abstract;

public interface IAppIdService
{
    string GetPrefixedKey();

    AppIdModel GetAppId();
}