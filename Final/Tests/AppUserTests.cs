namespace FinalProjectTests
{
    [TestFixture]
    public class AppUserTests
    {
        BitsContext dbContext;
        AppUser user;

        [SetUp]
        public void Setup()
        {
            dbContext = new BitsContext();
        }

        [Test]
        public void AddUser()
        {
            // Arrange
            user = new AppUser();
            user.Name = "Test";

            // Act
            dbContext.AppUsers.Add(user);
            dbContext.SaveChanges();
            AppUser testUser = dbContext.AppUsers.Find(6);

            // Assert
            Assert.IsNotNull(testUser);
            Assert.AreEqual(testUser.Name, "Test");
        }

        [Test]
        public void GetUser()
        {
            // Arrange - none needed here

            // Act
            AppUser user = dbContext.AppUsers.Find(6);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(user.Name, "Test");
        }

        [Test]
        public void DeleteUser()
        {
            // Arrange - none needed here
            AppUser user = dbContext.AppUsers.Find(2);

            // Act
            dbContext.Remove(user);
            dbContext.SaveChanges();

            user = dbContext.AppUsers.Find(2);

            // Assert
            Assert.IsNull(user);        
        }
    }
}