namespace LoginLauncher;

/// <summary>
/// Main program that starts an application on the Windows login screen.
/// </summary>
internal class Program {

	private static void Main(string[] args) {
		if (args.Length == 0) {
			Console.WriteLine("The full path to the application must be specified as first argument.");
			return;
		}

		string application = args[0];
		if (!Path.Exists(application)) {
			Console.WriteLine("The specified application does not exist.");
			return;
		}

		string applicationArgs = string.Empty;
		if (args.Length > 1) {
			applicationArgs = string.Join(" ", args.Skip(1).Select(a => "\"" + a + "\"").ToArray());
		}

		string currentDirectory = Path.GetDirectoryName(application) ?? string.Empty;
		try {
			LoginLauncher.LaunchApplication(application, currentDirectory, applicationArgs);
		} catch (Exception ex) {
			Console.WriteLine("Application could not be launched.");
			Console.WriteLine(ex.Message);
		}
	}

}