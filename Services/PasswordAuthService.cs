using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace PhotoGallery.Services;

public sealed class PasswordAuthService(IJSRuntime js, IServiceProvider services)
{
    private const string StorageKey = "gallery-auth";
    private const string ExpectedHash = "6563efa0bcae3031255e90c23d8f4cae99537f260751ca7d95a64f4ec2e5acf2";

    private bool _initialized;

    public bool IsAuthenticated { get; private set; }

    public async Task EnsureInitializedAsync()
    {
        if (_initialized)
            return;

        try
        {
            var stored = await js.InvokeAsync<string?>("sessionStorage.getItem", StorageKey);
            IsAuthenticated = stored == "1";
        }
        catch
        {
            IsAuthenticated = false;
        }

        _initialized = true;
    }

    public async Task<bool> LoginAsync(string password)
    {
        if (!VerifyPassword(password))
            return false;

        await js.InvokeVoidAsync("sessionStorage.setItem", StorageKey, "1");
        IsAuthenticated = true;
        NotifyStateChanged();
        return true;
    }

    public async Task LogoutAsync()
    {
        await js.InvokeVoidAsync("sessionStorage.removeItem", StorageKey);
        IsAuthenticated = false;
        NotifyStateChanged();
    }

    private static bool VerifyPassword(string password)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        var hex = Convert.ToHexString(hash);
        return hex.Equals(ExpectedHash, StringComparison.OrdinalIgnoreCase);
    }

    private void NotifyStateChanged()
    {
        if (services.GetService<AuthenticationStateProvider>() is GalleryAuthStateProvider provider)
            provider.NotifyAuthenticationStateChanged();
    }
}
