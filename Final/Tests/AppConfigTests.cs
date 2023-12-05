namespace FinalProjectTests
{
    [TestFixture]
    public class AppConfigTests
    {
        BitsContext dbContext;
        AppConfig config;

        [SetUp]
        public void Setup()
        {
            dbContext = new BitsContext();
        }

        [Test]
        public void GetBreweryInfo()
        {
            // Arrange
            // Act
            config = dbContext.AppConfigs.Find(1);

            // Assert
            Assert.AreEqual(config.BreweryId, 1);
            Assert.AreEqual(config.BreweryName, "Manifest");
            Assert.AreEqual(config.DefaultUnits, "metric");
            Assert.AreEqual(config.HomePageText, "Description of the brewery goes here");
            Assert.AreEqual(config.Color1, "c3e2ed");
        }
    }
}