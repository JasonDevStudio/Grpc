using System;
using System.Collections.Generic;
using Grpc.Core;
using GrpcProtos;
using GrpcService.Services;

namespace GrpcService
{
    class Program
    {
        static void Main(string[] args)
        {
            var channelOptions = new List<ChannelOption>();

            // add max message length option 设最大接收传输大小
            channelOptions.Add(new ChannelOption(ChannelOptions.MaxReceiveMessageLength, int.MaxValue)); 
            channelOptions.Add(new ChannelOption(ChannelOptions.MaxSendMessageLength, int.MaxValue)); 
            channelOptions.Add(new ChannelOption(ChannelOptions.MaxConcurrentStreams, int.MaxValue));

            var server = new Server(channelOptions)
            {
                Services = { GrpcProtos.GrpcTService.BindService(new GrpcTestService()) },
                Ports = { new ServerPort("localhost", 11180, ServerCredentials.Insecure) }
            };
             
            server.Start();

            Console.WriteLine("GrpcTService started...");
            Console.ReadLine();

            server.ShutdownAsync();
        }
    }
}
