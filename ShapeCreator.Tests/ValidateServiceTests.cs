using Moq;
using ShapeCreator.Models;
using ShapeCreator.Services;
using ShapeCreator.Services.Interface;

namespace ShapeCreator.Tests
{
    [TestFixture]
    public class ValidateServiceTests
    {
        private Mock<ILoggerService> mockLoggerService;
        private ValidateService validateService;

        [SetUp]
        public void Setup()
        {
            mockLoggerService = new Mock<ILoggerService>();
            validateService = new ValidateService(mockLoggerService.Object);
        }

        [Test]
        public void ValidateService_IsValid_ValidOneShape_Test()
        {
            var shape = new Shape()
            {
                Name = "name",
                Id = "1",
                IsActive = true,
                ShapeType = ShapeType.Circle,
                CoordinateStart = new Coordinate() { X = 0, Y = 0 },
                CoordinateFinish = new Coordinate() { X = 100, Y = 100 },
            };
            var shapes = new List<Shape>() { shape };

            (bool isValid, string message) = validateService.IsValid(shapes);

            Assert.That(isValid, Is.True);
        }

        [Test]
        public void ValidateService_IsValid_ValidTwoShape_Test()
        {
            var shapeOne = new Shape()
            {
                Name = "one",
                Id = "1",
                IsActive = true,
                ShapeType = ShapeType.Circle,
                CoordinateStart = new Coordinate() { X = 150, Y = 100 },
                CoordinateFinish = new Coordinate() { X = 200, Y = 150 },
            };
            var shapeTwo = new Shape()
            {
                Name = "two",
                Id = "2",
                IsActive = true,
                ShapeType = ShapeType.Circle,
                CoordinateStart = new Coordinate() { X = 10, Y = 10 },
                CoordinateFinish = new Coordinate() { X = 50, Y = 50 },
            };
            var shapes = new List<Shape>() { shapeOne, shapeTwo };

            (bool isValid, string message) = validateService.IsValid(shapes);

            Assert.That(isValid, Is.True);
            Assert.That(message, Is.Empty);
        }

        [Test]
        public void ValidateService_IsValid_InValidNegativeСoordinate_Test()
        {
            var shape = new Shape()
            {
                Name = "name",
                Id = "1",
                IsActive = true,
                ShapeType = ShapeType.Circle,
                CoordinateStart = new Coordinate() { X = -100, Y = 100 },
                CoordinateFinish = new Coordinate() { X = 100, Y = 100 },
            };
            var shapes = new List<Shape>() { shape };

            (bool isValid, string message) = validateService.IsValid(shapes);

            Assert.That(isValid, Is.False);
            Assert.That(message, !Is.Empty);
        }

        [Test]
        public void ValidateService_IsValid_InValidDuplicateName_Test()
        {
            var shapeOne = new Shape()
            {
                Name = "name",
                Id = "1",
                IsActive = true,
                ShapeType = ShapeType.Circle,
                CoordinateStart = new Coordinate() { X = 150, Y = 100 },
                CoordinateFinish = new Coordinate() { X = 200, Y = 150 },
            };
            var shapeTwo = new Shape()
            {
                Name = "name",
                Id = "2",
                IsActive = true,
                ShapeType = ShapeType.Circle,
                CoordinateStart = new Coordinate() { X = 10, Y = 10 },
                CoordinateFinish = new Coordinate() { X = 50, Y = 50 },
            };
            var shapes = new List<Shape>() { shapeOne, shapeTwo };

            (bool isValid, string message) = validateService.IsValid(shapes);

            Assert.That(isValid, Is.False);
            Assert.That(message, !Is.Empty);
        }
    }
}
