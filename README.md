# 🦁 Brave Nightly Portable

> **Unofficial** portable **[Brave Browser Nightly](https://brave.com/download-nightly/)** for Windows — built in **[PortableApps.com Format (PA.c)](https://portableapps.com/development/portableapps.com_format)**, auto-downloads from **official GitHub**, profile stays inside the folder.

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Channel](https://img.shields.io/badge/Channel-Nightly-orange.svg)](https://github.com/brave/brave-browser/releases)
[![Format](https://img.shields.io/badge/Format-PortableApps.com-0066cc.svg)](https://portableapps.com/)

---

## 🚀 Start here — before anything else

### 👉 **Double-click `START-BRAVE-NIGHTLY.bat`**

That is the **only** file you need to remember. Always start Brave this way.

```
📁 Brave-Portable/
   ├── 🟢 START-BRAVE-NIGHTLY.bat          ← ⭐ DOUBLE-CLICK THIS FIRST ⭐
   ├── 🔵 BraveNightlyPortable-AlexRabbit.exe   (launcher — called by the .bat)
   ├── 📁 App/          PA.c config + browser (auto-downloaded)
   ├── 📁 Data/         YOUR profile (created on first run)
   └── 📁 Other/        manual update scripts (optional)
```

| Step | What happens |
|------|----------------|
| **1** | `.bat` checks **.NET 8 Desktop Runtime** (launcher requirement) |
| **2** | If missing → opens the official download page for you |
| **3** | If OK → starts the wrapper exe |
| **4** | **First run:** small splash window while **~230 MB official Brave Nightly** downloads from GitHub (1–3 min) |
| **5** | Brave opens with a portable profile in `Data\profile\` |

> 💡 **No install wizard.** No admin rights. No registry pollution on the host PC (PA launcher cleans up after exit).

---

## 🎬 What you see when launching

| Situation | Splash window? | Message |
|-----------|----------------|---------|
| Normal launch (Brave already installed, up to date) | ❌ No | Brave opens immediately |
| First run / missing browser | ✅ Yes | *"Downloading official Brave Nightly from GitHub…"* |
| Update available | ✅ Yes | *"Downloading Brave Nightly vX.Y.Z…"* |
| Missing .NET 8 | ❌ (bat handles it) | Console + download page for Desktop Runtime |

The splash **only** appears when something is being **downloaded or set up** — never on a plain everyday launch.

---

## 💾 Moving to another computer — yes, everything travels with you

**This is the whole point of portable mode.**

Copy the **entire folder** (USB drive, cloud zip, external SSD — whatever you use). Your data lives here:

| Path | What it stores |
|------|----------------|
| `Data\profile\` | Bookmarks, history, passwords, extensions, Brave Rewards, sync settings, themes |
| `Data\cache\` | Disk cache (safe to delete — will rebuild) |
| `Data\settings\` | PortableApps launcher preferences |

On the new PC:

1. Paste the folder anywhere
2. Double-click **`START-BRAVE-NIGHTLY.bat`** again
3. Same Brave — same bookmarks, same logins ✅

> ⚠️ **Copy the whole folder**, not just `brave.exe`. If `App\Brave\` is empty on the new PC, the launcher re-downloads Brave automatically — your **profile in `Data\` is what matters**.

---

## 🧩 What each file does

| File | Size | Role | You click it? |
|------|------|------|---------------|
| **`START-BRAVE-NIGHTLY.bat`** | ~1 KB | Entry point — checks requirements, starts launcher | **✅ YES** |
| `BraveNightlyPortable-AlexRabbit.exe` | ~175 KB | Downloads/updates Brave, then starts PA engine | Called by `.bat` |
| `BraveNightlyPortable-Internal.exe` | ~140 KB | PortableApps.com launcher (registry, paths) | ❌ Hidden — do not click |
| `App\Brave\brave.exe` | ~230 MB | Official Brave Nightly binary | Auto-downloaded |
| `build.ps1` | tiny | Rebuild launcher (developers) | Optional |

### Where does Brave come from?

**Always official. Never modified.**

- Source: [github.com/brave/brave-browser/releases](https://github.com/brave/brave-browser/releases)
- Asset: `brave-v*-win32-x64.zip` from releases tagged **Nightly**
- Verification: `ProductName` must contain **"Brave Browser Nightly"**
- Stable/Beta builds are **rejected**

---

## 📥 Download & install (3 ways)

### 🥇 Option A — Clone this repo (developers / GitHub users)

```powershell
git clone https://github.com/AlexRabbit/Brave-Portable.git
cd Brave-Portable
START-BRAVE-NIGHTLY.bat
```

First launch downloads Brave into `App\Brave\`.

### 🥈 Option B — GitHub Release zip (easiest for most users)

1. Open [Releases](https://github.com/AlexRabbit/Brave-Portable/releases)
2. Download `BraveNightlyPortable_X.Y.Z_win64.zip`
3. Extract anywhere
4. Double-click **`START-BRAVE-NIGHTLY.bat`**

### 🥉 Option C — PortableApps Platform

1. Install [PortableApps.com Platform](https://portableapps.com/download)
2. Copy this folder into `PortableApps\BraveNightlyPortable\`
3. It appears in the PA menu — still launches via the same `.bat` / wrapper chain

---

## 🛠️ Troubleshooting

<details>
<summary><strong>❌ "App\Brave\brave.exe could not be found"</strong></summary>

Brave was not downloaded yet or download failed.

1. Check internet connection
2. Run manually: `Other\Source\update.bat`
3. Try again: `START-BRAVE-NIGHTLY.bat`

</details>

<details>
<summary><strong>❌ ".NET 8 Desktop Runtime required"</strong></summary>

Install **[.NET Desktop Runtime 8.x (x64)](https://dotnet.microsoft.com/download/dotnet/8.0)** — the **Desktop Runtime**, not just ASP.NET.

Windows 11 usually has it already. Windows 10 may need a one-time install.

</details>

<details>
<summary><strong>❌ GitHub API rate limit (403)</strong></summary>

The launcher automatically falls back to HTML parsing. If both fail, wait 10 minutes or run `Other\Source\update.bat`.

</details>

<details>
<summary><strong>🔄 Force manual update</strong></summary>

```
Other\Source\update.bat
```

</details>

<details>
<summary><strong>🧹 Reset profile (fresh start)</strong></summary>

Close Brave, delete `Data\profile\` and `Data\cache\`, launch again.

</details>

---

## 📁 Full folder layout

```
Brave-Portable/
├── START-BRAVE-NIGHTLY.bat              ⭐ START HERE
├── BraveNightlyPortable-AlexRabbit.exe  wrapper launcher
├── BraveNightlyPortable-Internal.exe    PA engine (hidden)
├── help.html                            quick help (PA standard)
├── LICENSE
├── README.md                            you are here
├── build.ps1                            rebuild wrapper (devs)
│
├── App/
│   ├── AppInfo/
│   │   ├── appinfo.ini                  PA metadata
│   │   ├── installer.ini                upgrade preserve rules
│   │   └── Launcher/
│   │       ├── BraveNightlyPortable-Internal.ini
│   │       └── Custom.nsh
│   ├── Brave/                           browser (auto-downloaded, gitignored)
│   └── DefaultData/
│
├── Data/                                YOUR DATA (gitignored, created at runtime)
│   ├── profile/                         Chromium user-data-dir
│   ├── cache/
│   └── settings/
│
├── Other/Source/
│   ├── update.bat
│   ├── Update-BraveNightly.ps1
│   └── Readme.txt
│
├── wrapper/                             C# source for AlexRabbit.exe
│   ├── Program.cs
│   ├── SplashForm.cs
│   └── BraveNightlyPortable-AlexRabbit.csproj
│
└── .github/workflows/
    └── release-brave-nightly.yml        auto-release every 6h
```

---

## 👨‍💻 For developers

### Rebuild the launcher

```powershell
.\build.ps1
```

Installs .NET 8 SDK locally to `tools\` (gitignored), publishes `BraveNightlyPortable-AlexRabbit.exe` to repo root.

### Requirements

- Windows 10/11 x64
- .NET 8 SDK (or let `build.ps1` install it)
- Internet (for Brave download)

### CI / auto-releases

GitHub Actions workflow `release-brave-nightly.yml`:

- Runs every **6 hours** + manual trigger
- Downloads latest official Nightly zip
- Builds launcher, publishes `BraveNightlyPortable_X.Y.Z_win64.zip`

---

## 📦 PortableApps.com — `.paf.exe` & submission

| Goal | Need `.paf.exe`? |
|------|------------------|
| Use on USB / personal portable collection | ❌ No — folder is enough |
| Install via PA Platform menu | ❌ No — drop folder in `PortableApps\` |
| **Official PA.com directory listing** | ✅ Yes |
| PA Platform built-in updater for your app | ✅ Yes |

**Full step-by-step:** see **[docs/PORTABLEAPPS-SUBMISSION.md](docs/PORTABLEAPPS-SUBMISSION.md)**

> ⚠️ **Trademark note:** Brave is trademarked by Brave Software. Official PA directory acceptance is **unlikely** without permission. Community **DevTest** forum submission is the realistic path.

---

## ⚖️ Legal & disclaimer

| Component | License / owner |
|-----------|-----------------|
| This wrapper (launcher, scripts, docs) | **MIT** — [AlexRabbit](https://github.com/AlexRabbit) |
| Brave Browser binary | **MPL 2.0** — [Brave Software](https://brave.com) |
| PortableApps.com Launcher engine | PA.c license — [PortableApps.com](https://portableapps.com) |

**Unofficial project.** Not affiliated with, endorsed by, or sponsored by Brave Software or PortableApps.com.

**Nightly channel** = bleeding edge — may crash. Use Stable Brave for daily critical work.

---

## 🗺️ Quick reference card

```
┌─────────────────────────────────────────────────────────┐
│  START     →  START-BRAVE-NIGHTLY.bat                     │
│  PROFILE   →  Data\profile\                               │
│  UPDATE    →  automatic on launch (or Other\Source\...)   │
│  MOVE PC   →  copy entire folder                          │
│  BRAVE SRC →  github.com/brave/brave-browser (Nightly)    │
│  THIS REPO →  github.com/AlexRabbit/Brave-Portable        │
└─────────────────────────────────────────────────────────┘
```

<p align="center"><strong>🟢 Double-click <code>START-BRAVE-NIGHTLY.bat</code> — that's it. 🚀</strong></p>
