using Moq;
using ShapeCreator.Models;
using ShapeCreator.Services;
using ShapeCreator.Services.Interface;

namespace ShapeCreator.Tests
{
    [TestFixture]
    public class ConfigServiceTests
    {
        private Mock<ILoggerService> mockLoggerService;
        private Mock<IFileService> mockFileService;
        private Mock<IUiService> mockUiService;

        private ConfigService configService;

        [SetUp]
        public void Setup()
        {
            mockLoggerService = new Mock<ILoggerService>();
            mockFileService = new Mock<IFileService>();
            mockUiService = new Mock<IUiService>();

            configService = new ConfigService(
                mockLoggerService.Object,
                mockFileService.Object,
                mockUiService.Object);
        }

        [Test]
        public void ConfigService_LoadTestProject_Valid_Test()
        {
            mockFileService
                .Setup(x => x.GetRootFromFile(It.IsAny<string>()))
                .Returns((true, string.Empty, new Root()));

            bool isValid = configService.LoadTestProject();

            Assert.That(isValid, Is.True);
        }

        [Test]
        public void ConfigService_LoadTestProject_InValid_Test()
        {
            mockFileService
                .Setup(x => x.GetRootFromFile(It.IsAny<string>()))
                .Returns((false, "описание ошибки", new Root()));

            bool isValid = configService.LoadTestProject();

            Assert.That(isValid, Is.False);
        }
    }
}
