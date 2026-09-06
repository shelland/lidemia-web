// Created on 29/12/2021 22:09 by shell

using System.Globalization;
using Lidemia.Core.Models.Misc;

namespace Lidemia.Web.BusinessLogic.Services.Abstract;

public interface IAppLocalizationService
{
    public AppCulture CurrentCulture { get; }

    public CultureInfo CurrentCultureInfo { get; }
}