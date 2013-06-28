using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace AppKontakt
{
    class Domäne
    {
        private readonly ReadModel _readModel;
        private readonly EventRecorder _eventRecorder;
        private readonly Map _map;

        public Domäne(ReadModel readModel, EventRecorder eventRecorder)
        {
            _readModel = readModel;
            _eventRecorder = eventRecorder;
            _map = new Map();
        }


        public void Starten()
        {
            var tb = _readModel.Laden();
            Tabelle(tb);
        }


        public void Speichern(DataTable tb)
        {
            var kommandos = _map.Kommandos_aus_Veränderungen_generieren(tb);
            var agg = new Aggregat();
            var events = agg.Ausführen(kommandos);
            _eventRecorder.Aufnehmen(events);
        }


        public event Action<DataTable> Tabelle;
    }
}
