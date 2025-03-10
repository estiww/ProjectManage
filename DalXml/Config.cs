﻿namespace Dal;

internal static class Config
{
    static string s_data_config_xml = "data-config";
    internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); }
    internal static int NextDependencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId"); }
    internal static DateTime? ProjectStartDate { get; set; } = null;
    internal static DateTime? ProjectPlannedEndDate { get; set; } = null;
}

