using ElectronNET.API;
using Syncfusion.Blazor;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddElectron();
builder.WebHost.UseElectron(args);


if (HybridSupport.IsElectronActive)
{
    // Open the Electron-Window
    Task.Run(async () => {
        var window = await Electron.WindowManager.CreateWindowAsync();
        window.OnClosed += () => {
            Electron.App.Quit();
        };
    });
}

var app = builder.Build();


// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
