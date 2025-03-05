using WeatherProject.API;
using WeatherProject.Bot;

namespace WeatherProject;

class Program
{
    static async Task Main(string[] args)
    {
        WeatherProjectAPI weatherProjectAPI = new WeatherProjectAPI();
        WeatherProjectBot weatherProjectBot = new WeatherProjectBot();

        await weatherProjectAPI.WeatherProjectAPISetup(args);
        await weatherProjectBot.WeatherProjectBotSetup(args);
    }
}
