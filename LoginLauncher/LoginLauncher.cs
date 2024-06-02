using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LoginLauncher;

/// <summary>
/// The LoginLauncher class provides methods to launch an application
/// on the Windows login screen.
/// </summary>
public class LoginLauncher {

	/// <summary>
	/// Get a duplicate of the current process token in order to
	/// impersonate the current user. The current user should be
	/// the local SYSTEM account in order to gain access to the
	/// Windows login screen.
	/// </summary>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException"></exception>
	public IntPtr GetDuplicatedToken() {
		bool processTokenCouldBeOpened = Win32.OpenProcessToken(
			Process.GetCurrentProcess().Handle,
			Win32.TOKEN_DUPLICATE,
			out IntPtr processToken
		);
		if (!processTokenCouldBeOpened) {
			throw new InvalidOperationException("Could not obtain current process token: " + Marshal.GetLastWin32Error());
		}

		bool processTokenCouldBeDuplicated = Win32.DuplicateTokenEx(
			processToken,
			Win32.TOKEN_ALL_ACCESS,
			IntPtr.Zero,
			Win32.SECURITY_IMPERSONATION_LEVEL.SecurityIdentification,
			Win32.TOKEN_TYPE.TokenPrimary,
			out IntPtr duplicatedToken
		);
		if (!processTokenCouldBeDuplicated) {
			throw new InvalidOperationException("Could not duplicate current process token: " + Marshal.GetLastWin32Error());
		}

		return duplicatedToken;
	}

	/// <summary>
	/// Get an impersonated session ID for the specified session token.
	/// </summary>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException"></exception>
	public uint GetImpersonatedSessionId(IntPtr sessionToken) {
		uint activeSessionId = Win32.WTSGetActiveConsoleSessionId();
		bool tokenInformationCouldBeSet = Win32.SetTokenInformation(
			sessionToken,
			Win32.TOKEN_INFORMATION_CLASS.TokenSessionId,
			ref activeSessionId,
			(uint)Marshal.SizeOf(activeSessionId)
		);
		if (!tokenInformationCouldBeSet) {
			throw new InvalidOperationException("Could not set token information " + Marshal.GetLastWin32Error());
		}

		return activeSessionId;
	}

	/// <summary>
	/// Force the screen to be locked.
	/// </summary>
	/// <param name="sessionId"></param>
	public void ForceLockScreen(uint sessionId) {
		this.ForceLockScreen(sessionId, TimeSpan.FromSeconds(10));
	}

	/// <summary>
	/// Force the screen to be locked.
	/// </summary>
	/// <param name="sessionId"></param>
	/// <param name="timeout"></param>
	/// <exception cref="InvalidOperationException"></exception>
	public void ForceLockScreen(uint sessionId, TimeSpan timeout) {
		if (Process.GetProcessesByName("LogonUI").Length == 0) {
			bool interactiveUserTokenCouldBeOpened = Win32.WTSQueryUserToken(
				sessionId,
				out IntPtr interactiveUserToken
			);
			if (interactiveUserTokenCouldBeOpened) {
				// Try to lock the screen using the interactive user token.
				Win32.STARTUPINFO siInteractive = new();
				siInteractive.cb = Marshal.SizeOf(siInteractive);
				siInteractive.lpDesktop = @"Winsta0\default";
				Win32.CreateProcessAsUser(
					interactiveUserToken,
					null,
					"rundll32.exe user32.dll,LockWorkStation",
					IntPtr.Zero,
					IntPtr.Zero,
					false,
					(uint)Win32.CreateProcessFlags.CREATE_NEW_CONSOLE | (uint)Win32.CreateProcessFlags.INHERIT_CALLER_PRIORITY,
					IntPtr.Zero,
					"C:\\Windows\\System32\\",
					ref siInteractive,
					out Win32.PROCESS_INFORMATION piInteractive
				);
			} else {
				// Or lock the screen using the LockWorkStation function.
				Win32.LockWorkStation();
			}
		}

		// Wait until the screen is locked.
		using (CancellationTokenSource cts = new()) {
			cts.CancelAfter(timeout); // Set your desired timeout here
			while (!cts.IsCancellationRequested) {
				if (Process.GetProcessesByName("LogonUI").Length > 0) {
					break;
				}

				try {
					Task.Delay(100, cts.Token);
				} catch (TaskCanceledException) {
					throw new InvalidOperationException("Timeout waiting for screen to be locked.");
				}
			}
		}
	}

	/// <summary>
	/// Start the specified application on the Windows login screen using the session token
	/// of an impersonated user.
	/// </summary>
	/// <param name="sessionToken"></param>
	/// <param name="application"></param>
	/// <param name="startInDirectory"></param>
	/// <param name="args"></param>
	/// <exception cref="InvalidOperationException"></exception>
	public void StartApplication(IntPtr sessionToken, string application, string startInDirectory = "", string args = "") {
		Win32.STARTUPINFO siApplication = new();

		siApplication.cb = Marshal.SizeOf(siApplication);
		siApplication.lpDesktop = @"Winsta0\Winlogon";

		string commandline = "\"" + application + "\" " + args;

		bool processCouldBeStarted = Win32.CreateProcessAsUser(
			sessionToken,
			null,
			commandline,
			IntPtr.Zero,
			IntPtr.Zero,
			true,
			(uint)Win32.CreateProcessFlags.CREATE_NEW_CONSOLE | (uint)Win32.CreateProcessFlags.INHERIT_CALLER_PRIORITY,
			IntPtr.Zero,
			startInDirectory,
			ref siApplication,
			out Win32.PROCESS_INFORMATION piApplication
		);
		if (!processCouldBeStarted) {
			throw new InvalidOperationException("Process could not be started: " + Marshal.GetLastWin32Error());
		}
	}

	/// <summary>
	/// Launch the specified application on the Windows login screen.
	/// </summary>
	/// <param name="application"></param>
	/// <param name="startInDirectory"></param>
	/// <param name="args"></param>
	public static void LaunchApplication(string application, string startInDirectory = "", string args = "") {
		LoginLauncher launcher = new();
		IntPtr token = launcher.GetDuplicatedToken();
		uint sessionId = launcher.GetImpersonatedSessionId(token);
		launcher.ForceLockScreen(sessionId);
		launcher.StartApplication(token, application, startInDirectory, args);
	}

}