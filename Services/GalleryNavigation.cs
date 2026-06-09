using Microsoft.AspNetCore.Components;

namespace PhotoGallery.Services;

/// <summary>
/// Builds hrefs that respect the app base path (e.g. /gallery/ on GitHub Pages).
/// Paths must not start with "/" when combined with BaseUri — .NET Uri treats
/// "/year/2001" as absolute from the host root, not from the app base.
/// </summary>
public static class GalleryNavigation
{
    public static string Href(NavigationManager navigation, string appRelativePath)
    {
        var relative = appRelativePath.TrimStart('/');
        return new Uri(new Uri(navigation.BaseUri), relative).AbsoluteUri;
    }

    public static string Home(NavigationManager navigation) =>
        new Uri(new Uri(navigation.BaseUri), ".").AbsoluteUri;

    public static string Year(NavigationManager navigation, string year) =>
        Href(navigation, $"year/{Uri.EscapeDataString(year)}");

    public static string Album(NavigationManager navigation, string year, string albumSlug) =>
        Href(navigation, $"year/{Uri.EscapeDataString(year)}/{Uri.EscapeDataString(albumSlug)}");
}
