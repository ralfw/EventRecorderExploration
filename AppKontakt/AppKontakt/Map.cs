using System.Collections.Generic;
using System.Data;

namespace AppKontakt
{
    class Map
    {
        public DataTable Entitäten_nach_Tabelle(IEnumerable<Kontakt> kontakte)
        {
            var tb = new DataTable();
            tb.Columns.Add("Id", typeof(string));
            tb.Columns.Add("Name", typeof(string));
            tb.Columns.Add("Straße", typeof(string));
            tb.Columns.Add("PLZ", typeof(string));
            tb.Columns.Add("Ort", typeof(string));
            tb.Columns.Add("Tel", typeof(string));

            foreach (var k in kontakte)
                tb.Rows.Add(new[] { k.Id, k.Name, k.Straße, k.PLZ, k.Ort, k.Tel });

            tb.AcceptChanges();

            return tb;
        }


        public void Tabelle_aktualisieren(DataTable tb, IEnumerable<Domänenevent> events)
        {
            foreach (var e in events)
                if (e.Name == "NeuerKontakt")
                    tb.Rows[int.Parse(e.Payload["RowIndex"])]["Id"] = e.Entitätsid;
            tb.AcceptChanges();
        }
    }
}