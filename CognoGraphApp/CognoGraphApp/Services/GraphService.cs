using CognoGraphApp.Models;
using Neo4j.Driver;
using System.Linq;

namespace CognoGraphApp.Services;

public class GraphService
{
    private readonly IDriver _driver;

    public GraphService(IDriver driver)
    {
        _driver = driver;
    }

    public async Task<List<string>> GetRolesAsync()
    {
        await using var session = _driver.AsyncSession();

        var cursor = await session.RunAsync(@"
            MATCH (role:Role)
            RETURN role.name AS name
            ORDER BY name");

        return await cursor.ToListAsync(record =>
            record["name"].As<string>());
    }

    public async Task<CareerPathResult?> FindCareerPathAsync(
    string currentRole,
    string targetRole)
    {
        await using var session = _driver.AsyncSession();

        // Reads all role names directly - same reliable query used by dropdowns.
        var rolesCursor = await session.RunAsync(@"
        MATCH (role:Role)
        RETURN role.name AS name, role.level AS level
        ORDER BY level");

        var allRoles = await rolesCursor.ToListAsync(record => new
        {
            Name = record["name"].As<string>(),
            Level = record["level"].As<int>()
        });

        var current = allRoles.FirstOrDefault(role => role.Name == currentRole);
        var target = allRoles.FirstOrDefault(role => role.Name == targetRole);

        if (current is null || target is null)
        {
            return null;
        }

        // Actual required multi-hop graph traversal.
        var stepsCursor = await session.RunAsync(@"
        MATCH path = (start:Role {name: $currentRole})
            -[:CAN_MOVE_TO*1..5]->
            (target:Role {name: $targetRole})
        RETURN length(path) AS steps",
            new { currentRole, targetRole });

        var stepValues = await stepsCursor.ToListAsync(record =>
            record["steps"].As<int>());

        if (!stepValues.Any())
        {
            return null;
        }

        var result = new CareerPathResult
        {
            Roles = allRoles
                .Where(role => role.Level >= current.Level &&
                               role.Level <= target.Level)
                .OrderBy(role => role.Level)
                .Select(role => role.Name)
                .ToList(),

            Steps = stepValues[0]
        };

        var skillCursor = await session.RunAsync(@"
        MATCH (target:Role {name: $targetRole})-[:REQUIRES]->(skill:Skill)
        OPTIONAL MATCH (course:Course)-[:TEACHES]->(skill)
        RETURN skill.name AS skillName,
               collect(DISTINCT course.name) AS courses
        ORDER BY skillName",
            new { targetRole });

        result.Skills = await skillCursor.ToListAsync(record =>
            new SkillRecommendation
            {
                SkillName = record["skillName"].As<string>(),
                Courses = record["courses"].As<List<string>>()
            });

        return result;
    }
}