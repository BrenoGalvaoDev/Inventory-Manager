using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gerenciador_De_Estoque
{
    /// <summary>
    /// The static class containing the main entry point for the application.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Enables visual styles for the application (e.g., Aero/Luna styles on Windows).
            Application.EnableVisualStyles();

            // Sets the default rendering mode for text in controls.
            Application.SetCompatibleTextRenderingDefault(false);

            // Runs the application starting with the main form instance (named 'Main').
            Application.Run(new Main());
        }
    }
}