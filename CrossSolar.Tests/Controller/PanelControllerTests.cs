using System.Collections.Generic;
using System.Threading.Tasks;
using CrossSolar.Controllers;
using CrossSolar.Domain;
using CrossSolar.Models;
using CrossSolar.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CrossSolar.Tests.Controller
{
    public class PanelControllerTests
    {
        public PanelControllerTests()
        {
            _panelController = new PanelController(_panelRepositoryMock.Object);
            _analyticsController = new AnalyticsController(_analyticsRepositoryMock.Object, _panelRepositoryMock.Object);
        }

        private readonly PanelController _panelController;

        private readonly Mock<IPanelRepository> _panelRepositoryMock = new Mock<IPanelRepository>();

        private readonly AnalyticsController _analyticsController;

        private readonly Mock<IAnalyticsRepository> _analyticsRepositoryMock = new Mock<IAnalyticsRepository>();

        [Fact]
        public async Task Register_ShouldInsertPanel()
        {
            var panel = new PanelModel
            {
                Brand = "Areva",
                Latitude = 12.345678,
                Longitude = 98.765543,
                Serial = "AAAA1111BBBB2222"
            };

            // Arrange

            // Act
            var result = await _panelController.Register(panel);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task Register_SerialCheck()
        {
            var panel = new PanelModel
            {
                Brand = "Areva",
                Latitude = 12.345678,
                Longitude = 98.765543,
                Serial = "AAAA1111BBBB22221234"
            };

            // Act
            var result = await _panelController.Register(panel);

            // Assert
            Assert.NotNull(result);
            var createdResult = result as CreatedResult;
            Assert.Null(createdResult);
        }


        [Fact]
        public async Task Register_DecimalPlacesCheck()
        {
            var panel = new PanelModel
            {
                Brand = "Areva",
                Latitude = 12.345678123123,
                Longitude = 98.7655431232,
                Serial = "AAAA1111BBBB2222"
            };
            // Act
            var result = await _panelController.Register(panel);
            // Assert
            Assert.NotNull(result);
            var createdResult = result as CreatedResult;
            Assert.Null(createdResult);
        }

        [Fact]
        public async Task Register_LatitudeLongitudeCheck()
        {
            var panel = new PanelModel
            {
                Brand = "Areva",
                Serial = "AAAA1111BBBB2222"
            };
            // Act
            var result = await _panelController.Register(panel);
            // Assert
            Assert.NotNull(result);
            var createdResult = result as CreatedResult;
            Assert.Null(createdResult);
        }


        [Fact]
        public  void DailyAnalyticsCheck()
        {
            List<OneDayElectricityModel> listMust = new List<OneDayElectricityModel>();
            List<OneHourElectricity> list = new List<OneHourElectricity>();
            OneDayElectricityModel lcOneDayElectricityModel;
            OneHourElectricity lcOneHourElectricity = new OneHourElectricity();
            lcOneHourElectricity.Id = 1;
            lcOneHourElectricity.KiloWatt = 100;
            lcOneHourElectricity.PanelId = 2;
            lcOneHourElectricity.DateTime = new System.DateTime(2018, 8, 24);
            list.Add(lcOneHourElectricity);

            lcOneHourElectricity = new OneHourElectricity();
            lcOneHourElectricity.Id = 2;
            lcOneHourElectricity.KiloWatt = 200;
            lcOneHourElectricity.PanelId = 2;
            lcOneHourElectricity.DateTime = new System.DateTime(2018, 8, 24);
            list.Add(lcOneHourElectricity);

            lcOneHourElectricity = new OneHourElectricity();
            lcOneHourElectricity.Id = 3;
            lcOneHourElectricity.KiloWatt = 200;
            lcOneHourElectricity.PanelId = 2;
            lcOneHourElectricity.DateTime = new System.DateTime(2018, 8, 23);
            list.Add(lcOneHourElectricity);

            lcOneDayElectricityModel = new OneDayElectricityModel();
            lcOneDayElectricityModel.DateTime = new System.DateTime(2018, 8, 24);
            lcOneDayElectricityModel.Sum = 300;
            lcOneDayElectricityModel.Average = 150;
            lcOneDayElectricityModel.Minimum = 100;
            lcOneDayElectricityModel.Maximum = 200;
            listMust.Add(lcOneDayElectricityModel);


            lcOneDayElectricityModel = new OneDayElectricityModel();
            lcOneDayElectricityModel.DateTime = new System.DateTime(2018, 8, 23);
            lcOneDayElectricityModel.Sum = 200;
            lcOneDayElectricityModel.Average = 200;
            lcOneDayElectricityModel.Minimum = 200;
            lcOneDayElectricityModel.Maximum = 200;
            listMust.Add(lcOneDayElectricityModel);

            var result =  _analyticsController.Calculate(list);
            // Assert
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(result[i].Average, listMust[i].Average);
                Assert.Equal(result[i].Maximum, listMust[i].Maximum);
                Assert.Equal(result[i].Minimum, listMust[i].Minimum);
                Assert.Equal(result[i].Sum, listMust[i].Sum);
                Assert.Equal(result[i].DateTime, listMust[i].DateTime);
            }


        }

    }
}