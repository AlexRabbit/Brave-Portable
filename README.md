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

## 🔄 How updates work

Every time you launch **`BraveNightlyPortable-AlexRabbit.exe`**:

1. Wrapper checks GitHub for a newer Nightly build with a Windows zip
2. If found → splash → download → extract → launch
3. If up to date → Brave opens immediately

**Manual fallback (advanced):** run `Other\Source\update.bat`, then launch the AlexRabbit exe again.



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

<p align="center">
  <strong>🟢 Ready?</strong> Double-click <code>BraveNightlyPortable-AlexRabbit.exe</code> — by AlexRabbit.
</p>
