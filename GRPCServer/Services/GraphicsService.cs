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
            if (pointsCount.PointsCount_ == 0)
            {
                logger.LogError("Был получен график с 0 точками");
                throw new ArgumentNullException("Был получен график с 0 точками");
            }

            if (pointsCount.PointsCount_ == 1)
            {
                logger.LogError("График не может состоять из 1 точки");
                throw new Exception("График не может состоять из 1 точки");
            }

            PointsArray result = new PointsArray();

            try
            {
                double[] line = await service.GenerateRandomGraphic(pointsCount.PointsCount_);

                result.GraphicPoints.AddRange(line);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message);
                throw new Exception(ex.Message);
            }
        } 

        public override async Task<PointsArray> GetCustomGraphic(PointsArray pointsArray, ServerCallContext context) 
        {
            if (pointsArray.GraphicPoints.Count == 0) 
            {
                logger.LogError("Был получен пустой массив");
                throw new ArgumentNullException("Был получен пустой массив");
            }

            if (pointsArray.GraphicPoints.Count == 1)
            {
                logger.LogError("График не может состоять из 1 точки");
                throw new Exception("График не может состоять из 1 точки");
            }

            try
            {
                PointsArray result = new PointsArray();

                double[] line = await service.GenerateCustomGraphic(pointsArray.GraphicPoints.ToArray());

                result.GraphicPoints.AddRange(line);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        } 


        public override async Task<GraphicsArray> GetGraphicsFromFile(EmptyMessage a, ServerCallContext context) 
        {
            try
            {
                GraphicsArray result = new GraphicsArray();

                List<List<double>> lines = await service.GetGraphicsFromFile();

                foreach (var line in lines)
                {
                    PointsArray newLine = new PointsArray();
                    newLine.GraphicPoints.Add(line.ToArray());
                    result.Lines.Add(newLine);
                }


                return result;
            }
            catch (Exception ex) 
            {
                logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
