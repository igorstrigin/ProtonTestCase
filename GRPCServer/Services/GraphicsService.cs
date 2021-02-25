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


        public override async Task<PointsArray> GetRandomGraphic(PointsCount pointsCount, ServerCallContext context) 
        {
            PointsArray result = new PointsArray();

            double[] line = await service.GenerateRandomGraphic(pointsCount.PointsCount_);

            result.GraphicPoints.AddRange(line);

            return result;
        } 
        public override async Task<PointsArray> GetCustomGraphic(PointsArray pointsArray, ServerCallContext context) 
        {
            PointsArray result = new PointsArray();

            double[] line = await service.GenerateCustomGraphic(pointsArray.GraphicPoints.ToArray());

            result.GraphicPoints.AddRange(line);

            return result;
        } 


        public override async Task<PointsArray> GetGraphicFromFile(GraphicNumber graphicNumber, ServerCallContext context) 
        {
            PointsArray result = new PointsArray();

            double[] line = await service.GetGraphicFromFile(graphicNumber.GraphicNumber_);

            result.GraphicPoints.AddRange(line);

            return result;
        }
    }
}
