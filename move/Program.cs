using MyWorkerService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "Move File";
});
builder.Services.AddSingleton<Worker>();
builder.Services.AddHostedService<Worker>();
var host = builder.Build();
host.Run();
