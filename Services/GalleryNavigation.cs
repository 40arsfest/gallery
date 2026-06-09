using Microsoft.AspNetCore.Components;

namespace PhotoGallery.Services;

/// <summary>
/// Builds hrefs that respect the app base path (e.g. /gallery/ on GitHub Pages).
/// Plain &lt;a href="..."&gt; ignores base href for Blazor enhanced navigation.
/// </summary>
public static class GalleryNavigation
{
    public static string Href(NavigationManager navigation, string appRelativePath)
    {
        var path = appRelativePath.StartsWith('/') ? appRelativePath : "/" + appRelativePath;
        return navigation.ToAbsoluteUri(path).AbsoluteUri;
    }

    public static string Home(NavigationManager navigation) => Href(navigation, "/");

    public static string Year(NavigationManager navigation, string year) =>
        Href(navigation, $"/year/{year}");

    public static string Album(NavigationManager navigation, string year, string albumSlug) =>
        Href(navigation, $"/year/{year}/{albumSlug}");
}
