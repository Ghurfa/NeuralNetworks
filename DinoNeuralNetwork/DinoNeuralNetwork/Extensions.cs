using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetworkLibrary
{
    static class Extensions
    {
        public static double NextDouble(this Random source, double min, double max)
        {
            return source.NextDouble() * (max - min) + min;
        }
    }
}
