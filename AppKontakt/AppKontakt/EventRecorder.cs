using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppKontakt
{
    class EventRecorder
    {
        private const string PATH = "EventRecorder.Events";


        private int _nächste_laufende_Eventnummer;


        public EventRecorder()
        {
            Directory.CreateDirectory(PATH);

            _nächste_laufende_Eventnummer = 1;
            if (File.Exists(Path.Combine(PATH, "1")))
                _nächste_laufende_Eventnummer = Directory.GetFiles(PATH)
                                                          .Select(Path.GetFileName)
                                                          .Select(int.Parse)
                                                          .Max()
                                                 + 1;
        }


        public void Aufnehmen(IEnumerable<Domänenevent> events)
        {
            foreach (var e in events)
            {
                e.Laufende_Nummer = _nächste_laufende_Eventnummer++;
                File.WriteAllText(Path.Combine(PATH, e.Laufende_Nummer.ToString()), e.ToString());
            }

            foreach (var e in events)
                Aufgenommen(e);
        }


        public IEnumerable<Domänenevent> Wiedergeben()
        {
            return Directory.GetFiles(PATH)
                            .OrderBy(filename => filename)
                            .Select(File.ReadAllText)
                            .Select(Domänenevent.Parse);
        } 


        public event Action<Domänenevent> Aufgenommen = _ => {};
    }
}
