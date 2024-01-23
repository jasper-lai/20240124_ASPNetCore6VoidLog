using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASPNetCore6VoidLog.Services;
using ASPNetCore6VoidLog.ViewModels;
using ExpectedObjects;
using ASPNetCore6VoidLog.Wrapper;
using Moq;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.Logging;

namespace ASPNetCore6VoidLog.Services.Tests
{
    [TestClass()]
    public class LottoServiceTests
    {
        [TestMethod()]
        public void Test_Lottoing_今天是20240105_主辦人宣告開始_輸入亂數範圍_0_10_預期回傳_9_恭喜中獎()
        {
            // Arrange
            var expected = new LottoViewModel()
            { Sponsor = "傑士伯", YourNumber = 9, Message = "恭喜中獎" }
                        .ToExpectedObject();

            int fixedValue = 9;
            DateTime today = new(2024, 01, 05);
            var mockRandomGenerator = new Mock<IRandomGenerator>();
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockRandomGenerator.Setup(r => r.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(fixedValue);
            mockDateTimeProvider.Setup(d => d.GetCurrentTime()).Returns(today);
            // [檔案系統]
            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    { @"Extras/startup.txt", new MockFileData("傑士伯") },
                }
            );
            // [Logger]
            var mockLogger = new Mock<ILogger<LottoService>>();

            // Act
            var target = new LottoService(mockRandomGenerator.Object, mockDateTimeProvider.Object, mockFileSystem, mockLogger.Object);
            var actual = target.Lottoing(0, 10);

            // Assert
            //---------------
            //LogInformation() 是擴充方法, 不能直接 Verify ! 
            //---------------
            //mockLogger.Verify(x => x.LogInformation(It.IsAny<string>()), Times.Once());

            //---------------
            //CS1593 delegate 'Func<Func<It.IsAnyType, Exception, string>, bool>' does not accept 3 arguments 
            //---------------
            //mockLogger.Verify(
            //    x => x.Log(
            //        LogLevel.Information, // Match the log level
            //        It.IsAny<EventId>(), // Use It.IsAny for EventId
            //        It.Is<It.IsAnyType>((v, t) => true), // Match any log message
            //        It.IsAny<Exception>(), // Use It.IsAny for Exception
            //        It.Is<Func<It.IsAnyType, Exception, string>>((v, e, f) => true)), // Match any Func delegate
            //    Times.Once);

