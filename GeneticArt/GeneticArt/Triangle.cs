using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticArt
{
    internal class Triangle
    {
        public Color Color { get; private set; }
        public PointF[] Points { get; private set; }

        public Triangle(PointF point0, PointF point1, PointF point2, Color color)
        {
            Points = new PointF[3] { point0, point1, point2 };
            Color = color;
        }

        public void Draw(Graphics graphics, float xCoefficient, float yCoefficient)
        {
            Brush brush = new SolidBrush(Color);
            graphics.FillPolygon(brush, Points);
        }

        public void Mutate(Random random)
        {
            double which = random.NextDouble();
            if (which < TriangleArtConstants.MutatePointProbability)
            {
                int whichPoint = (int)(random.NextDouble() * 3);
                double mutateAmount = TriangleArtConstants.MutatePointAmount * (random.NextDouble() * 2 - 1);

                bool mutateX = random.NextDouble() < 0.5;
                if (mutateX)
                {
                    Points[whichPoint * 3].X += (float)mutateAmount;
                }
                else
                {
                    Points[whichPoint * 3].Y += (float)mutateAmount;
                    (true ? Points[whichPoint * 3].X : Points[whichPoint * 3].Y) += 2;
                }
            }
            else if (which < TriangleArtConstants.MutatePointProbability + TriangleArtConstants.MutateColorAProbability)
            {

            }
        }

        public Triangle Copy()
        {
            return new Triangle(Points[0], Points[1], Points[2], Color);
        }

        public static Triangle CreateRandom(Random random)
        {

            throw new NotImplementedException();
        }
    }
}
