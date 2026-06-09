# PhotoGallery – 40 år av tveksamma beslut

En statisk Blazor WebAssembly-fotoapp med Tailwind CSS. Visar ett privat fotoarkiv med navigation **År → Album → Foton**, lightbox och slumpat minne.

**Live:** [https://40arsfest.github.io/gallery/](https://40arsfest.github.io/gallery/)

## Krav

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (endast för att bygga Tailwind CSS)

## Köra lokalt

```bash
npm install
npm run build:css

# Kopiera och fyll i din publika blob-URL (ingen connection string):
cp wwwroot/appsettings.Development.json.example wwwroot/appsettings.Development.json

dotnet run
```

`appsettings.Development.json` är gitignored och används bara lokalt. Den innehåller en **publik HTTPS-URL** till blob-containern — aldrig connection string eller kontonycklar.

## Bildstruktur

```text
{år}/{album}/{filnamn}

2004/fest hos pelle 24-05-2004/img1.jpg
```

Metadata (index-JSON) hämtas från blob via `BlobGalleryPhotoService`: `data/index.json`, `data/{år}/index.json`, `data/{år}/{album-slug}.json`.

## GitHub Pages

Appen deployas automatiskt till [40arsfest/gallery](https://github.com/40arsfest/gallery) vid push till `main`.

### Engångsinställning i GitHub

1. **Settings → Pages → Build and deployment → Source:** välj **GitHub Actions**.
2. **Settings → Secrets and variables → Actions → Variables:** skapa  
   `GALLERY_DATA_BASE_URL` = `https://luddephotogallery.blob.core.windows.net/photos/`  
   (publik läs-URL till containern, **inte** en connection string).
3. **Azure Storage → CORS** på blob-kontot: tillåt `GET`/`HEAD` från  
   `https://40arsfest.github.io` (och `http://localhost:5114` för lokal utveckling).

### Pusha kod

```bash
git add .
git commit -m "Deploy gallery to GitHub Pages"
git push -u origin main
```

Workflowen bygger CSS, publicerar Blazor med `BaseHref=/gallery/`, och deployar till Pages.

### Djup-länkar

`wwwroot/404.html` + redirect-script i `index.html` gör att t.ex. `/gallery/year/2004` fungerar vid direktladdning.

## Säkerhet

| OK i klient/repo | Skall **aldrig** finnas här |
|------------------|----------------------------|
| Publik blob-URL (`DataBaseUrl`) | Connection string |
| Publika bild-URL:er i JSON | Account key / SAS med skrivrättighet |
| | PhotoUploader-hemligheter |

Uppladdning sker via **PhotoUploader** (separat verktyg) med connection string i **lokala** användarsecrets — inte i det här webbprojektet.

## Projektstruktur

```
.github/workflows/   CI → GitHub Pages
Models/              PhotoItem, GalleryOptions, index-DTO:er
Services/            BlobGalleryPhotoService
Components/          Lightbox, PhotoGrid, Breadcrumbs
Pages/               Home, Year, Album
wwwroot/             appsettings.json, 404.html, .nojekyll
Styles/              Tailwind input
```

## Licens

Privat projekt för 40-årsfirande.
