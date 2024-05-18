using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Web.Mvc;
using System.Web;
using Moq;
using TP03MainProj.DataHandle;
using System;

namespace TP03MainProj.Controllers
{
    [TestClass]
    public class EducationControllerTests
    {
        private Mock<HttpContextBase> _mockHttpContext;
        private Mock<HttpServerUtilityBase> _mockServer;
        private EducationController _controller;
        private string _basePath;

        [TestInitialize]
        public void Setup()
        {
            _mockHttpContext = new Mock<HttpContextBase>();
            _mockServer = new Mock<HttpServerUtilityBase>();

            _mockHttpContext.Setup(ctx => ctx.Server).Returns(_mockServer.Object);

            _controller = new EducationController();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };

            // Set the base path to the relative path of the TP03MainProj directory

        }

        [DataTestMethod]
        [DataRow("China")]
        [DataRow("Japan")]
        [DataRow("Korea")]
        [DataRow("Philipines")]
        [DataRow("Vietnam")]
        public void GetCountryData_ReturnsCorrectData(string countryName)
        {
            // Set the base path relative to the TP03MainProj directory
            _basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\TP03MainProj\DataHandle");
            _basePath = Path.GetFullPath(_basePath);
            // Arrange
            var agePath = Path.Combine(_basePath, countryName, "age_distrbution.json");
            var occupationPath = Path.Combine(_basePath, countryName, "occupation.json");
            var populationPath = Path.Combine(_basePath, countryName, "population.json");
            var religionPath = Path.Combine(_basePath, countryName, "religion.json");
            Console.WriteLine($"Age path: {agePath}");
            Console.WriteLine($"Occupation path: {occupationPath}");
            Console.WriteLine($"Population path: {populationPath}");
            Console.WriteLine($"Religion path: {religionPath}");
            // Mock file reading
            var ageData = "[{\"CensusId\": 1, \"CensusYear\": 2020, \"CountryName\": \"" + countryName + "\", \"0-4\": 100, \"5-9\": 150}]";
            var occupationData = "{\"Occupation\": \"Engineer\"}";
            var populationData = "[{\"Year\": 2020, \"Population\": 1000000}]";
            var religionData = "{\"Religion\": \"Christianity\"}";

            MockFileRead(agePath, ageData);
            MockFileRead(occupationPath, occupationData);
            MockFileRead(populationPath, populationData);
            MockFileRead(religionPath, religionData);

            // Act
            var result = _controller.GetCountryData(countryName) as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            var countryData = result.Data as CountryData;
            Assert.IsNotNull(countryData);
            Assert.AreEqual(countryName, countryData.CountryName);
            Assert.AreEqual(1, countryData.ageDistributions.Count);
            Assert.AreEqual(1, countryData.occupation.Count);
            Assert.AreEqual(1, countryData.populations.Count);
            Assert.AreEqual(1, countryData.religions.Count);
        }

        private void MockFileRead(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}
