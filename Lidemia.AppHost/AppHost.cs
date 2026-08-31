var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Lidemia>("lidemia");

builder.AddProject<Projects.Lidemia_Storage>("lidemia-storage");

builder.AddProject<Projects.Lidemia_Admin>("lidemia-admin");

builder.Build().Run();
