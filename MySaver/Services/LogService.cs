namespace MySaver.Services;

static class LogService
{
    private static string path;
    private static ReaderWriterLock rwl = new ReaderWriterLock();

    static LogService()
    {
#if WINDOWS
        path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#elif ANDROID
        path = "/storage/emulated/0/Documents";
#endif

        path = Path.Combine(path, "MySaver");
        Directory.CreateDirectory(path);
        path = Path.Combine(path, DateTime.Now.ToString("yyMMdd_HHmmss") + ".txt");
    }

    public static void Log(String message)
    {
        try
        {
            rwl.AcquireWriterLock(10000);

            try
            {
                File.AppendAllText(path, $"{DateTime.Now.ToString("HH:mm:ss")} {message}\n");
            }
            finally
            {
                rwl.ReleaseWriterLock();
            }
        }
        catch (ApplicationException) { }
    }
}
