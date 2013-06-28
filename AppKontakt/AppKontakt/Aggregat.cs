using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AppKontakt
{
    class Aggregat
    {
        //readonly Dictionary<string, Kontakt> _entitäten = new Dictionary<string, Kontakt>();


        //public void Rehydrieren(IEnumerable<Domänenevent> events)
        //{
        //    foreach (var e in events)
        //    {
        //        Kontakt k;

        //        switch (e.Name)
        //        {
        //            case "NeuerKontakt":
        //                k = new Kontakt();
        //                k.Id = e.Entitätsid;
        //                _entitäten.Add(k.Id, k);
        //                k.Name = e.Payload["Name"];
        //                k.Straße = e.Payload["Straße"];
        //                k.PLZ = e.Payload["PLZ"];
        //                k.Ort = e.Payload["Ort"];
        //                k.Tel = e.Payload["Tel"];
        //                break;
        //            case "KontaktVerändert":
        //                k = _entitäten[e.Entitätsid];
        //                k.Name = e.Payload["Name"];
        //                k.Straße = e.Payload["Straße"];
        //                k.PLZ = e.Payload["PLZ"];
        //                k.Ort = e.Payload["Ort"];
        //                k.Tel = e.Payload["Tel"];
        //                break;
        //            case "KontaktGelöscht":
        //                _entitäten.Remove(e.Entitätsid);
        //                break;
        //        }
        //    }
        //}


        public IEnumerable<Domänenevent> Ausführen(IEnumerable<Domänenkommando> kommandos)
        {
            return kommandos.Select((cmd, i) => {
                var e = new Domänenevent();
                switch (cmd.Name)
                {
                    case "KontaktAnlegen":
                        e.Name = "NeuerKontakt";
                        e.Entitätsid = Guid.NewGuid().ToString();
                        e.Payload = cmd.Payload;
                        e.Payload.Add("RowIndex", i.ToString());
                        break;

                    case "KontaktAktualisieren":
                        e.Name = "KontaktVerändert";
                        e.Entitätsid = cmd.Entitätsid;
                        e.Payload = cmd.Payload;
                        break;

                    case "KontaktLöschen":
                        e.Name = "KontaktGelöscht";
                        e.Entitätsid = cmd.Entitätsid;
                        break;
                }
                return e;
            });
        }
    }
}