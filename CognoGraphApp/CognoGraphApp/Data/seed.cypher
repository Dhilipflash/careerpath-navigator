// Roles
MERGE (junior:Role {name: "Junior Software Developer"})
SET junior.description = "Builds features with guidance"

MERGE (software:Role {name: "Software Engineer"})
SET software.description = "Designs and builds production software"

MERGE (senior:Role {name: "Senior Software Engineer"})
SET senior.description = "Leads technical design and mentors developers"

MERGE (lead:Role {name: "Tech Lead"})
SET lead.description = "Guides a team and makes architecture decisions"

MERGE (manager:Role {name: "Engineering Manager"})
SET manager.description = "Leads engineering teams and delivery"

// Skills
MERGE (csharp:Skill {name: "C#"})
MERGE (dotnet:Skill {name: ".NET"})
MERGE (sql:Skill {name: "SQL"})
MERGE (systemDesign:Skill {name: "System Design"})
MERGE (leadership:Skill {name: "Leadership"})
MERGE (cloud:Skill {name: "Cloud Fundamentals"})

// Courses
MERGE (course1:Course {name: "C# and .NET Foundations", provider: "Microsoft Learn"})
MERGE (course2:Course {name: "SQL for Developers", provider: "Khan Academy"})
MERGE (course3:Course {name: "System Design Basics", provider: "Educative"})
MERGE (course4:Course {name: "Leading Technical Teams", provider: "Coursera"})

// Career transitions - multi-hop graph path
MERGE (junior)-[:CAN_MOVE_TO {typicalYears: 2}]->(software)
MERGE (software)-[:CAN_MOVE_TO {typicalYears: 3}]->(senior)
MERGE (senior)-[:CAN_MOVE_TO {typicalYears: 2}]->(lead)
MERGE (lead)-[:CAN_MOVE_TO {typicalYears: 3}]->(manager)

// Skills required for each role
MERGE (junior)-[:REQUIRES]->(csharp)
MERGE (junior)-[:REQUIRES]->(dotnet)

MERGE (software)-[:REQUIRES]->(csharp)
MERGE (software)-[:REQUIRES]->(dotnet)
MERGE (software)-[:REQUIRES]->(sql)

MERGE (senior)-[:REQUIRES]->(systemDesign)
MERGE (senior)-[:REQUIRES]->(cloud)

MERGE (lead)-[:REQUIRES]->(systemDesign)
MERGE (lead)-[:REQUIRES]->(leadership)

MERGE (manager)-[:REQUIRES]->(leadership)

// Courses teaching skills
MERGE (course1)-[:TEACHES]->(csharp)
MERGE (course1)-[:TEACHES]->(dotnet)
MERGE (course2)-[:TEACHES]->(sql)
MERGE (course3)-[:TEACHES]->(systemDesign)
MERGE (course3)-[:TEACHES]->(cloud)
MERGE (course4)-[:TEACHES]->(leadership)