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

    public override string Receive()
    {
        if(IsConnected)
        {
            return DateTime.Now.ToString() + ";" + GenerateRandomWeight().ToString();
        }
        else
        {
            throw new AIMException("Scale is not connected");
        }
    }

    private static double GenerateRandomWeight()
    {
        var random = new Random();
        return Math.Round(random.Next(19, 21) + random.NextDouble(), 1);
    }

    public override async Task<string> ReceiveAsync()
    {
        return await Task.Run(() => Receive());
    }

    public override void Send(string command)
    {
        if(IsConnected)
        {
            if(command != "GET_WEIGHT")
            {
                throw new AIMException("Invalid command");
            }
        }
        else
        {
            throw new AIMException("Scale is not connected");
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
