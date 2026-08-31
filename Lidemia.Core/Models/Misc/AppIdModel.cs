// Created on 23/7/2024 22:47 by Laserson

namespace Lidemia.Core.Models.Misc;

public record AppIdModel(
    string AppName,
    string Module,
    string Environment,
    string Version
);