using System;
using System.Collections.Generic;
using System.Data;

namespace AppKontakt
{
    class Map
    {
        //public DataTable Entitäten_nach_Tabelle(IEnumerable<Kontakt> kontakte)
        //{
        //    var tb = new DataTable();
        //    tb.Columns.Add("Id", typeof(string));
        //    tb.Columns.Add("Name", typeof(string));
        //    tb.Columns.Add("Straße", typeof(string));
        //    tb.Columns.Add("PLZ", typeof(string));
        //    tb.Columns.Add("Ort", typeof(string));
        //    tb.Columns.Add("Tel", typeof(string));

        //    foreach (var k in kontakte)
        //        tb.Rows.Add(new[] { k.Id, k.Name, k.Straße, k.PLZ, k.Ort, k.Tel });

        //    tb.AcceptChanges();

        //    return tb;
        //}


        //public void Tabelle_aktualisieren(DataTable tb, IEnumerable<Domänenevent> events)
        //{
        //    foreach (var e in events)
        //        if (e.Name == "NeuerKontakt")
        //            tb.Rows[int.Parse(e.Payload["RowIndex"])]["Id"] = e.Entitätsid;
        //    tb.AcceptChanges();
        //}





        public IEnumerable<Domänenkommando> Kommandos_aus_Veränderungen_generieren(DataTable tb)
        {
            for (var i = 0; i < tb.Rows.Count; i++)
            {
                var k = tb.Rows[i];

                if (k.RowState != DataRowState.Unchanged)
                {
                    var cmd = new Domänenkommando();
                    switch (k.RowState)
                    {
                        case DataRowState.Added:
                            cmd.Name = "KontaktAnlegen";
                            Veränderungen_in_Payload_eintragen(tb, cmd, k);
                            break;

                        case DataRowState.Modified:
                            cmd.Name = "KontaktAktualisieren";
                            cmd.Entitätsid = k["Id"].ToString();
                            Veränderungen_in_Payload_eintragen(tb, cmd, k);
                            break;

                        case DataRowState.Deleted:
                            cmd.Name = "KontaktLöschen";
                            cmd.Entitätsid = k["Id", DataRowVersion.Original].ToString();
                            break;
                    }
                    yield return cmd;
                }
            }
        }

        private static void Veränderungen_in_Payload_eintragen(DataTable tb, Domänenkommando e, DataRow k)
        {
            for (var ci = 0; ci < tb.Columns.Count; ci++)
                e.Payload.Add(tb.Columns[ci].ColumnName, k[ci].ToString());
        }
    }
}