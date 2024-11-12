//Generate xunit tests using FakeItEasy library for UARTCommunication class
using AIMCore;
using AIMCore.Communication;
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


        [Fact]
        public void Send_GivenPortIsOpen_InvokesWriteLine()
        {
            //Arrange
            A.CallTo(() => _serialPort.IsOpen).Returns(true);

            //Act
            _uartCommunication.Send("test");

            //Assert
            A.CallTo(() => _serialPort.WriteLine("test")).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void Send_GivenPortIsNotOpen_ThrowsException()
        {
            //Arrange
            A.CallTo(() => _serialPort.IsOpen).Returns(false);

            //Act
            Action act = () => _uartCommunication.Send("test");

            //Assert
            Assert.Throws<AIMException>(act);
        }

        [Fact]
        public async Task SendAsync_GivenPortIsOpen_InvokesWriteLine()
        {
            //Arrange
            A.CallTo(() => _serialPort.IsOpen).Returns(true);

            //Act
            await _uartCommunication.SendAsync("test");

            //Assert
            A.CallTo(() => _serialPort.WriteLine("test")).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task SendAsync_GivenPortIsNotOpen_ThrowsException()
        {
            //Arrange
            A.CallTo(() => _serialPort.IsOpen).Returns(false);

            //Act
            Func<Task> act = async () => await _uartCommunication.SendAsync("test");

            //Assert
            await Assert.ThrowsAsync<AIMException>(act);
        }

        [Fact]
        public void Receive_GivenPortIsOpen_InvokesReadLine()
        {
            //Arrange
            A.CallTo(() => _serialPort.IsOpen).Returns(true);

            //Act
            _uartCommunication.Receive();

            //Assert
            A.CallTo(() => _serialPort.ReadLine()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void Receive_GivenPortIsNotOpen_ThrowsException()
        {
            //Arrange
            A.CallTo(() => _serialPort.IsOpen).Returns(false);

            //Act
            Action act = () => _uartCommunication.Receive();

            //Assert
            Assert.Throws<AIMException>(act);
        }

        [Fact]
        public async Task ReceiveAsync_GivenPortIsOpen_InvokesReadLine()
        {
            //Arrange
            A.CallTo(() => _serialPort.IsOpen).Returns(true);

            //Act
            await _uartCommunication.ReceiveAsync();

            //Assert
            A.CallTo(() => _serialPort.ReadLine()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ReceiveAsync_GivenPortIsNotOpen_ThrowsException()
        {
            //Arrange
            A.CallTo(() => _serialPort.IsOpen).Returns(false);

            //Act
            Func<Task> act = async () => await _uartCommunication.ReceiveAsync();

            //Assert
            await Assert.ThrowsAsync<AIMException>(act);
        }
    }
}