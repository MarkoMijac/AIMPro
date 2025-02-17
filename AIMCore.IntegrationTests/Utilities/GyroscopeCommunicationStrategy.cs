using System;
using AIMCore.Communication;
using AIMCore.Exceptions;

namespace AIMCore.IntegrationTests.Utilities;

public class GyroscopeCommunicationStrategy : CommunicationStrategy<string>
{
    public GyroscopeCommunicationStrategy()
    {
        Name = "GyroscopeCommunicationStrategy";
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

    private static double GenerateRandomValue()
    {
        var random = new Random();
        return random.Next(0, 30);
    }

    public override string Execute(string command)
    {
        if(IsConnected)
        {
            if(command != "GET_INCLINE")
            {
                throw new AIMException("Invalid command");
            }

            return DateTime.Now.ToString() + ";" + GenerateRandomValue().ToString();
        }
        else
        {
            throw new AIMException("Gyroscope is not connected");
        }
    }

    public override async Task<string> ExecuteAsync(string command)
    {
        return await Task.Run(() => Execute(command));
    }
}
