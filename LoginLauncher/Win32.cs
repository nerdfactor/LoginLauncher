using System.Runtime.InteropServices;

namespace LoginLauncher;

/// <summary>
/// Access to Windows API functions.
/// </summary>
public class Win32 {

	[Flags]
	public enum ACCESS_MASK : uint {

		DELETE = 0x00010000,
		READ_CONTROL = 0x00020000,
		WRITE_DAC = 0x00040000,
		WRITE_OWNER = 0x00080000,
		SYNCHRONIZE = 0x00100000,

		STANDARD_RIGHTS_REQUIRED = 0x000f0000,

		STANDARD_RIGHTS_READ = 0x00020000,
		STANDARD_RIGHTS_WRITE = 0x00020000,
		STANDARD_RIGHTS_EXECUTE = 0x00020000,

		STANDARD_RIGHTS_ALL = 0x001f0000,

		SPECIFIC_RIGHTS_ALL = 0x0000ffff,

		ACCESS_SYSTEM_SECURITY = 0x01000000,

		MAXIMUM_ALLOWED = 0x02000000,

		GENERIC_READ = 0x80000000,
		GENERIC_WRITE = 0x40000000,
		GENERIC_EXECUTE = 0x20000000,
		GENERIC_ALL = 0x10000000,

		DESKTOP_READOBJECTS = 0x00000001,
		DESKTOP_CREATEWINDOW = 0x00000002,
		DESKTOP_CREATEMENU = 0x00000004,
		DESKTOP_HOOKCONTROL = 0x00000008,
		DESKTOP_JOURNALRECORD = 0x00000010,
		DESKTOP_JOURNALPLAYBACK = 0x00000020,
		DESKTOP_ENUMERATE = 0x00000040,
		DESKTOP_WRITEOBJECTS = 0x00000080,
		DESKTOP_SWITCHDESKTOP = 0x00000100,

		WINSTA_ENUMDESKTOPS = 0x00000001,
		WINSTA_READATTRIBUTES = 0x00000002,
		WINSTA_ACCESSCLIPBOARD = 0x00000004,
		WINSTA_CREATEDESKTOP = 0x00000008,
		WINSTA_WRITEATTRIBUTES = 0x00000010,
		WINSTA_ACCESSGLOBALATOMS = 0x00000020,
		WINSTA_EXITWINDOWS = 0x00000040,
		WINSTA_ENUMERATE = 0x00000100,
		WINSTA_READSCREEN = 0x00000200,

		WINSTA_ALL_ACCESS = 0x0000037f

	}

	[Flags]
	public enum CreateProcessFlags : uint {

		DEBUG_PROCESS = 0x00000001,
		DEBUG_ONLY_THIS_PROCESS = 0x00000002,
		CREATE_SUSPENDED = 0x00000004,
		DETACHED_PROCESS = 0x00000008,
		CREATE_NEW_CONSOLE = 0x00000010,
		NORMAL_PRIORITY_CLASS = 0x00000020,
		IDLE_PRIORITY_CLASS = 0x00000040,
		HIGH_PRIORITY_CLASS = 0x00000080,
		REALTIME_PRIORITY_CLASS = 0x00000100,
		CREATE_NEW_PROCESS_GROUP = 0x00000200,
		CREATE_UNICODE_ENVIRONMENT = 0x00000400,
		CREATE_SEPARATE_WOW_VDM = 0x00000800,
		CREATE_SHARED_WOW_VDM = 0x00001000,
		CREATE_FORCEDOS = 0x00002000,
		BELOW_NORMAL_PRIORITY_CLASS = 0x00004000,
		ABOVE_NORMAL_PRIORITY_CLASS = 0x00008000,
		INHERIT_PARENT_AFFINITY = 0x00010000,
		INHERIT_CALLER_PRIORITY = 0x00020000,
		CREATE_PROTECTED_PROCESS = 0x00040000,
		EXTENDED_STARTUPINFO_PRESENT = 0x00080000,
		PROCESS_MODE_BACKGROUND_BEGIN = 0x00100000,
		PROCESS_MODE_BACKGROUND_END = 0x00200000,
		CREATE_BREAKAWAY_FROM_JOB = 0x01000000,
		CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 0x02000000,
		CREATE_DEFAULT_ERROR_MODE = 0x04000000,
		CREATE_NO_WINDOW = 0x08000000,
		PROFILE_USER = 0x10000000,
		PROFILE_KERNEL = 0x20000000,
		PROFILE_SERVER = 0x40000000,
		CREATE_IGNORE_SYSTEM_DEFAULT = 0x80000000

	}

