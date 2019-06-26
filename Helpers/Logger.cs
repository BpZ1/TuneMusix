using System;
using TuneMusix.Model;

namespace TuneMusix.Helpers
{
    /// <summary>
    /// Class for Logging of the Program
    /// </summary>
    static class Logger
    {    
        public static void Log(String lines)
        {
            Options options = Options.Instance;
            // Write the string to a file.append mode is enabled so that the log
            // lines get appended to  test.txt than wiping content and writing the log
            if (options.LoggerActive)
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() + "\\log.txt", true);
                file.WriteLine(Convert.ToString(System.DateTime.Now) + " ----- " + lines + "-----");

                file.Close();
            }
        }
        public static void LogException(Exception ex)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() + "\\log.txt",true);
            file.WriteLine(Convert.ToString(System.DateTime.Now) + " ----- " + ex.Message + "-----\n" + ex.ToString());

            file.Close();
        }
    }
}
