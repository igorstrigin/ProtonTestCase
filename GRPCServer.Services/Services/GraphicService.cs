using GRPCServer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRPCServer;

namespace GRPCServer.Services.Services
{
    public class GraphicService : IGraphicGenerator
    {
        public async Task<double[]> GenerateCustomGraphic(params double[] pointsArray)
        {
            if (pointsArray.Length == 0)
                throw new ArgumentNullException("Нельзя сгенерировать график без точек на графике");

            if (pointsArray.Length == 1)
                throw new Exception("График должен состоять минимум из 2 точек");

            string path = Environment.CurrentDirectory + "GraphicPoints.txt";

            string result = String.Join(',', pointsArray) + ";";

            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                byte[] resultByte = new byte[result.Length];
                await fileStream.WriteAsync(Encoding.Default.GetBytes(result));
            }

            return pointsArray;
        }

        public Task<double[]> GenerateRandomGraphic(int pointsCount)
        {
            if (pointsCount == 0)
                throw new ArgumentNullException("Нельзя сгенерировать график без точек на графике");

            if (pointsCount == 1)
                throw new Exception("График должен состоять минимум из 2 точек");

            Random generator = new Random();

            double[] result = new double[pointsCount];

            for (int point = 0; point < pointsCount; point++)
                result[point] = generator.NextDouble();

            return Task.FromResult(result);
        }

        public async Task<List<List<double>>> GetGraphicsFromFile()
        {
            List<List<double>> lines = new List<List<double>>();

            string path = Directory.GetCurrentDirectory() + "\\GraphicPoints.txt";

            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate)) 
            {
                byte[] byteText = new byte[fileStream.Length];

                await fileStream.ReadAsync(byteText,0,byteText.Length);

                string textFromFile = Encoding.Default.GetString(byteText);

                string[] Graphics = textFromFile.Split(';');

                

                foreach (var line in Graphics)
                {
                    string[] pointsText = line.Split(',');
                    List<double> points = new List<double>();

                    //проверка на парсинг нулевой строки
                    if (pointsText.Length == 1 && pointsText[0] == "")
                        break;

                    foreach (var point in pointsText)
                        points.Add(Convert.ToDouble(point));

                    lines.Add(points);
                }

                

                
            }

            return lines;

        }
    }
}
