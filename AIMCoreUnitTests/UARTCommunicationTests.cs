//Generate xunit tests using FakeItEasy library for UARTCommunication class
using AIMCore.Communication;
using AIMCore.Exceptions;
using FakeItEasy;

namespace AIMCoreUnitTests
{
    public class UARTCommunicationTests
    {
        private ISerialPort _serialPort;
        private UARTCommunication _uartCommunication;

        public UARTCommunicationTests()
        {
            _serialPort = A.Fake<ISerialPort>();
            _uartCommunication = new UARTCommunication(_serialPort);
        }

        [Fact]
        public void Connect_GivenNotConnected_InvokesPortOpen()
        {
            //Arrange
            A.CallTo(() => _serialPort.IsOpen).Returns(false);

            //Act
            _uartCommunication.Connect();

            //Assert
            A.CallTo(() => _serialPort.Open()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void Connect_GivenAlreadyConnected_DoesNotInvokePortOpen()
        {
            //Arrange
            A.CallTo(() => _serialPort.IsOpen).Returns(true);

            //Act
            _uartCommunication.Connect();

            //Assert
            A.CallTo(() => _serialPort.Open()).MustNotHaveHappened();
        }

        [Fact]
        public void IsConnected_GivenPortIsOpen_ReturnsTrue()
        {
            //Arrange
            A.CallTo(() => _serialPort.IsOpen).Returns(true);

            //Act
            var result = _uartCommunication.IsConnected;

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void IsConnected_GivenPortIsNotOpen_ReturnsFalse()
        {
            //Arrange
            A.CallTo(() => _serialPort.IsOpen).Returns(false);

            //Act
            var result = _uartCommunication.IsConnected;

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Disconnect_GivenNotConnected_DoesNotInvokePortClose()
        {
            //Arrange
            A.CallTo(() => _serialPort.IsOpen).Returns(false);

            //Act
            _uartCommunication.Disconnect();
            
            //Assert
            A.CallTo(() => _serialPort.Close()).MustNotHaveHappened();
        }

        [Fact]
        public void Disconnect_GivenConnected_InvokesPortClose()
        {
            //Arrange
            A.CallTo(() => _serialPort.IsOpen).Returns(true);

            //Act
            _uartCommunication.Disconnect();
            
            //Assert
            A.CallTo(() => _serialPort.Close()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ConnectAsync_GivenNotConnected_InvokesPortOpen()
        {
            // Arrange
            A.CallTo(() => _serialPort.IsOpen).Returns(false);

            // Act
            await _uartCommunication.ConnectAsync();

            // Assert
            A.CallTo(() => _serialPort.Open()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ConnectAsync_GivenAlreadyConnected_InvokesPortOpen()
        {
            // Arrange
            A.CallTo(() => _serialPort.IsOpen).Returns(true);

            // Act
            await _uartCommunication.ConnectAsync();

            // Assert
            A.CallTo(() => _serialPort.Open()).MustNotHaveHappened();
        }

        [Fact]
        public async Task DisconnectAsync_GivenNotConnected_DoesNotInvokePortClose()
        {
            //Arrange
            A.CallTo(() => _serialPort.IsOpen).Returns(false);

            //Act
            await _uartCommunication.DisconnectAsync();
            
            //Assert
            A.CallTo(() => _serialPort.Close()).MustNotHaveHappened();
        }

        [Fact]
        public async Task DisconnectAsync_GivenConnected_InvokesPortClose()
        {
            //Arrange
            A.CallTo(() => _serialPort.IsOpen).Returns(true);

            //Act
            await _uartCommunication.DisconnectAsync();
            
            //Assert
            A.CallTo(() => _serialPort.Close()).MustHaveHappenedOnceExactly();
        }
    }
}