using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AppKontakt
{
    class KontakteAggregat
    {
        readonly Dictionary<string, Kontakt> _entitäten = new Dictionary<string, Kontakt>();


        public void Rehydrieren(IEnumerable<Domänenevent> events)
        {
            foreach (var e in events)
            {
                Kontakt k;

                switch (e.Name)
                {
                    case "NeuerKontakt":
                        k = new Kontakt();
                        k.Id = e.Entitätsid;
                        _entitäten.Add(k.Id, k);
                        k.Name = e.Payload["Name"];
                        k.Straße = e.Payload["Straße"];
                        k.PLZ = e.Payload["PLZ"];
                        k.Ort = e.Payload["Ort"];
                        k.Tel = e.Payload["Tel"];
                        break;
                    case "KontaktVerändert":
                        k = _entitäten[e.Entitätsid];
                        k.Name = e.Payload["Name"];
                        k.Straße = e.Payload["Straße"];
                        k.PLZ = e.Payload["PLZ"];
                        k.Ort = e.Payload["Ort"];
                        k.Tel = e.Payload["Tel"];
                        break;
                    case "KontaktGelöscht":
                        _entitäten.Remove(e.Entitätsid);
                        break;
                }
            }
        }


        public IEnumerable<Kontakt> Entitäten
        {
            get
            {
                return _entitäten.Select(kvp => kvp.Value);
            }
        } 


        public IEnumerable<Domänenevent> Events_aus_Veränderungen_ableiten(DataTable tb)
        {
            for (var i = 0; i < tb.Rows.Count; i++)
            {
                var k = tb.Rows[i];

                if (k.RowState != DataRowState.Unchanged)
                {
                    var e = new Domänenevent();
                    switch (k.RowState)
                    {
                        case DataRowState.Added:
                            e.Name = "NeuerKontakt";
                            e.Entitätsid = Guid.NewGuid().ToString();
                            e.Payload.Add("RowIndex", i.ToString());
                            Veränderungen_in_Payload_eintragen(tb, e, k);
                            break;

                        case DataRowState.Modified:
                            e.Name = "KontaktVerändert";
                            e.Entitätsid = k["Id"].ToString();
                            Veränderungen_in_Payload_eintragen(tb, e, k);
                            break;

                        case DataRowState.Deleted:
                            e.Name = "KontaktGelöscht";
                            e.Entitätsid = k["Id", DataRowVersion.Original].ToString();
                            break;
                    }
                    yield return e;
                }
            }
        }

        private static void Veränderungen_in_Payload_eintragen(DataTable tb, Domänenevent e, DataRow k)
        {
            for (var ci = 0; ci < tb.Columns.Count; ci++)
                e.Payload.Add(tb.Columns[ci].ColumnName, k[ci].ToString());
        }
    }
}