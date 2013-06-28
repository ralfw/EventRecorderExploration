using System.Collections.Generic;
using System.IO;

namespace AppKontakt
{
    internal class Domänenevent
    {
        public string Name;
        public int Laufende_Nummer;
        public string Entitätsid;
        public Dictionary<string, string> Payload = new Dictionary<string, string>();


        public static Domänenevent Parse(string eventText)
        {
            var sr = new StringReader(eventText);
            var e = new Domänenevent {
                Name = sr.ReadLine(),
                Laufende_Nummer = int.Parse(sr.ReadLine()),
                Entitätsid = sr.ReadLine()
            };
            do
            {
                var l = sr.ReadLine();
                if (string.IsNullOrEmpty(l)) break;

                var kv = l.Split('\t');
                e.Payload.Add(kv[0], kv[1]);
            } while (true);
            return e;
        }


        public override string ToString()
        {
            var sw = new StringWriter();
            sw.WriteLine(Name);
            sw.WriteLine(Laufende_Nummer);
            sw.WriteLine(Entitätsid);
            foreach(var p in Payload)
                sw.WriteLine("{0}\t{1}", p.Key, p.Value);
            return sw.ToString();
        }
    }
}