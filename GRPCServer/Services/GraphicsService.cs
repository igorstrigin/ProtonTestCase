using Grpc.Core;
using GRPCServer.Protos;
using GRPCServer.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GRPCServer
{
    public class GraphicsService : Graphics.GraphicsBase
    {
        private readonly ILogger<GraphicsService> logger;
        private readonly IGraphicGenerator service;

        public GraphicsService(ILogger<GraphicsService> logger, IGraphicGenerator service)
        {
            this.logger = logger;
            this.service = service;
        }


        //public override Task<PointsArray> GetRandomGraphic(PointsCount pointsCount, ServerCallContext context) 
        //{
        //    PointsArray result = new PointsArray()

        //    return Task.FromResult( new PointsArray() { GraphicPoints = service.GenerateRandomGraphic(pointsCount.PointsCount_) } ;
        //} 
        //public override Task<PointsArray> GetCustomGraphic(PointsArray pointsArray, ServerCallContext context) 
        //{ 

        //} 
        //public override Task<PointsArray> GetGraphicFromFile(GraphicNumber graphicNumber, ServerCallContext context) 
        //{ 

        //}
    }
}
