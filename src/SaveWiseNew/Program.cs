using FluentMigrator.Runner;
using FluentValidation;
using SaveWise.Repositories;
using SaveWiseNew.DataAccess.Interfaces;
using SaveWiseNew.DataAccess.Migrations;
using SaveWiseNew.Services;
using SaveWiseNew.Services.Interfaces;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080);
});

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddScoped<IDbConnection>(sp => new Npgsql.NpgsqlConnection(Environment.GetEnvironmentVariable("ConnectionStrings__DbUri")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISqlExecutor, SqlExecutor>();

builder.Services.AddHealthChecks();

builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
                     .AddPostgres()
                     .WithGlobalConnectionString(Environment.GetEnvironmentVariable("ConnectionStrings__DbUri"))
                     .ScanIn(typeof(CreateUsersTable).Assembly).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole());

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var migrator = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    migrator.ListMigrations();
    migrator.MigrateUp();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

public partial class Program { }