	public enum SECURITY_IMPERSONATION_LEVEL {

		SecurityAnonymous,
		SecurityIdentification,
		SecurityImpersonation,
		SecurityDelegation

	}

	public enum TOKEN_INFORMATION_CLASS {

		TokenUser = 1,
		TokenGroups,
		TokenPrivileges,
		TokenOwner,
		TokenPrimaryGroup,
		TokenDefaultDacl,
		TokenSource,
		TokenType,
		TokenImpersonationLevel,
		TokenStatistics,
		TokenRestrictedSids,
		TokenSessionId,
		TokenGroupsAndPrivileges,
		TokenSessionReference,
		TokenSandBoxInert,
		TokenAuditPolicy,
		TokenOrigin,
		MaxTokenInfoClass

	}

	public enum TOKEN_TYPE {

		TokenPrimary = 1,
		TokenImpersonation

	}

	public const int READ_CONTROL = 0x00020000;
	public const int STANDARD_RIGHTS_REQUIRED = 0x000F0000;
	public const int STANDARD_RIGHTS_READ = READ_CONTROL;
	public const int STANDARD_RIGHTS_WRITE = READ_CONTROL;
	public const int STANDARD_RIGHTS_EXECUTE = READ_CONTROL;
	public const int STANDARD_RIGHTS_ALL = 0x001F0000;
	public const int SPECIFIC_RIGHTS_ALL = 0x0000FFFF;
	public const int TOKEN_ASSIGN_PRIMARY = 0x0001;
	public const int TOKEN_DUPLICATE = 0x0002;
	public const int TOKEN_IMPERSONATE = 0x0004;
	public const int TOKEN_QUERY = 0x0008;
	public const int TOKEN_QUERY_SOURCE = 0x0010;
	public const int TOKEN_ADJUST_PRIVILEGES = 0x0020;
	public const int TOKEN_ADJUST_GROUPS = 0x0040;
	public const int TOKEN_ADJUST_DEFAULT = 0x0080;
	public const int TOKEN_ADJUST_SESSIONID = 0x0100;

	public const int TOKEN_ALL_ACCESS = TOKEN_ADJUST_SESSIONID |
	                                    STANDARD_RIGHTS_REQUIRED |
	                                    TOKEN_ASSIGN_PRIMARY |
	                                    TOKEN_DUPLICATE |
	                                    TOKEN_IMPERSONATE |
	                                    TOKEN_QUERY |
	                                    TOKEN_QUERY_SOURCE |
	                                    TOKEN_ADJUST_PRIVILEGES |
	                                    TOKEN_ADJUST_GROUPS |
	                                    TOKEN_ADJUST_DEFAULT;

	[StructLayout(LayoutKind.Sequential)]
	public struct PROCESS_INFORMATION {

		public IntPtr hProcess;
		public IntPtr hThread;
		public int dwProcessId;
		public int dwThreadId;

	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct STARTUPINFO {

		public Int32 cb;
		public string lpReserved;
		public string lpDesktop;
		public string lpTitle;
		public Int32 dwX;
		public Int32 dwY;
		public Int32 dwXSize;
		public Int32 dwYSize;
		public Int32 dwXCountChars;
		public Int32 dwYCountChars;
		public Int32 dwFillAttribute;
		public Int32 dwFlags;
		public Int16 wShowWindow;
		public Int16 cbReserved2;
		public IntPtr lpReserved2;
		public IntPtr hStdInput;
		public IntPtr hStdOutput;
		public IntPtr hStdError;

	}

