# GitHub Upload Checklist — AlexRabbit / Brave-Portable

Manual steps for AlexRabbit to publish this repo. **Do not include secrets, personal paths, or runtime data.**

---

## Pre-upload verification

- [ ] `BraveNightlyPortable-AlexRabbit.exe` at repo root
- [ ] `BraveNightlyPortable-Internal.exe` at repo root
- [ ] `App/Brave/` contains **only** `README.txt` (no ~230 MB binaries)
- [ ] No `tools/`, `Data/`, `wrapper/bin`, `wrapper/obj`, `wrapper/out`
- [ ] No `tmp-*`, `*.log`, `*.bak`, `Thumbs.db`
- [ ] README opens with **double-click `BraveNightlyPortable-AlexRabbit.exe`**
- [ ] No API keys, tokens, or local machine paths in any file
- [ ] All branding says **AlexRabbit** (not Cursor, not AI tools)

---

## Files that belong in the public source repo (25 files)

```
.gitattributes
.github/dependabot.yml
.github/workflows/release-brave-nightly.yml
.gitignore
App/AppInfo/                    (appinfo.ini, appicon.ico, installer.ini, Launcher/)
App/Brave/README.txt
App/DefaultData/whatgoeshere.txt
BraveNightlyPortable-AlexRabbit.exe
BraveNightlyPortable-Internal.exe
LICENSE
Other/Source/                   (bat + ps1 helpers, Readme.txt, LauncherLicense.txt)
README.md
build.ps1
docs/                           (PORTABLEAPPS-SUBMISSION.md, UPLOAD-CHECKLIST.md, Set-GitHub-Repo-Meta.ps1)
wrapper/                        (Program.cs, DefaultBrowser.cs, SplashForm.cs, .csproj)
```

---

## Upload option A — git push

```powershell
cd path\to\Brave-Portable
git status
git add -A
git commit -m "Public release by AlexRabbit"
git push origin main
```

---

## Upload option B — zip for manual GitHub upload

1. Select all folder contents **except** `.git`
2. Compress to `Brave-Portable-Source.zip`
3. Upload via GitHub web UI or create a Release and attach the zip

---

## After upload — set repo metadata (AlexRabbit)

```powershell
gh auth login
powershell -NoProfile -ExecutionPolicy Bypass -File docs\Set-GitHub-Repo-Meta.ps1
```

This sets the description and topics: `brave`, `brave-browser`, `nightly`, `portable`, `portableapps`, `windows`, `chromium`, `browser`, `alexrabbit`

---

## Create a user Release zip (optional)

After pushing to `main`:

1. Go to **Actions** → **Release Brave Nightly Portable** → **Run workflow**
2. CI downloads official Brave Nightly, builds launcher, publishes `BraveNightlyPortable_X.Y.Z_win64.zip`
3. Users extract and double-click `BraveNightlyPortable-AlexRabbit.exe` inside `BraveNightlyPortable\`

---

## What must NEVER be uploaded

| Path | Reason |
|------|--------|
| `Data/` | User profile (bookmarks, passwords) |
| `App/Brave/*.exe` and binaries | ~230 MB; downloaded on first run or bundled by CI Release only |
| `tools/` | Local .NET SDK from build.ps1 |
| `.cursor/`, `assets/` | Editor workspace |
| Any `.env`, credentials, or personal notes | Security |

---

<p align="center"><strong>Maintained by <a href="https://github.com/AlexRabbit">AlexRabbit</a></strong></p>
