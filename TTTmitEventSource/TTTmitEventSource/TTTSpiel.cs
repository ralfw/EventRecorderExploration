using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTTmitEventSource
{
    public enum Spielstände
    {
        Läuft,
        GewinnX,
        GewinnO,
        Unentschieden
    }


    public class TTTSpiel
    {
        private readonly List<string> _eventsource;

        public TTTSpiel() : this(new List<string>()) {}
        internal TTTSpiel(IEnumerable<string> eventsource)
        {
            _eventsource = new List<string>(eventsource);
        }


        public Tuple<string[,], Spielstände> Ziehen(string spielfeldkoordinate)
        {
            validieren(spielfeldkoordinate);
            ausführen(spielfeldkoordinate);
            var spielstand = Spielstand_ermitteln();
            Spieler_wechseln();
            return ReadModel_generieren(spielstand);
        }


        private void validieren(string spielfeldkoordinate)
        {
            if (_eventsource.Any(e => e == spielfeldkoordinate))
                throw new InvalidOperationException("Ungültiger Zug! Spielfeld schon belegt.");
        }

        private void ausführen(string spielfeldkoordinate)
        {
            _eventsource.Add(spielfeldkoordinate);
        }

        private Spielstände Spielstand_ermitteln()
        {
            if (_eventsource.Count == 9) return Spielstände.Unentschieden;

            foreach (var reihe in new[] { new[] { "00", "01", "02" }, new[] { "10", "11", "12" }, new[] { "20", "21", "22" },
                                            new[] { "00", "10", "20" }, new[] { "01", "11", "21" }, new[] { "02", "12", "22" },
                                            new[] { "00", "11", "11" }, new[] { "20", "11", "02" }})
            {
                var spielstand = Prüfe_Reihe_auf_Gewinn(reihe);
                if (spielstand != Spielstände.Läuft) return spielstand;
            }

            return Spielstände.Läuft;
        }

        private Spielstände Prüfe_Reihe_auf_Gewinn(IEnumerable<string> reihe)
        {
            var belegungssumme = _eventsource.Select((e, i) => new { Zug = e, Index = i })
                                                .Where(zi => reihe.Contains(zi.Zug))
                                                .Select(zi => zi.Index % 2 == 0 ? 1 : -1)
                                                .Sum();
            if (Math.Abs(belegungssumme) != 3) return Spielstände.Läuft;
            return belegungssumme > 0 ? Spielstände.GewinnX : Spielstände.GewinnO;
        }

        private void Spieler_wechseln()
        {
            // nichts zu tun
        }

        private Tuple<string[,], Spielstände> ReadModel_generieren(Spielstände spielstand)
        {
            var spielbrett = _eventsource.Select((e, i) => new {Zug=e, Index=i})
                                         .Aggregate(new string[3,3], (sb, zi) => {
                                                    var zeile = int.Parse(zi.Zug[0].ToString());
                                                    var spalte = int.Parse(zi.Zug[1].ToString());
                                                    sb[zeile, spalte] = Bestimme_Spieler(zi.Index);
                                                    return sb;
                                         });

            return new Tuple<string[,], Spielstände>(spielbrett, spielstand);
        }

        private static string Bestimme_Spieler(int zugIndex)
        {
            return zugIndex % 2 == 0 ? "X" : "O";
        }
    }
}
