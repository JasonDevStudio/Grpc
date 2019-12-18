using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcProtos;
using System.Linq;
using System.Threading;

namespace GrpcService.Services
{
    public class GrpcTestService : GrpcProtos.GrpcTService.GrpcTServiceBase
    {
        public override Task GetGrpcTester(HelloRequest request, IServerStreamWriter<TestEntity> responseStream, ServerCallContext context)
        { 
            var cont = Guid.NewGuid().ToString();
            var header = Guid.NewGuid().ToString();

            var enttiy = new TestEntity
            {
                Context = $"{cont}",
                Hearder = $"{header}"
            };

            return responseStream.WriteAsync(enttiy);
        }

        public override Task GetGrpcTestEntities(HelloRequest request, IServerStreamWriter<TestEntities> responseStream, ServerCallContext context)
        {
            return Task.Run(async () =>
            {
                //responseStream.WriteOptions = new WriteOptions(WriteFlags.BufferHint);                
                var cont = Guid.NewGuid().ToString();
                var header = Guid.NewGuid().ToString();

                for (int i = 0; i < 100; i++)
                {
                    var tmpEntities = new List<TestEntity>();

                    for (int j = i + 1; j < 10000000; j++)
                    {
                        var enttiy = new TestEntity
                        {
                            Context = $"{cont}{j}",
                            Hearder = $"{header}{j}"
                        };

                        tmpEntities.Add(enttiy);
                    }

                    var entities = new TestEntities();
                    entities.Context.AddRange(tmpEntities); 
                    await responseStream.WriteAsync(entities);
                }
            }); 
        } 
    }
}
