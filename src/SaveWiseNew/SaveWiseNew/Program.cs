using FluentMigrator.Runner;
using SaveWise.Migrations;
using SaveWise.Repositories;
using SaveWiseNew.Repositories;
using SaveWiseNew.Service;
using SaveWiseNew.Utils;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Настроить Kestrel только на HTTP (порт 5000)
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); // только HTTP
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddScoped<IDbConnection>(sp =>
                                  new Npgsql.NpgsqlConnection(Environment.GetEnvironmentVariable("ConnectionStrings__DbUri")));
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
//app.MapGet("/", () => "Приложение работает!");

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