	/// <summary>
	///     Opens a handle to the access token associated with a process.
	/// </summary>
	/// <param name="ProcessHandle"></param>
	/// <param name="DesiredAccess"></param>
	/// <param name="TokenHandle"></param>
	/// <returns></returns>
	[DllImport("advapi32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool OpenProcessToken(
		IntPtr ProcessHandle,
		uint DesiredAccess,
		out IntPtr TokenHandle
	);

	/// <summary>
	///     Creates a new process, using the credentials supplied by hToken.
	///     The application opened is running under the credentials and
	///     authority for the user supplied to LogonUser.
	/// </summary>
	/// <param name="hToken"></param>
	/// <param name="lpApplicationName"></param>
	/// <param name="lpCommandLine"></param>
	/// <param name="lpProcessAttributes"></param>
	/// <param name="lpThreadAttributes"></param>
	/// <param name="bInheritHandles"></param>
	/// <param name="dwCreationFlags"></param>
	/// <param name="lpEnvironment"></param>
	/// <param name="lpCurrentDirectory"></param>
	/// <param name="lpStartupInfo"></param>
	/// <param name="lpProcessInformation"></param>
	/// <returns></returns>
	[DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
	public static extern bool CreateProcessAsUser(
		IntPtr hToken,
		string? lpApplicationName,
		string lpCommandLine,
		IntPtr lpProcessAttributes,
		IntPtr lpThreadAttributes,
		bool bInheritHandles,
		uint dwCreationFlags,
		IntPtr lpEnvironment,
		string lpCurrentDirectory,
		ref STARTUPINFO lpStartupInfo,
		out PROCESS_INFORMATION lpProcessInformation
	);

	/// <summary>
	///     The DuplicateTokenEx function creates a new access token that
	///     duplicates an existing token. This function can create either
	///     a primary token or an impersonation token.
	/// </summary>
	/// <param name="hExistingToken"></param>
	/// <param name="dwDesiredAccess"></param>
	/// <param name="lpTokenAttributes"></param>
	/// <param name="ImpersonationLevel"></param>
	/// <param name="TokenType"></param>
	/// <param name="phNewToken"></param>
	/// <returns></returns>
	[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern bool DuplicateTokenEx(
		IntPtr hExistingToken,
		uint dwDesiredAccess,
		IntPtr lpTokenAttributes,
		SECURITY_IMPERSONATION_LEVEL ImpersonationLevel,
		TOKEN_TYPE TokenType,
		out IntPtr phNewToken
	);

	/// <summary>
	///     The SetTokenInformation function sets various types of information
	///     for a specified access token. The information that this function
	///     sets replaces existing information. The calling process must have
	///     appropriate access rights to set the information.
	/// </summary>
	/// <param name="TokenHandle"></param>
	/// <param name="TokenInformationClass"></param>
	/// <param name="TokenInformation"></param>
	/// <param name="TokenInformationLength"></param>
	/// <returns></returns>
	[DllImport("advapi32.dll", SetLastError = true)]
	public static extern bool SetTokenInformation(
		IntPtr TokenHandle,
		TOKEN_INFORMATION_CLASS TokenInformationClass,
		ref uint TokenInformation,
		uint TokenInformationLength
	);

	/// <summary>
	///     Locks the workstation's display. Locking a workstation protects it
	///     from unauthorized use.
	/// </summary>
	/// <returns></returns>
	[DllImport("user32.dll", SetLastError = true)]
	public static extern bool LockWorkStation();

	/// <summary>
	///     Obtains the primary access token of the logged-on user specified
	///     by the session ID. To call this function successfully, the calling
	///     application must be running within the context of the LocalSystem
	///     account and have the SE_TCB_NAME privilege.
	///     Caution  WTSQueryUserToken is intended for highly trusted services.
	///     Service providers must use caution that they do not leak user tokens
	///     when calling this function. Service providers must close token
	///     handles after they have finished using them.
	/// </summary>
	/// <param name="sessionId"></param>
	/// <param name="Token"></param>
	/// <returns></returns>
	[DllImport("wtsapi32.dll", SetLastError = true)]
	public static extern bool WTSQueryUserToken(
		uint sessionId,
		out IntPtr Token
	);

	/// <summary>
	///     Retrieves the session identifier of the console session. The console
	///     session is the session that is currently attached to the physical
	///     console. Note that it is not necessary that Remote Desktop Services
	///     be running for this function to succeed.
	/// </summary>
	/// <returns></returns>
	[DllImport("kernel32.dll")]
	public static extern uint WTSGetActiveConsoleSessionId();

}