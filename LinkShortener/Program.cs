using LinkShortener;
using LinkShortener.Components;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
//BL services
builder.Services.AddScoped<Repository>();
builder.Services.AddScoped<ShortenerService>();
builder.Services.AddScoped<Coordinator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//Gets the stored link
app.MapGet("/go/{hash}", (string hash, Coordinator coordinator, HttpContext context) =>
    {
        var link = coordinator.ReadLink(hash);
        //TODO: this does not redirect invalid links properly yet, it redirects to blank
        if (link == null) link = "";

        //uses 301 to indicate a permanent redirect to the client against the hashed url
        context.Response.StatusCode = StatusCodes.Status301MovedPermanently;
        context.Response.Headers.Location = link;

        return Task.CompletedTask;
    })
    .WithName("Redirect");

//Shortens a link
app.MapPost("/shorten", ([FromBody] string link, Coordinator coordinator) =>
    {
        var hash = coordinator.WriteLink(link);
        return $"https://shortlynx.site/go/{hash}";
    })
    .WithName("Shorten");

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();