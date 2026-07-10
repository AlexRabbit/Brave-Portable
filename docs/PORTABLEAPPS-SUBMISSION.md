# 📦 Building & Submitting a `.paf.exe` to PortableApps.com

This guide explains how to turn **Brave Nightly Portable** into a standard PortableApps.com installer (`.paf.exe`) and how to submit it to the community.

> **You cannot build `.paf.exe` from this repo alone** — you need the official **[PortableApps.com Installer Generator](https://portableapps.com/development)** (free, Windows-only GUI tool).

---

## ✅ Prerequisites

Before building the installer:

| # | Requirement | Notes |
|---|-------------|-------|
| 1 | **Working portable folder** | Run `START-BRAVE-NIGHTLY.bat` once so `App\Brave\brave.exe` exists |
| 2 | **PortableApps.com Launcher** | Already included as `BraveNightlyPortable-Internal.exe` |
| 3 | **PA Installer Generator** | Download from [portableapps.com/development](https://portableapps.com/development) |
| 4 | **App icon** | Optional but recommended — `App\AppInfo\appicon.ico` (128×128 or multi-size `.ico`) |

### Adding an icon (recommended for submission)

After first successful launch:

1. Copy Brave's icon, or export from `App\Brave\brave.exe`
2. Save as **`App\AppInfo\appicon.ico`**
3. Rebuild the `.paf.exe` (step below)

---

## 🔨 Step 1 — Prepare a release-ready folder

```powershell
cd path\to\Brave-Portable
START-BRAVE-NIGHTLY.bat
```

Wait until Brave opens (confirms `App\Brave\` is populated).

**Remove before packaging** (should already be gitignored):

- `Data\` — user profile (empty for distribution)
- `tools\` — local .NET SDK from `build.ps1`
- `wrapper\bin`, `wrapper/obj`, `wrapper/out`

Your folder should contain at minimum:

```
BraveNightlyPortable/
├── START-BRAVE-NIGHTLY.bat
├── BraveNightlyPortable-AlexRabbit.exe
├── BraveNightlyPortable-Internal.exe
├── App/  (with Brave binaries + AppInfo)
├── Other/
├── help.html
└── ...
```

> **Folder name for PA:** use `BraveNightlyPortable` as the directory name inside the installer (matches `AppId` in `appinfo.ini`).

---

## 🔨 Step 2 — Open PA Installer Generator

1. Download & run **PortableApps.com Installer Generator** from  
   [https://portableapps.com/development](https://portableapps.com/development)
2. Point it at your **`App\AppInfo\appinfo.ini`**
3. Verify detected settings:

| Field | Expected value |
|-------|----------------|
| AppId | `BraveNightlyPortable-AlexRabbit` |
| Start | `START-BRAVE-NIGHTLY.bat` |
| 64-bit | Yes |
| Admin required | No |

4. Click **Build** / **Create installer**
5. Output: **`BraveNightlyPortable_AlexRabbit_X.Y.Z.paf.exe`** (name varies)

Test the `.paf.exe` on a clean folder:

- Install to a test `PortableApps\` directory
- Run from PA Platform menu
- Confirm profile writes to `Data\profile\`

---

## 🔨 Step 3 — What the `.paf.exe` gives you

| Benefit | Description |
|---------|-------------|
| **One-click install** | Users run `.paf.exe` → app appears in PA Platform |
| **PA updater integration** | Platform can check your hosted `.paf.exe` for updates |
| **Standard format** | Same as Firefox Portable, LibreOffice Portable, etc. |
| **Directory submission** | Required format for official PA.com listing requests |

---

## 📤 Step 4 — Submit to PortableApps.com (DevTest)

Official directory listing for a **trademarked browser** is unlikely without Brave Software permission. The realistic path is the **Development Forum (DevTest)**.

### Where to post

**Forum:** [Portable App Development](https://portableapps.com/forums/portable_app_development)  
**Subforum:** DevTest Releases (follow current forum rules)

### Submission checklist

Copy/paste and fill in when creating your thread:

```markdown
**App name:** Brave Nightly Portable (unofficial)
**Version:** [from appinfo.ini DisplayVersion]
**Download:** [GitHub Release URL or direct .paf.exe link]
**Source code:** https://github.com/AlexRabbit/Brave-Portable
**License:** MIT (wrapper) + MPL 2.0 (Brave binary, separate download)
**Category:** Internet
**64-bit:** Yes

**Description:**
Unofficial portable wrapper for Brave Browser Nightly channel.
Downloads official binaries from github.com/brave/brave-browser releases.
PortableApps.com Format with PA Launcher. Profile stored in Data/profile/.

**Update mechanism:**
Wrapper checks GitHub for new Nightly zips on each launch.

**Trademark disclaimer:**
Not affiliated with Brave Software. Community-maintained wrapper only.
```

### Attach / link

- [ ] `.paf.exe` hosted on GitHub Releases (recommended)
- [ ] Source repo public (required for DevTest)
- [ ] Screenshot of app running from PA Platform
- [ ] SHA256 hash of `.paf.exe` (optional, builds trust)

### After posting

- Respond to tester feedback promptly
- Bump `PackageVersion` in `appinfo.ini` for each release
- Rebuild `.paf.exe` when Brave Nightly updates

---

## 📤 Step 5 — GitHub Releases (recommended hosting)

Host both artifacts on each release:

| File | Audience |
|------|----------|
| `BraveNightlyPortable_X.Y.Z_win64.zip` | GitHub users / manual portable |
| `BraveNightlyPortable_X.Y.Z.paf.exe` | PortableApps.com Platform users |

GitHub Actions in this repo builds the **zip** automatically. Build the **`.paf.exe` locally** with PA Installer Generator after CI runs, then attach to the same Release manually.

---

## ❓ FAQ

**Q: Can the `.paf.exe` download Brave instead of bundling it?**  
A: The PA installer packages what's in the folder at build time. For a smaller `.paf.exe`, build from a folder **without** `App\Brave\` and let first-run download handle it — but DevTest testers may prefer a ready-to-run package. The CI zip includes Brave pre-bundled.

**Q: Do I need to modify Brave?**  
A: **No.** Only launcher flags (`--user-data-dir`, `--disable-brave-update`, etc.) in `BraveNightlyPortable-Internal.ini`.

**Q: Can I submit to the official PA directory?**  
A: Possible in theory, but trademark + unofficial status make **DevTest** the practical route unless Brave Software cooperates.

---

<p align="center"><strong>Questions?</strong> Open an issue at <a href="https://github.com/AlexRabbit/Brave-Portable/issues">GitHub Issues</a>.</p>
