# 🦁 Brave Nightly Portable

> **Unofficial** portable **[Brave Browser Nightly](https://brave.com/download-nightly/)** for Windows — [PortableApps.com Format](https://portableapps.com/development/portableapps.com_format), auto-downloads from **official GitHub**, profile stays inside the folder.

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Channel](https://img.shields.io/badge/Channel-Nightly-orange.svg)](https://github.com/brave/brave-browser/releases)
[![Format](https://img.shields.io/badge/Format-PortableApps.com-0066cc.svg)](https://portableapps.com/)
[![Windows](https://img.shields.io/badge/Windows-10%2F11%20x64-0078D4.svg)](https://www.microsoft.com/windows)

---

## ⚡ Start here — double-click this

```
BraveNightlyPortable-AlexRabbit.exe
```

**That is the entire app.**  
Download → extract anywhere → **double-click `BraveNightlyPortable-AlexRabbit.exe`**.  
No installer. No admin for normal use.  
(Optional: register as default browser — see below.)

| Step | What you do | What happens |
|:----:|-------------|--------------|
| **1** | Double-click **`BraveNightlyPortable-AlexRabbit.exe`** | Launcher checks for updates and opens Brave |
| **2** | Wait (first run only) | Splash window downloads **~230 MB** official Brave Nightly from GitHub |
| **3** | Done | Brave opens — your profile lives in `Data\profile\` |

> 🧠 **Tip:** Pin `BraveNightlyPortable-AlexRabbit.exe` to your taskbar or desktop. Always launch through this file — it handles download, update, and portable paths automatically.

---

## 🌐 Set as your default browser

Windows keeps trying to use **`App\Brave\brave.exe`** when you click “Set as default” **inside Brave**. That opens a **second Brave** with a different profile.

**Use the AlexRabbit wrapper instead** — one command:

```
BraveNightlyPortable-AlexRabbit.exe --register-default
```

Or double-click:

```
Other\Source\Set-As-Default-Browser.bat
```

Then in **Windows Settings → Apps → Default apps**:

1. Find **Brave Nightly Portable (AlexRabbit)**
2. Set it as default for **Web browser**, **HTTP**, and **HTTPS**

| Do this ✅ | Don't do this ❌ |
|-----------|------------------|
| Set default via `--register-default` + Windows Settings | Click “Set as default” inside Brave Settings |
| Always open **`BraveNightlyPortable-AlexRabbit.exe`** | Open `App\Brave\brave.exe` directly |

To remove the registration later:

```
BraveNightlyPortable-AlexRabbit.exe --unregister-default
```

---

## 🎬 What you see when launching

| Situation | Splash window? | What you see |
|-----------|:--------------:|--------------|
| Everyday launch (Brave installed, up to date) | ❌ No | Brave opens in a few seconds |
| **First run** / missing browser | ✅ Yes | *"Downloading official Brave Nightly from GitHub…"* |
| **Update available** | ✅ Yes | *"Downloading Brave Nightly vX.Y.Z…"* |
| Missing **.NET 8 Desktop Runtime** | ❌ | Windows shows a runtime error — install .NET 8 (see below) |

The splash **only** appears when something is being **downloaded or set up** — never on a plain everyday launch.

---

## 📥 How to get this project

### Option A — GitHub Release zip (easiest)

1. Open **[Releases](https://github.com/AlexRabbit/Brave-Portable/releases)**
2. Download `BraveNightlyPortable_X.Y.Z_win64.zip`
3. Extract — you get a ready folder named **`BraveNightlyPortable\`**
4. Inside that folder, **double-click `BraveNightlyPortable-AlexRabbit.exe`**

> The Release zip already includes official Brave Nightly binaries when built by CI — first launch is instant.

### Option B — Clone the repository

```powershell
git clone https://github.com/AlexRabbit/Brave-Portable.git
cd Brave-Portable
```

Then **double-click `BraveNightlyPortable-AlexRabbit.exe`**.

### Option C — PortableApps.com Platform

1. Install the [PortableApps.com Platform](https://portableapps.com/download)
2. Copy this folder to `PortableApps\BraveNightlyPortable\`
3. Launch **`BraveNightlyPortable-AlexRabbit.exe`** from the PA menu or folder

---

## ✅ Requirements

| Requirement | Who needs it | Details |
|-------------|--------------|---------|
| **Windows 10 or 11 (64-bit)** | Everyone | 32-bit Windows is not supported |
| **Internet** | First run + updates | Downloads official Brave from GitHub |
| **[.NET 8 Desktop Runtime (x64)](https://dotnet.microsoft.com/download/dotnet/8.0)** | Launcher only | ~50 MB one-time install; Brave itself does **not** need .NET |
| **~250 MB free disk space** | First run | For the Brave Nightly browser files |

> 💡 Windows 11 usually already has .NET 8. Windows 10 may need a one-time install of the **Desktop Runtime** (not ASP.NET, not SDK).

---

## 🗂️ Folder map — what everything is

```
Brave-Portable/
│
├── BraveNightlyPortable-AlexRabbit.exe   ← 🟢 YOU START HERE (always)
├── BraveNightlyPortable-Internal.exe     ← PortableApps launcher engine (do not run directly)
│
├── App/
│   ├── Brave/                       ← Official Brave Nightly (empty until first run)
│   └── AppInfo/                     ← PortableApps metadata + icons
│
├── Data/                            ← Created on first run — YOUR profile lives here
│   ├── profile/                     ← Bookmarks, passwords, extensions, settings
│   └── cache/                       ← Browser cache
│
├── Other/Source/
│   ├── Set-As-Default-Browser.bat   ← Register as Windows default browser
│   ├── update.bat                   ← Manual update fallback (advanced)
│   └── Update-BraveNightly.ps1      ← Same logic, PowerShell
│
├── wrapper/                         ← C# source (developers rebuild launcher here)
├── build.ps1                        ← Rebuild the launcher exe
├── docs/                            ← Advanced guides (PortableApps submission)
└── README.md                        ← You are here
```

---

## 🔒 Where Brave comes from (always official)

| | |
|---|---|
| **Source** | [github.com/brave/brave-browser/releases](https://github.com/brave/brave-browser/releases) |
| **Channel** | **Nightly** only |
| **Asset** | `brave-v*-win32-x64.zip` |
| **Verification** | `ProductName` must contain **"Brave Browser Nightly"** |
| **Rejected** | Stable, Beta, Dev builds |

This project **does not modify** Brave binaries. It downloads, verifies, and runs them in portable mode.

---

## 💾 Moving to another computer

**This is the whole point of portable mode.**

1. **Close Brave** completely (check system tray)
2. Copy the **entire folder** — USB, cloud zip, external SSD, whatever you use
3. On the new PC, **double-click `BraveNightlyPortable-AlexRabbit.exe`**

> ⚠️ Copy the **whole folder**, not just `brave.exe`.  
> If `App\Brave\` is empty on the new PC, the launcher re-downloads Brave automatically — your **profile in `Data\` is what matters**.

---

## 🔄 How updates work

Updates are **automatic** on every launch:

1. You double-click **`BraveNightlyPortable-AlexRabbit.exe`**
2. The wrapper checks GitHub for a newer Nightly build with a Windows zip
3. If found → splash appears → download → extract → launch
4. If up to date → Brave opens immediately (no splash)

### Force a manual update (advanced)

If automatic update failed, run:

```
Other\Source\update.bat
```

This opens a console with download progress. When finished, launch **`BraveNightlyPortable-AlexRabbit.exe`** again.

---

## 🛠️ Troubleshooting

<details>
<summary><strong>❌ "Could not download Brave Nightly from official GitHub"</strong></summary>

**Cause:** No internet, GitHub rate limit, or the newest Nightly tag has no Windows zip yet.

**Fix:**

1. Check your internet connection
2. Run `Other\Source\update.bat` and wait for it to finish
3. Wait 10 minutes if GitHub rate-limited you, then try again
4. **Double-click `BraveNightlyPortable-AlexRabbit.exe`** again

</details>

<details>
<summary><strong>❌ App won't start / ".NET Desktop Runtime" error</strong></summary>

The launcher requires **[.NET Desktop Runtime 8.x (x64)](https://dotnet.microsoft.com/download/dotnet/8.0)**.

Install the **Desktop Runtime** (not SDK, not ASP.NET), then **double-click `BraveNightlyPortable-AlexRabbit.exe`** again.

</details>

<details>
<summary><strong>❌ "App\Brave\brave.exe could not be found"</strong></summary>

Brave was not downloaded yet or the download failed.

1. Run `Other\Source\update.bat`
2. **Double-click `BraveNightlyPortable-AlexRabbit.exe`**

</details>

<details>
<summary><strong>❌ GitHub API rate limit (403)</strong></summary>

The launcher automatically falls back to HTML parsing. If both fail, wait 10 minutes or run `Other\Source\update.bat`.

</details>

<details>
<summary><strong>🐌 Download is slow or stuck</strong></summary>

- First download is **~230 MB** — allow 1–5 minutes on normal broadband
- Corporate firewalls may block GitHub — try another network or hotspot
- Run `Other\Source\update.bat` to see detailed progress in the console

</details>

<details>
<summary><strong>🧹 Reset profile (fresh start)</strong></summary>

1. Close Brave completely
2. Delete `Data\profile\` and `Data\cache\`
3. **Double-click `BraveNightlyPortable-AlexRabbit.exe`**

</details>

<details>
<summary><strong>🔐 SmartScreen / "Windows protected your PC"</strong></summary>

This is an **unsigned community portable wrapper**. Click **More info → Run anyway**, or build the launcher yourself with `build.ps1` if you prefer.

</details>

---

## 👨‍💻 For developers

### Rebuild the launcher

```powershell
.\build.ps1
```

Installs .NET 8 SDK locally to `tools\` (gitignored), publishes `BraveNightlyPortable-AlexRabbit.exe` to the repo root.

### Requirements

- Windows 10/11 x64
- .NET 8 SDK (or let `build.ps1` install it)
- Internet (for Brave download testing)

### CI / auto-releases

GitHub Actions workflow `.github/workflows/release-brave-nightly.yml`:

- Runs every **6 hours** + manual trigger
- Downloads latest official Nightly zip
- Builds launcher, publishes `BraveNightlyPortable_X.Y.Z_win64.zip`

### Architecture overview

```
BraveNightlyPortable-AlexRabbit.exe   ← C# wrapper: GitHub API/HTML → download → verify Nightly
        │
        ▼
BraveNightlyPortable-Internal.exe     ← PortableApps engine: portable paths, profile in Data\
        │
        ▼
App\Brave\brave.exe                   ← Official Brave Nightly binary
```

---

## 📦 PortableApps.com — `.paf.exe` & submission

| Goal | Need `.paf.exe`? |
|------|:----------------:|
| Use on USB / personal portable collection | ❌ No — folder is enough |
| Install via PA Platform menu | ❌ No — drop folder in `PortableApps\` |
| **Official PA.com directory listing** | ✅ Yes |
| PA Platform built-in updater for your app | ✅ Yes |

**Full step-by-step:** see **[docs/PORTABLEAPPS-SUBMISSION.md](docs/PORTABLEAPPS-SUBMISSION.md)**

> ⚠️ **Trademark note:** Brave is trademarked by Brave Software. Official PA directory acceptance is **unlikely** without permission. Community **DevTest** forum submission is the realistic path.

---

## 📋 What gets created at runtime (not in the repo)

These folders are **gitignored** and created on your machine — **do not delete them** unless you want a fresh start:

| Path | Purpose |
|------|---------|
| `App\Brave\` | Official Brave Nightly binaries (~230 MB) |
| `Data\profile\` | Your bookmarks, passwords, extensions |
| `Data\cache\` | Browser cache |
| `tools\` | Local .NET SDK (only if you ran `build.ps1`) |

---

## ⚖️ Legal & disclaimer

| Component | License / owner |
|-----------|-----------------|
| This wrapper (launcher, scripts, docs) | **MIT** — [AlexRabbit](https://github.com/AlexRabbit) |
| Brave Browser binary | **MPL 2.0** — [Brave Software](https://brave.com) |
| PortableApps.com Launcher engine | PA.c license — [PortableApps.com](https://portableapps.com) |

**Unofficial project.** Not affiliated with, endorsed by, or sponsored by Brave Software or PortableApps.com.

**Nightly channel** = bleeding edge — may crash. Use [Stable Brave](https://brave.com/download/) for daily critical work.

---

## ⭐ If this helped you

Consider starring the repo — it helps others find a clean portable Brave Nightly setup.

**Questions or bugs?** [Open an issue](https://github.com/AlexRabbit/Brave-Portable/issues)

---

<p align="center">
  <strong>🟢 Ready?</strong> Double-click <code>BraveNightlyPortable-AlexRabbit.exe</code> and go.
</p>
