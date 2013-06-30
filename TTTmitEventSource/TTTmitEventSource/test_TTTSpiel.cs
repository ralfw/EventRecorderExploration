using NUnit.Framework;

namespace TTTmitEventSource
{
    [TestFixture]
    public class test_TTTSpiel
    {
        [Test]
        public void Ziehen()
        {
            var sut = new TTTSpiel(new[]{"00", "11", "01", "02", "20", "12"});

            var result = sut.Ziehen("22");

            Assert.AreEqual("O", result.Item1[1,2]);
            Assert.AreEqual("X", result.Item1[2,2]);
            Assert.AreEqual(Spielstände.Läuft, result.Item2);
        }


        [Test]
        public void Gewinn_feststellen()
        {
            var sut = new TTTSpiel(new[] { "00", "11", "01", "02", "20", "12", "22" });

            var result = sut.Ziehen("10");

            Assert.AreEqual(Spielstände.GewinnO, result.Item2);
        }
    }
}