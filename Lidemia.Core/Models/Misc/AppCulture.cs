// Created on 29/12/2021 22:10 by shell

namespace Lidemia.Core.Models.Misc;

public record AppCulture(
    string Culture,
    string DateFormat,
    string CalendarFormat,
    string CalendarTimeFormat
);