using System.Collections.Generic;

namespace CognoGraphApp.Models;

public class CareerPathResult
{
    public List<string> Roles { get; set; } = new();

    public int Steps { get; set; }

    public List<SkillRecommendation> Skills { get; set; } = new();
}

public class SkillRecommendation
{
    public string SkillName { get; set; } = string.Empty;

    public List<string> Courses { get; set; } = new();
}