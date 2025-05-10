using FluentMigrator.Runner;
using SaveWise.Migrations;
using SaveWise.Repositories;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDbConnection>(sp =>
                                  new Npgsql.NpgsqlConnection(builder.Configuration.GetConnectionString("DbUri")));
builder.Services.AddScoped<UserRepository>();

builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
                     .AddPostgres()
                     .WithGlobalConnectionString(builder.Configuration.GetConnectionString("DbUri"))
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
