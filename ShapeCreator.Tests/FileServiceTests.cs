using Moq;
using ShapeCreator.Models;
using ShapeCreator.Services;
using ShapeCreator.Services.Interface;

namespace ShapeCreator.Tests
{
    [TestFixture]
    public class FileServiceTests
    {
        private Mock<ILoggerService> mockLoggerService;
        private Mock<IUiService> mockUiService;
        private FileService fileService;
        private string tempFilePath;

        [SetUp]
        public void Setup()
        {
            mockLoggerService = new Mock<ILoggerService>();
            mockUiService = new Mock<IUiService>();
            fileService = new FileService(mockLoggerService.Object, mockUiService.Object);
            tempFilePath = Path.GetTempFileName();
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }

        [Test]
        public void FileService_GetRootFromFile_Valid_Test()
        {
            string json = "{ \"shapes\": [ { \"id\": \"00000000000000000000000000000003\", \"name\": \"вытянутый ромб\", \"shapeType\": \"Rhombus\", \"coordinateStart\": { \"x\": 400, \"y\": 50 }, \"coordinateFinish\": { \"x\": 500, \"y\": 100 }, \"isActive\": true } ] }";
            File.WriteAllText(tempFilePath, json);

            (bool isValid, string errorMessage, Root root) = fileService.GetRootFromFile(tempFilePath);

            Assert.That(isValid, Is.True);
        }

        [Test]
        public void FileService_GetRootFromFile_InValidNonExistFile_Test()
        {
            (bool isValid, string errorMessage, Root root) = fileService.GetRootFromFile(tempFilePath);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void FileService_GetRootFromFile_InValidEmptyFile_Test()
        {
            File.WriteAllText(tempFilePath, string.Empty);

            (bool isValid, string errorMessage, Root root) = fileService.GetRootFromFile(tempFilePath);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void FileService_GetRootFromFile_InValidJsonError_Test()
        {
            string json = "{\r\n  \"shapes\": [\r\n}";
            File.WriteAllText(tempFilePath, json);

            (bool isValid, string errorMessage, Root root) = fileService.GetRootFromFile(tempFilePath);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void FileService_SaveToFile_Valid_Test()
        {
            var root = new Root()
            {
                Shapes = new List<Shape>()
                {
                    new Shape()
                    {
                        CoordinateStart = new Coordinate(){ X = 10, Y = 10 },
                        CoordinateFinish= new Coordinate(){ X = 50, Y = 50 },
                        Id = "",
                        isActive = true,
                        ShapeType = ShapeType.Circle,
                    }
                }
            };

            mockUiService
                .Setup(x => x.SaveFileDialog())
                .Returns((true, string.Empty, tempFilePath));

            (bool? isValid, string errorMessage) = fileService.SaveToFile(root);

            Assert.That(isValid, Is.True);
        }

        [Test]
        public void FileService_SaveToFile_InValid_Test()
        {
            var root = new Root()
            {
                Shapes = new List<Shape>()
                {
                    new Shape()
                    {
                        CoordinateStart = new Coordinate(){ X = 10, Y = 10 },
                        CoordinateFinish= new Coordinate(){ X = 50, Y = 50 },
                        Id = "",
                        isActive = true,
                        ShapeType = ShapeType.Circle,
                    }
                }
            };

            mockUiService
                .Setup(x => x.SaveFileDialog())
                .Returns((false, "error", tempFilePath));

            (bool? isValid, string errorMessage) = fileService.SaveToFile(root);

            Assert.That(isValid, Is.False);
        }
    }
}
