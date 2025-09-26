using MudBlazor.Services;
using GreenPastures.BlogBurster.Components;
using GreenPastures.BlogBurster.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add HttpClient and Blog Services
builder.Services.AddHttpClient();

// Register the WordPress API service
builder.Services.AddScoped<IWordPressBlogService, WordPressBlogService>();
builder.Services.AddScoped<IBlogService, WordPressBlogService>(); // Use WordPress API service directly

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
