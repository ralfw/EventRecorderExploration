using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AppKontakt
{
    class Aggregat
    {
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