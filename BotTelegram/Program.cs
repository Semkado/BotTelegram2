using System;
using System.Windows.Forms;

namespace BotTelegram
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var mainForm = new MainForm())
            {
                Application.Run(mainForm);
            }
        }
    }
}
