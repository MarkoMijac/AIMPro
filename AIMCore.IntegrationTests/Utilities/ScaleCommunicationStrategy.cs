using System;
using AIMCore.Communication;
using AIMCore.Exceptions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;

namespace AIMCore.IntegrationTests.Utilities;

public class ScaleCommunicationStrategy : CommunicationStrategy<string>
{
    public ScaleCommunicationStrategy()
    {
        Name = "ScaleCommunicationStrategy";
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

    private static double GenerateRandomWeight()
    {
        var random = new Random();
        return Math.Round(random.Next(19, 21) + random.NextDouble(), 1);
    }

    public override string Execute(string command)
    {
        if(IsConnected)
        {
            if(command != "GET_WEIGHT")
            {
                throw new AIMException("Invalid command");
            }

            return DateTime.Now.ToString() + ";" + GenerateRandomWeight().ToString();
        }
        else
        {
            throw new AIMException("Scale is not connected");
        }
    }

    public override async Task<string> ExecuteAsync(string command)
    {
        return await Task.Run(() => Execute(command));
    }
}
