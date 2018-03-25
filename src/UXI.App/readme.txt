Add single instance support to the app:

1. Update properties of the App.xaml:
   - Build Action: Page
   - Copy to Output Directory: Do not copy

2. In project's properties set Startup object to the App class.

3. Add the following code to the App.xaml.cs

	/// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {
        private const string AppIdentification = "<YourAppName>";


        [STAThread]
        public static void Main()
        {
            if (DesignTimeHelper.IsDesignTime == false && SingleInstance<App>.InitializeAsFirstInstance(AppIdentification))
            {
                var application = new App();
                                                            
                application.InitializeComponent();
                application.Run();

                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            return true;
        }

	// ...
	}