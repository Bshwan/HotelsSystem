using Microsoft.AspNetCore.SignalR;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddLocalization(option => option.ResourcesPath = "Resources");
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddServerSideBlazor(options =>
{
    options.JSInteropDefaultCallTimeout = TimeSpan.FromHours(1);
});
builder.Services.AddScoped<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddBlazorDownloadFile();
builder.Services.AddBlazoredModal();
builder.Services.AddMudServices();


builder.Services.AddToaster(config =>
{
    //example customizations
    config.PositionClass = Sotsera.Blazor.Toaster.Core.Models.Defaults.Classes.Position.TopRight;
    config.NewestOnTop = false;
    config.ShowTransitionDuration = 1000;
    config.HideTransitionDuration = 1000;
    config.VisibleStateDuration = 2000;
    config.ShowProgressBar = false;
    config.ShowCloseIcon = false;
});
builder.Services.Configure<HubOptions>(options =>
{
    options.MaximumReceiveMessageSize = 1 * 1024 * 1024; // 1MB
});

RequestLocalizationOptions GetLocalizationOptions()
{
    var cultures = builder.Configuration.GetSection("Cultures")
        .GetChildren().ToDictionary(x => x.Key, x => x.Value);

    var supportedCultures = cultures.Keys.ToArray();

    var localizationOptions = new RequestLocalizationOptions()
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);

    return localizationOptions;
}

builder.Host.UseSerilog((ctx, lc) =>
{
    lc.WriteTo.File("log.txt", Serilog.Events.LogEventLevel.Error, rollingInterval: RollingInterval.Month, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}");
    lc.WriteTo.Console(Serilog.Events.LogEventLevel.Verbose, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();


}
//app.UseExceptionHandler("/Error");

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRequestLocalization(GetLocalizationOptions());
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapBlazorHub(options =>
    {
        options.WebSockets.CloseTimeout = new TimeSpan(1, 1, 1);
        options.LongPolling.PollTimeout = new TimeSpan(1, 0, 0);
    });
}
);

app.UseMiddleware<CookieMiddleware>();
app.Run();

