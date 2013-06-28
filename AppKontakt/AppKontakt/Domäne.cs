using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace AppKontakt
{
    class Domäne
    {
        private readonly EventRecorder _eventRecorder;
        private readonly Map _map;

        public Domäne(EventRecorder eventRecorder)
        {
            _eventRecorder = eventRecorder;
            _map = new Map();
        }


        public void Starten()
        {
            var events = _eventRecorder.Wiedergeben();
            var agg = new KontakteAggregat();
            agg.Rehydrieren(events);
            var tb = _map.Entitäten_nach_Tabelle(agg.Entitäten);
            Aktuelle_Kontakte(tb);
        }

        public void Speichern(DataTable kontakte)
        {
            var agg = new KontakteAggregat();
            var events = agg.Events_aus_Veränderungen_ableiten(kontakte);
            _eventRecorder.Aufnehmen(events);
            _map.Tabelle_aktualisieren(kontakte, events);
            Aktuelle_Kontakte(kontakte);
        }
 

        public event Action<DataTable> Aktuelle_Kontakte;
    }
}
