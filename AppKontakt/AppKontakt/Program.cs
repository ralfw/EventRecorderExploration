using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppKontakt
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using(var er = new EventRecorder())
            using (var rm = new ReadModel(er))
            {
                var dom = new Domäne(rm, er);
                var view = new View();

                er.Aufgenommen += rm.Nachführen;

                view.Veränderte_Daten += dom.Speichern;
                dom.Tabelle += view.Anzeigen;

                dom.Starten();

                Application.Run(view);
            }
        }
    }
}
