using System;
using System.Collections.Generic;

namespace steam_discovery_platform.Server.Models;

public partial class ApplicationsStaging
{
    public string? Appid { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? IsFree { get; set; }

    public string? ReleaseDate { get; set; }

    public string? RequiredAge { get; set; }

    public string? ShortDescription { get; set; }

    public string? SupportedLanguages { get; set; }

    public string? HeaderImage { get; set; }

    public string? Background { get; set; }

    public string? MetacriticScore { get; set; }

    public string? RecommendationsTotal { get; set; }

    public string? MatSupportsWindows { get; set; }

    public string? MatSupportsMac { get; set; }

    public string? MatSupportsLinux { get; set; }

    public string? MatInitialPrice { get; set; }

    public string? MatFinalPrice { get; set; }

    public string? MatDiscountPercent { get; set; }

    public string? MatCurrency { get; set; }

    public string? MatAchievementCount { get; set; }

    public string? MatPcOsMin { get; set; }

    public string? MatPcProcessorMin { get; set; }

    public string? MatPcMemoryMin { get; set; }

    public string? MatPcGraphicsMin { get; set; }

    public string? MatPcOsRec { get; set; }

    public string? MatPcProcessorRec { get; set; }

    public string? MatPcMemoryRec { get; set; }

    public string? MatPcGraphicsRec { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }
}
