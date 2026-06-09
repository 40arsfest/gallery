using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PhotoGallery;
using PhotoGallery.Models;
using PhotoGallery.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.Configure<GalleryOptions>(
    builder.Configuration.GetSection(GalleryOptions.SectionName));
builder.Services.AddSingleton<GalleryDataUrls>();
builder.Services.AddScoped(_ => new HttpClient());
builder.Services.AddScoped<IPhotoService, BlobGalleryPhotoService>();

await builder.Build().RunAsync();
