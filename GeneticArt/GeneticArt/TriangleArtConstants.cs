using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticArt
{
    internal static class TriangleArtConstants
    {
        public static double MutatePointProbability = 0.5;
        public static double MutateColorAProbability = 0.2;
        public static double MutateColorRGBProbability = 0.3;

        public static double MutatePointAmount = 0.01;
        public static double MutateColorAAmount = 0.1;
        public static double MutateColorRGBAmount = 0.1;

        public static double ColorAMin = 0.01;
        public static double ColorAMax = 1;
    }
}
