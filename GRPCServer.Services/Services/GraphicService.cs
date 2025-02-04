﻿using GRPCServer.Services.Interfaces;
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

        string path = Directory.GetCurrentDirectory() + "\\GraphicPoints.txt";

        public async Task<double[]> GenerateCustomGraphic(params double[] pointsArray)
        {
            if (pointsArray.Length == 0)
                throw new ArgumentNullException("Нельзя сгенерировать график без точек на графике");

            if (pointsArray.Length == 1)
                throw new Exception("График должен состоять минимум из 2 точек");

            

            string result = String.Join(';', pointsArray) + Environment.NewLine;

            await File.AppendAllTextAsync(path, result);

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

            

            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate)) 
            {
                if (fileStream.Length == 0)
                    throw new ArgumentNullException("Файл с исходными данными пустой");

                byte[] byteText = new byte[fileStream.Length];


                await fileStream.ReadAsync(byteText,0,byteText.Length);

                string textFromFile = Encoding.Default.GetString(byteText);

                textFromFile = textFromFile.TrimStart();

                string[] Graphics = textFromFile.Split(Environment.NewLine);
                

                foreach (var line in Graphics)
                {
                    string[] pointsText = line.Split(';');
                    List<double> points = new List<double>();

                    //проверка на парсинг нулевой строки
                    if (pointsText.Length == 1 && pointsText[0] == "")
                        break;

                    try
                    {
                        foreach (var point in pointsText)
                            points.Add(Convert.ToDouble(point));
                    }
                    catch (FormatException ex) 
                    {
                        throw new FormatException("В файле содержатся недопустимые символы", ex);
                    }

                    lines.Add(points);
                }

                

                
            }

            return lines;

        }
    }
}
