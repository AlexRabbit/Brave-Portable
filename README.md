# 🦁 Brave Nightly Portable

> Unofficial portable **[Brave Browser Nightly](https://brave.com/download-nightly/)** for Windows — [PortableApps.com Format](https://portableapps.com/development/portableapps.com_format), auto-updates from **official GitHub**.

---

## ⚡ How to open (one file only)

### 👉 Double-click **`BraveNightlyPortable-AlexRabbit.exe`**

That is the **only** file you need. No `.bat`. No install wizard.

```
📁 Brave-Portable/
   ├── 🔵 BraveNightlyPortable-AlexRabbit.exe   ← DOUBLE-CLICK THIS
   ├── 📁 App/
   ├── 📁 Data/          (your profile — created on first run)
   └── 📁 Other/
```

**First launch:** downloads ~230 MB **official Brave Nightly** from GitHub (1–3 min).  
**Every launch after:** checks for updates, then opens Brave.

> **Do NOT click** `App\AppInfo\Launcher\BraveNightlyPortable-Internal.exe` — that is the inner PA engine, not for users.

---

## 🧩 What the 64 MB vs 230 MB means

| File | Size | What it is |
|------|------|------------|
| **`BraveNightlyPortable-AlexRabbit.exe`** | **~175 KB** | Our launcher (update + start). Needs [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0) — usually on Win11 already |
| **`App\Brave\brave.exe`** | **~230 MB** | Official Brave Nightly — **downloaded automatically** from GitHub, not stored in git |

The old 64 MB exe was our launcher bundled with .NET — **replaced** with a tiny ~175 KB launcher.  
**Brave itself** always comes from official GitHub — never modified, never repackaged.

---

## 🎬 What happens on launch

1. Checks [brave/brave-browser releases](https://github.com/brave/brave-browser/releases) for **Nightly**
2. Downloads `brave-v*-win32-x64.zip` if missing or outdated
3. Verifies **ProductName = Brave Browser Nightly**
4. Launches via PA launcher with portable profile in `Data\profile\`

---

## ✅ Is this really Nightly?

**Yes.** Only GitHub releases tagged **Nightly** are used. Stable/Beta are rejected.

---

## 📦 PortableApps.com directory — can you submit?

| Question | Answer |
|----------|--------|
| **Works with PA Platform?** | ✅ Yes — drop this folder in `PortableApps\` |
| **Needs `.paf` file?** | For **official directory listing**: yes — build with [PA Installer Generator](https://portableapps.com/development) |
| **Will Brave be accepted officially?** | ⚠️ **Unlikely** — Brave is trademarked; no official PA Brave app exists |
| **DevTest / community?** | ✅ Post in [PortableApps Development Forum](https://portableapps.com/forums/portable_app_development) with source visible |

**For personal / GitHub use:** the folder format is enough. **`.paf.exe`** is only needed for PA.com’s installer/updater ecosystem.

---

## 📁 Folder layout (repo = portable app)

The **git repo root IS the portable app** — no extra wrapper folder.

```
Brave-Portable/
├── BraveNightlyPortable-AlexRabbit.exe    ← you click this
├── App/
│   ├── AppInfo/          PA.c config (appinfo.ini, launcher ini)
│   ├── Brave/            browser (auto-downloaded)
│   └── DefaultData/
├── Data/                 YOUR profile (gitignored)
├── Other/Source/         update.bat (manual)
├── wrapper/              C# source (developers)
├── build.ps1             rebuild launcher
└── .github/workflows/    auto-release zips
```

---

## 🛠️ Manual update

```
Other\Source\update.bat
```

---

## 👨‍💻 Developers — rebuild launcher

```powershell
.\build.ps1
```

Requires .NET 8 SDK (script can install to `tools\`).

---

## 📦 GitHub Releases

CI publishes **`BraveNightlyPortable_X.Y.Z_win64.zip`** with Brave + launcher pre-bundled.

---

## ⚠️ Disclaimer

Unofficial. Not affiliated with Brave Software or PortableApps.com. Nightly = bleeding edge. MPL 2.0 (Brave) / MIT (wrapper).

---

<p align="center"><strong>Double-click <code>BraveNightlyPortable-AlexRabbit.exe</code> 🚀</strong></p>
