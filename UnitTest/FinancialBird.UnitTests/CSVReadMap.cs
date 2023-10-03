using FinancialBird.VereinigteVolksbank;

namespace FinancialBird.UnitTests
{
    public class VereinigteVolksbankCSV
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            IUmsatzMapper mapper = new CSVMapper();
            string csvFile = @"..\..\..\Application\TestData\Kontoauszüge_csv\Umsaetze_DummyDaten.csv";

            var umsaetze = mapper.FileReader(csvFile);

            Assert.Pass();
        }
    }
}