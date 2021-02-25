using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GRPCServer.Services.Interfaces
{
    public interface IGraphicGenerator
    {
        Task<double[]> GenerateRandomGraphic(int pointsCount);
        Task<double[]> GenerateCustomGraphic(params double[] pointsArray);
        Task<double[]> GetGraphicFromFile(int graphicNumber);
    }
}
