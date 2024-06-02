# Login Launcher

Simple app that launches applications on the Windows login screen.

## Usage
Start the app with the path to the application you want to launch and any arguments you want to pass to it. The arguments will be passed along by the launcher to your application.

```powershell
.\LoginLauncher.exe "C:\path\to\app.exe" "arg1" "arg2" "arg3"
```

The launcher has to be run with the Windows internal SYSTEM account to be able to launch applications on the logon screen. In order to start the launcher as the SYSTEM account, you may start it as a service or by using a scheduled task.

As Windows will put the lock screen on top of the login screen, the application will not be visible. Therefore, it is recommended to disable the Windows lock screen on top of the login screen with the "Do not display the lock screen" group policy setting. Or create a new registry key with the following content:

```registry
Windows Registry Editor Version 5.00

[HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization]
"NoLockScreen"=dword:00000001
```

## Example

```powershell
.\LoginLauncher.exe "C:\Windows\System32\cmd.exe"
```

<kbd> <img src="" /> </kbd>






