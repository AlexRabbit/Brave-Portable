# 🦁 Brave Nightly Portable

> **Unofficial** portable **[Brave Browser Nightly](https://brave.com/download-nightly/)** for Windows — built by **[AlexRabbit](https://github.com/AlexRabbit)** in [PortableApps.com Format](https://portableapps.com/development/portableapps.com_format). Auto-downloads from **official GitHub**. Your profile stays inside the folder.

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Maintainer](https://img.shields.io/badge/Maintainer-AlexRabbit-ff6b35.svg)](https://github.com/AlexRabbit)
[![Channel](https://img.shields.io/badge/Channel-Nightly-orange.svg)](https://github.com/brave/brave-browser/releases)
[![Format](https://img.shields.io/badge/Format-PortableApps.com-0066cc.svg)](https://portableapps.com/)
[![Windows](https://img.shields.io/badge/Windows-10%2F11%20x64-0078D4.svg)](https://www.microsoft.com/windows)

---

## ⚡ Start here — double-click this first

```
BraveNightlyPortable-AlexRabbit.exe
```

**That is the entire app.** Nothing else to install. Nothing else to configure for a first launch.

| Step | What you do | What happens |
|:----:|-------------|--------------|
| **1** | **Double-click `BraveNightlyPortable-AlexRabbit.exe`** | AlexRabbit launcher checks for updates |
| **2** | Wait on first run only (~1–5 min) | Splash downloads **~230 MB** official Brave Nightly from GitHub |
| **3** | Done | Brave opens — bookmarks, extensions, and settings live in `Data\profile\` |

> 🧠 **Golden rule:** Always open **`BraveNightlyPortable-AlexRabbit.exe`**. Never open `App\Brave\brave.exe` directly — that skips the portable profile and update logic.

---

## 🎬 What you see when launching

| Situation | Splash? | What you see |
|-----------|:-------:|--------------|
| Normal day — Brave installed, up to date | ❌ | Brave opens in seconds |
| **First run** — no browser files yet | ✅ | *"Downloading official Brave Nightly from GitHub…"* |
| **Update available** — newer Nightly on GitHub | ✅ | *"Downloading Brave Nightly vX.Y.Z…"* |
| Missing **.NET 8 Desktop Runtime** | ❌ | Windows runtime error — install .NET 8 (see Requirements) |

The splash **only** appears when something is being downloaded or set up. Everyday launches are instant.

---

## 📥 How to get this project

### Option A — GitHub Release zip (recommended for most users)

1. Open **[Releases](https://github.com/AlexRabbit/Brave-Portable/releases)** by AlexRabbit
2. Download `BraveNightlyPortable_X.Y.Z_win64.zip`
3. Extract anywhere — you get a folder named **`BraveNightlyPortable\`**
4. **Double-click `BraveNightlyPortable-AlexRabbit.exe`** inside that folder

> Release zips built by CI include official Brave Nightly binaries — **no download wait on first launch**.

### Option B — Clone the source repository

```powershell
git clone https://github.com/AlexRabbit/Brave-Portable.git
cd Brave-Portable
```

Then **double-click `BraveNightlyPortable-AlexRabbit.exe`**.  
First run downloads Brave (~230 MB) because the source repo ships without browser binaries.

### Option C — PortableApps.com Platform

1. Install the [PortableApps.com Platform](https://portableapps.com/download)
2. Copy this folder to `PortableApps\BraveNightlyPortable\`
3. Launch **`BraveNightlyPortable-AlexRabbit.exe`** from the PA menu

---

## ✅ Requirements

| Requirement | Who needs it | Details |
|-------------|--------------|---------|
| **Windows 10 or 11 (64-bit)** | Everyone | 32-bit Windows is not supported |
| **Internet** | First run + updates | Downloads from official GitHub only |
| **[.NET 8 Desktop Runtime (x64)](https://dotnet.microsoft.com/download/dotnet/8.0)** | Launcher only | ~50 MB one-time; Brave itself does **not** need .NET |
| **~250 MB free disk** | First run | For Brave Nightly browser files |

> 💡 Windows 11 usually has .NET 8 already. On Windows 10, install the **Desktop Runtime** — not SDK, not ASP.NET.

---

## 🗂️ Folder map

```
Brave-Portable/
│
├── BraveNightlyPortable-AlexRabbit.exe   ← 🟢 START HERE (AlexRabbit launcher)
├── BraveNightlyPortable-Internal.exe     ← PortableApps engine (do not run directly)
│
├── App/
│   ├── Brave/         ← Official Brave Nightly (empty in source repo until first run)
│   └── AppInfo/       ← PortableApps metadata, icons, launcher config
│
├── Data/              ← Created on first run — YOUR profile
│   ├── profile/       ← Bookmarks, passwords, extensions, settings
│   └── cache/         ← Browser cache
│
├── Other/Source/
│   ├── Set-As-Default-Browser.bat   ← Register wrapper as Windows default browser
│   ├── update.bat                   ← Manual update (advanced fallback)
│   └── Update-BraveNightly.ps1      ← Same update logic in PowerShell
│
├── wrapper/           ← C# source (developers rebuild launcher here)
├── build.ps1          ← Rebuild BraveNightlyPortable-AlexRabbit.exe
├── docs/              ← PortableApps submission guide + upload checklist
└── README.md          ← You are here
```

---

## 🌐 Set as your default browser

Windows tries to register **`App\Brave\brave.exe`** when you click “Set as default” **inside Brave Settings**. That opens a **second Brave** with a different profile — not what you want.

**Use the AlexRabbit wrapper instead:**

```
BraveNightlyPortable-AlexRabbit.exe --register-default
```

Or double-click `Other\Source\Set-As-Default-Browser.bat`, then in **Windows Settings → Apps → Default apps** choose **Brave Nightly Portable (AlexRabbit)** for Web browser, HTTP, and HTTPS.

| Do this ✅ | Don't do this ❌ |
|-----------|------------------|
| `--register-default` + Windows Settings | “Set as default” inside Brave Settings |
| Always launch **`BraveNightlyPortable-AlexRabbit.exe`** | Open `App\Brave\brave.exe` directly |

Remove registration later: `BraveNightlyPortable-AlexRabbit.exe --unregister-default`

---

## 🔒 Where Brave comes from (always official)

| | |
|---|---|
| **Source** | [github.com/brave/brave-browser/releases](https://github.com/brave/brave-browser/releases) |
| **Channel** | **Nightly** only |
| **Asset** | `brave-v*-win32-x64.zip` |
| **Verification** | `ProductName` must contain **"Brave Browser Nightly"** |
| **Rejected** | Stable, Beta, Dev builds |

AlexRabbit's wrapper **does not modify** Brave binaries. It downloads, verifies, and runs them in portable mode.

---

## 💾 Moving to another computer

1. **Close Brave** completely (check system tray)
2. Copy the **entire folder** — USB, cloud zip, external SSD
3. On the new PC, **double-click `BraveNightlyPortable-AlexRabbit.exe`**

> ⚠️ Copy the **whole folder**, not just `brave.exe`. Your **profile in `Data\` is what matters**. If `App\Brave\` is empty on the new PC, the launcher re-downloads Brave automatically.

---

## 🔄 How updates work

Every time you launch **`BraveNightlyPortable-AlexRabbit.exe`**:

1. Wrapper checks GitHub for a newer Nightly build with a Windows zip
2. If found → splash → download → extract → launch
3. If up to date → Brave opens immediately

**Manual fallback (advanced):** run `Other\Source\update.bat`, then launch the AlexRabbit exe again.

---

## 🛠️ Troubleshooting

<details>
<summary><strong>❌ "Could not download Brave Nightly from official GitHub"</strong></summary>

**Cause:** No internet, GitHub rate limit, or newest Nightly tag has no Windows zip yet.

1. Check internet connection
2. Run `Other\Source\update.bat` and wait for completion
3. Wait 10 minutes if rate-limited, then retry
4. **Double-click `BraveNightlyPortable-AlexRabbit.exe`** again

</details>

<details>
<summary><strong>❌ App won't start / ".NET Desktop Runtime" error</strong></summary>

Install **[.NET Desktop Runtime 8.x (x64)](https://dotnet.microsoft.com/download/dotnet/8.0)** — Desktop Runtime only, not SDK or ASP.NET. Then launch the AlexRabbit exe again.

</details>

<details>
<summary><strong>❌ "App\Brave\brave.exe could not be found"</strong></summary>

Brave was not downloaded yet or download failed.

1. Run `Other\Source\update.bat`
2. **Double-click `BraveNightlyPortable-AlexRabbit.exe`**

</details>

<details>
<summary><strong>❌ GitHub API rate limit (403)</strong></summary>

Launcher falls back to HTML parsing automatically. If both fail, wait 10 minutes or run `Other\Source\update.bat`.

</details>

<details>
<summary><strong>❌ Second Brave window / wrong profile</strong></summary>

You likely set default via Brave Settings instead of the AlexRabbit wrapper. Run `--register-default` and set default in Windows Settings to **Brave Nightly Portable (AlexRabbit)**.

</details>

<details>
<summary><strong>🐌 Download slow or stuck</strong></summary>

- First download is **~230 MB** — allow 1–5 minutes
- Corporate firewalls may block GitHub — try another network
- Run `Other\Source\update.bat` for console progress output

</details>

<details>
<summary><strong>🧹 Reset profile (fresh start)</strong></summary>

1. Close Brave completely
2. Delete `Data\profile\` and `Data\cache\`
3. **Double-click `BraveNightlyPortable-AlexRabbit.exe`**

</details>

<details>
<summary><strong>🔐 SmartScreen / "Windows protected your PC"</strong></summary>

Unsigned community wrapper by AlexRabbit. Click **More info → Run anyway**, or rebuild with `build.ps1` from source if you prefer.

</details>

---

## 👨‍💻 For developers

### Rebuild the launcher

```powershell
.\build.ps1
```

Installs .NET 8 SDK locally to `tools\` (gitignored), publishes `BraveNightlyPortable-AlexRabbit.exe` to repo root.

### Architecture

```
BraveNightlyPortable-AlexRabbit.exe   ← AlexRabbit C# wrapper: download, verify, default-browser registration
        │
        ▼
BraveNightlyPortable-Internal.exe     ← PortableApps engine: portable paths, profile in Data\
        │
        ▼
App\Brave\brave.exe                   ← Official Brave Nightly binary (unchanged)
```

### CI / auto-releases

[`.github/workflows/release-brave-nightly.yml`](.github/workflows/release-brave-nightly.yml) runs every **6 hours** + manual trigger. Produces `BraveNightlyPortable_X.Y.Z_win64.zip` with a ready `BraveNightlyPortable\` folder.

---

## 📦 PortableApps.com — `.paf.exe` & submission

| Goal | Need `.paf.exe`? |
|------|:----------------:|
| USB / personal portable use | ❌ |
| PA Platform menu | ❌ |
| Official PA.com directory | ✅ |

**Full guide:** [docs/PORTABLEAPPS-SUBMISSION.md](docs/PORTABLEAPPS-SUBMISSION.md)

---

## 📋 Runtime folders (not in source repo)

| Path | Purpose |
|------|---------|
| `App\Brave\` | Official Brave Nightly binaries (~230 MB) |
| `Data\profile\` | Your bookmarks, passwords, extensions |
| `Data\cache\` | Browser cache |
| `tools\` | Local .NET SDK (only after running `build.ps1`) |

---

## ⚖️ Legal & disclaimer

| Component | License / owner |
|-----------|-----------------|
| Wrapper (launcher, scripts, docs) | **MIT** — [AlexRabbit](https://github.com/AlexRabbit) |
| Brave Browser binary | **MPL 2.0** — [Brave Software](https://brave.com) |
| PortableApps.com Launcher engine | PA.c license — [PortableApps.com](https://portableapps.com) |

**Unofficial project by AlexRabbit.** Not affiliated with, endorsed by, or sponsored by Brave Software or PortableApps.com.

**Nightly channel** = bleeding edge — may crash. Use [Stable Brave](https://brave.com/download/) for daily critical work.

---

## ⭐ Support this project

Star the repo — it helps others find a clean portable Brave Nightly setup.

**Questions or bugs?** [Open an issue](https://github.com/AlexRabbit/Brave-Portable/issues)

**Publishing this repo?** See [docs/UPLOAD-CHECKLIST.md](docs/UPLOAD-CHECKLIST.md)

---

<p align="center">
  <strong>🟢 Ready?</strong> Double-click <code>BraveNightlyPortable-AlexRabbit.exe</code> — by AlexRabbit.
</p>
