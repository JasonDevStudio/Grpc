using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcProtos;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Thread.Sleep(3000);
            var channelOptions = new List<ChannelOption>();

            // add max message length option 设最大接收传输大小
            channelOptions.Add(new ChannelOption(ChannelOptions.MaxReceiveMessageLength, int.MaxValue));
            channelOptions.Add(new ChannelOption(ChannelOptions.MaxSendMessageLength, int.MaxValue));
            channelOptions.Add(new ChannelOption(ChannelOptions.MaxConcurrentStreams, int.MaxValue));

            var channel = new Channel("localhost", 11180, ChannelCredentials.Insecure, channelOptions);
            var client = new GrpcTService.GrpcTServiceClient(channel);
            var result = client.GetGrpcTestEntities(new HelloRequest { });
            var stream = result.ResponseStream;
            var i = 1;
            while (await stream.MoveNext())
            {
                Console.WriteLine($"[{i}] 读取数据量 ： {stream.Current.Context.Count}");
                stream.Current.Context.Clear();
                i++;
            }

            Console.ReadLine();
        }
    }
}
