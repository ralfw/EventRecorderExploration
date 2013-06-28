using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppKontakt
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var er = new EventRecorder();
            var dom = new Domäne(er);
            var dlg = new View();

            dlg.Veränderte_Daten += dom.Speichern;
            dom.Aktuelle_Kontakte += dlg.Anzeigen;

            dom.Starten();

            Application.Run(dlg);
        }
    }
}