            //---------------
            //CS8620 因為參考型別的可 NULL 性有所差異，所以無法針對 'void ILogger.Log<IsAnyType>(LogLevel logLevel, EventId eventId, IsAnyType state, Exception? exception, Func<IsAnyType, Exception?, string> formatter)' 內類型 'Func<It.IsAnyType, Exception?, string>' 的參數 'formatter' 使用類型 'Func<It.IsAnyType, Exception, string>' 的引數。	
            //---------------
            //mockLogger.Verify(
            //    x => x.Log(
            //        LogLevel.Information, // Match the log level
            //        It.IsAny<EventId>(), // Use It.IsAny for EventId
            //        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("your expected log message")), // Match log message content
            //        It.IsAny<Exception>(), // Use It.IsAny for Exception
            //        It.IsAny<Func<It.IsAnyType, Exception, string>>() // Use It.IsAny for the message formatter Func
            //    ),
            //    Times.Once);

            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information, // Match the log level
                    It.IsAny<EventId>(), // Use It.IsAny for EventId
                    It.Is<It.IsAnyType>((v, t) => true), // Match any log message
                    It.IsAny<Exception?>(), // Use It.IsAny for Exception (nullable)
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>() // Use It.IsAny for the message formatter Func (nullable)
                ),
                Times.Once);

            expected.ShouldEqual(actual);
        }


        [TestMethod()]
        public void Test_Lottoing_今天是20240105_主辦人宣告開始_輸入亂數範圍_0_10_預期回傳_1_再接再厲()
        {
            // Arrange
            var expected = new LottoViewModel()
            { Sponsor="傑士伯", YourNumber = 1, Message = "再接再厲" }
                        .ToExpectedObject();

            int fixedValue = 1;
            DateTime today = new(2024, 01, 05);
            var mockRandomGenerator = new Mock<IRandomGenerator>();
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockRandomGenerator.Setup(r => r.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(fixedValue);
            mockDateTimeProvider.Setup(d => d.GetCurrentTime()).Returns(today);
            // [檔案系統]
            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    { @"Extras/startup.txt", new MockFileData("傑士伯") },
                }
            );
            // [Logger]
            var mockLogger = new Mock<ILogger<LottoService>>();

            // Act
            var target = new LottoService(mockRandomGenerator.Object, mockDateTimeProvider.Object, mockFileSystem, mockLogger.Object);
            var actual = target.Lottoing(0, 10);

            // Assert
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information, // Match the log level
                    It.IsAny<EventId>(), // Use It.IsAny for EventId
                    It.Is<It.IsAnyType>((v, t) => true), // Match any log message
                    It.IsAny<Exception?>(), // Use It.IsAny for Exception (nullable)
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>() // Use It.IsAny for the message formatter Func (nullable)
                ),
                Times.Once);
            expected.ShouldEqual(actual);
        }


        [TestMethod()]
        public void Test_Lottoing_今天是20240122_不論主辦人是否宣告開始_輸入亂數範圍_0_10_預期回傳_負1_非每個月5日_不開獎()
        {
            // Arrange
            var expected = new LottoViewModel()
            { Sponsor = "", YourNumber = -1, Message = "非每個月5日, 不開獎" }
                        .ToExpectedObject();

            int fixedValue = 9;
            DateTime today = new(2024, 01, 22);
            var mockRandomGenerator = new Mock<IRandomGenerator>();
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockRandomGenerator.Setup(r => r.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(fixedValue);
            mockDateTimeProvider.Setup(d => d.GetCurrentTime()).Returns(today);
            // [檔案系統]
            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    { @"Extras/startup.txt", new MockFileData("傑士伯") },
                }
            );
            // [Logger]
            var mockLogger = new Mock<ILogger<LottoService>>();

            // Act
            var target = new LottoService(mockRandomGenerator.Object, mockDateTimeProvider.Object, mockFileSystem, mockLogger.Object);
            var actual = target.Lottoing(0, 10);

            // Assert
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Critical, // Match the log level
                    It.IsAny<EventId>(), // Use It.IsAny for EventId
                    It.Is<It.IsAnyType>((v, t) => true), // Match any log message
                    It.IsAny<Exception?>(), // Use It.IsAny for Exception (nullable)
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>() // Use It.IsAny for the message formatter Func (nullable)
                ),
                Times.Once);
            expected.ShouldEqual(actual);
        }

        [TestMethod()]
        public void Test_Lottoing_今天是20240105_但主辦人常未宣告開始_輸入亂數範圍_0_10_預期回傳_負2_主辦人員尚未按下開始按鈕()
        {
            // Arrange
            var expected = new LottoViewModel()
            { Sponsor = "", YourNumber = -2, Message = "主辦人員尚未按下[開始]按鈕" }
                        .ToExpectedObject();

            int fixedValue = 1;
            DateTime today = new(2024, 01, 05);
            var mockRandomGenerator = new Mock<IRandomGenerator>();
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockRandomGenerator.Setup(r => r.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(fixedValue);
            mockDateTimeProvider.Setup(d => d.GetCurrentTime()).Returns(today);
            // [檔案系統]
            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    //只要不提供檔案路徑, 就會視為 FileNotFound Exception
                    //{ @"startup.txt", new MockFileData("傑士伯") },
                }
            );
            // [Logger]
            var mockLogger = new Mock<ILogger<LottoService>>();

            // Act
            var target = new LottoService(mockRandomGenerator.Object, mockDateTimeProvider.Object, mockFileSystem, mockLogger.Object);
            var actual = target.Lottoing(0, 10);

            // Assert
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error, // Match the log level
                    It.IsAny<EventId>(), // Use It.IsAny for EventId
                    It.Is<It.IsAnyType>((v, t) => true), // Match any log message
                    It.IsAny<Exception?>(), // Use It.IsAny for Exception (nullable)
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>() // Use It.IsAny for the message formatter Func (nullable)
                ),
                Times.Once);
            expected.ShouldEqual(actual);
        }

    }
}