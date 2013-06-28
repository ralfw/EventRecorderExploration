using System;
using System.Collections.Generic;
using System.Data;

namespace AppKontakt
{
    class Map
    {
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