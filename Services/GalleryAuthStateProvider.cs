using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace PhotoGallery.Services;

public sealed class GalleryAuthStateProvider(PasswordAuthService auth) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        await auth.EnsureInitializedAsync();

        if (!auth.IsAuthenticated)
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var identity = new ClaimsIdentity(
            [new Claim(ClaimTypes.Name, "guest")],
            authenticationType: "gallery-password");

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public void NotifyAuthenticationStateChanged() =>
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
}
