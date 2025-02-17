using System;
using AIMCore.Communication;
using AIMCore.Exceptions;

namespace AIMCore.IntegrationTests.Utilities;

public class VibrationCommunicationStrategy : CommunicationStrategy<string>
{
    public VibrationCommunicationStrategy()
    {
        Name = "VibrationCommunicationStrategy";
        IsConnected = false;
    }

    public override void Connect()
    {
        IsConnected = true;
    }

    public override Task ConnectAsync()
    {
        IsConnected = true;
        return Task.CompletedTask;
    }

    public override void Disconnect()
    {
        IsConnected = false;
    }

    public override Task DisconnectAsync()
    {
        IsConnected = false;
        return Task.CompletedTask;
    }

    public override string Receive()
    {
        if(IsConnected)
        {
            return DateTime.Now.ToString() + ";" + GenerateRandomValue().ToString();
        }
        else
        {
            throw new AIMException("Vibration sensor is not connected");
        }
    }

    private static double GenerateRandomValue()
    {
        var random = new Random();
        return random.Next(0, 1) + random.NextDouble();
    }

    public override async Task<string> ReceiveAsync()
    {
        return await Task.Run(() => Receive());
    }

    public override void Send(string command)
    {
        if(IsConnected)
        {
            if(command != "GET_VIBR")
            {
                throw new AIMException("Invalid command");
            }
        }
        else
        {
            throw new AIMException("Vibration sensor is not connected");
        }
    }

    public override Task SendAsync(string command)
    {
        return Task.Run(() => Send(command));
    }

    public override string Execute(string command)
    {
        if(IsConnected)
        {
            if(command != "GET_VIBR")
            {
                throw new AIMException("Invalid command");
            }

            return DateTime.Now.ToString() + ";" + GenerateRandomValue().ToString();
        }
        else
        {
            throw new AIMException("Vibration sensor is not connected");
        }
    }

    public override async Task<string> ExecuteAsync(string command)
    {
        return await Task.Run(() => Execute(command));
    }
}
