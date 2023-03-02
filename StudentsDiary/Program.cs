using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentsDiary
{
    internal static class Program
    {
        public static string FilePath = Path.Combine($@"{Path.GetDirectoryName(Application.ExecutablePath)}", "Students.txt");
        public static string GroupIDListPath = Path.Combine($@"{Path.GetDirectoryName(Application.ExecutablePath)}", "GroupIDList.txt");
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
