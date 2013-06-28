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
        private const string PATH = @"ReadModel.Daten";

        private DataTable _tb;

        public ReadModel(EventRecorder eventRecorder)
        {
            _eventRecorder = eventRecorder;
            Directory.CreateDirectory(PATH);
        }


        public DataTable Laden()
        {
            if (!File.Exists(Path.Combine(PATH, "tabelle")))
            {
                _tb = new DataTable();
                _tb.Columns.Add("Id", typeof(string));
                _tb.Columns.Add("Name", typeof(string));
                _tb.Columns.Add("Straße", typeof(string));
                _tb.Columns.Add("PLZ", typeof(string));
                _tb.Columns.Add("Ort", typeof(string));
                _tb.Columns.Add("Tel", typeof(string));

                Speichern();
            }

            using (var fs = new FileStream(Path.Combine(PATH, "tabelle"), FileMode.Open))
            {
                var bf = new BinaryFormatter();
                _tb = (DataTable) bf.Deserialize(fs);
            }

            return _tb;
        }

        private void Speichern()
        {
            using (var fs = new FileStream(Path.Combine(PATH, "tabelle"), FileMode.Create))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(fs, _tb);
            }
        }


        public void Nachführen(Domänenevent e)
        {
            DataRow r;

            switch (e.Name)
            {
                case "NeuerKontakt":
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


        public void Dispose()
        {
            Speichern();
            _tb = null;
        }
    }
}
