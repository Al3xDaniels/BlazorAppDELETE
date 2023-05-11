using BlazorAppDELETE.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<UserCollectionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Use(async (context, next) =>
{
    var userIpAddress = context.Connection.RemoteIpAddress?.ToString();

    // Pass the user's IP address to WeatherForecastService
    var weatherForecastService = context.RequestServices.GetRequiredService<UserCollectionService>();
    weatherForecastService.UserIpAddress = userIpAddress;

    await next.Invoke();
});

app.Run();

// Additional cleanup or configuration code if needed