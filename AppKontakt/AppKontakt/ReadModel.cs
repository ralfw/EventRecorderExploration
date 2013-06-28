using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace AppKontakt
{
    class ReadModel : IDisposable
    {
        private readonly EventRecorder _eventRecorder;
        private DataTable _tb;

        public ReadModel(EventRecorder eventRecorder)
        {
            _eventRecorder = eventRecorder;
        }


        public DataTable Laden()
        {
            _tb = new DataTable();
            _tb.Columns.Add("Id", typeof(string));
            _tb.Columns.Add("Name", typeof(string));
            _tb.Columns.Add("Straße", typeof(string));
            _tb.Columns.Add("PLZ", typeof(string));
            _tb.Columns.Add("Ort", typeof(string));
            _tb.Columns.Add("Tel", typeof(string));

            foreach (var e in _eventRecorder.Wiedergeben())
                Initialisieren(e);

            return _tb;
        }

        private void Initialisieren(Domänenevent e)
        {
            DataRow r;

            switch (e.Name)
            {
                case "NeuerKontakt":
                    var i = int.Parse(e.Payload["RowIndex"]);
                    r = _tb.Rows.Add(new object[_tb.Columns.Count]);
                    foreach (var p in e.Payload)
                        if (_tb.Columns.Contains(p.Key)) 
                            r[p.Key] = p.Value;
                    r["Id"] = e.Entitätsid;
                    break;

                case "KontaktVerändert":
                    r = _tb.Rows.Cast<object>().Cast<DataRow>()
                                .First(_ => _["Id", DataRowVersion.Original].ToString() == e.Entitätsid);
                    foreach (var p in e.Payload)
                        r[p.Key] = p.Value;
                    break;
                case "KontaktGelöscht":
                    r = _tb.Rows.Cast<object>().Cast<DataRow>()
                                .First(_ => _["Id", DataRowVersion.Original].ToString() == e.Entitätsid);
                    r.Delete();
                    break;
            }

            _tb.AcceptChanges();
        }


        public void Nachführen(Domänenevent e)
        {
            DataRow r;

            switch (e.Name)
            {
                case "NeuerKontakt":
                    var i = int.Parse(e.Payload["RowIndex"]);
                    r = _tb.Rows[int.Parse(e.Payload["RowIndex"])];
                    r["Id"] = e.Entitätsid;
                    r.AcceptChanges();
                    break;

                case "KontaktVerändert":
                case "KontaktGelöscht":
                    r = _tb.Rows.Cast<object>().Cast<DataRow>()
                                .First(_ => _["Id", DataRowVersion.Original].ToString() == e.Entitätsid);
                    r.AcceptChanges();
                    break;
            }
        }


        public void Dispose() {}
    }
}
