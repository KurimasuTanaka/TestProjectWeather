
using Telegram.Bot;
using Telegram.Bot.Types;
using WeatherProject.Bot;
using WeatherProject.Bot.Services.MessagesSender;
using WeatherProject.Db.DataAccessObjects;
using WeatherProject.Db.DataTransferObjects;
using WeatherProject.Db.Repositories;

namespace WeatherProject.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddUserSecrets<Program>();  

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen();


        builder.Services.AddScoped<IUserRepository, UsersDataAccess>();
        builder.Services.AddScoped<IWeatherHistoryRepository, WeatherHistoryDataAccess>();

        builder.Services.AddWeatherTelegramBot(builder.Configuration["TelegramBot:Token"]);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseSwagger();
        app.UseSwaggerUI();


        app.UseHttpsRedirection();

        //app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }

}
