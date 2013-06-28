using System.Collections.Generic;

namespace AppKontakt
{
    internal class Domänenkommando
    {
        public string Name;
        public string Entitätsid;
        public Dictionary<string,string> Payload = new Dictionary<string, string>(); 
    }
}