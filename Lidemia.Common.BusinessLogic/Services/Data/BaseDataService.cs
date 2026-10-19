// Created on 14/10/2026 22:15 by Laserson

using Lidemia.Core.Exceptions;

namespace Lidemia.Common.BusinessLogic.Services.Data;

public class BaseDataService
{
    protected static void ThrowNotAuthorized()
    {
        throw AppFlowException.Unauthorized;
    }
}