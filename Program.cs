using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PhotoGallery;
using PhotoGallery.Models;
using PhotoGallery.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
builder.Services.AddScoped<PasswordAuthService>();
builder.Services.AddScoped<GalleryAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<GalleryAuthStateProvider>());

builder.Services.Configure<GalleryOptions>(
    builder.Configuration.GetSection(GalleryOptions.SectionName));
builder.Services.AddSingleton<GalleryDataUrls>();
builder.Services.AddScoped(_ => new HttpClient());
builder.Services.AddScoped<IPhotoService, BlobGalleryPhotoService>();

await builder.Build().RunAsync();
