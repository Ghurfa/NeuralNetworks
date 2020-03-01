using System;

namespace Perceptron
{
    class Perceptron
    {
        public double[] Weights;
        public double BiasWeight;
        public Perceptron(int numOfWeights)
        {
            Weights = new double[numOfWeights];
        }
        public double Compute(double[] inputs)
        {
            double sum = 0;
            for (int i = 0; i < Weights.Length; i++)
            {
                sum += Weights[i] * inputs[i];
            }
            sum += BiasWeight;
            return sum > 0 ? 1 : 0;
        }
        public void Train(double[][] inputs, double[] desiredOutputs)
        {
            double MAE;
            double[] outputs = new double[inputs.Length];
            do
            {
                for (int i = 0; i < inputs.Length; i++)
                {
                    double output = Compute(inputs[i]);
                    double error = desiredOutputs[i] - output;
                    for (int j = 0; j < Weights.Length; j++)
                    {
                        Weights[j] += error * inputs[i][j];
                    }
                    BiasWeight += error;
                }
                for (int i = 0; i < inputs.Length; i++)
                {
                    outputs[i] = Compute(inputs[i]);
                }
                MAE = calcMAE(outputs, desiredOutputs);
            } while (MAE > 0.0);
        }
        public bool Check(double[][] inputs, double[] desiredOutputs)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                double output = Compute(inputs[i]);
                if(output != desiredOutputs[i])
                {
                    return false;
                }
            }
            return true;
        }
        private double calcMAE(double[] input, double[] goal)
        {
            double total = 0;
            for (int i = 0; i < input.Length; i++)
            {
                total += Math.Abs(input[i] - goal[i]);
            }
            return total / input.Length;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            double[][] inputs =
                {
                new double[]{ 0, 0 },
                new double[]{ 0, 1 },
                new double[]{ 1, 0 },
                new double[]{ 1, 1 }
                };

            Perceptron andPerceptron = new Perceptron(2);
            double[] andDesiredOutputs = new double[] { 0, 0, 0, 1 };
            andPerceptron.Train(inputs, andDesiredOutputs);
            Console.Write($"And weights: {andPerceptron.Weights[0]}, {andPerceptron.Weights[1]}, B: {andPerceptron.BiasWeight} ");
            Console.WriteLine(andPerceptron.Check(inputs, andDesiredOutputs));

            Perceptron orPerceptron = new Perceptron(2);
            double[] orDesiredOutputs = new double[] { 0, 1, 1, 1 };
            orPerceptron.Train(inputs, orDesiredOutputs);
            Console.Write($"Or weights: {orPerceptron.Weights[0]}, {orPerceptron.Weights[1]}, B: {orPerceptron.BiasWeight} ");
            Console.WriteLine(orPerceptron.Check(inputs, orDesiredOutputs));

            Perceptron nandPerceptron = new Perceptron(2);
            double[] nandDesiredOutputs = new double[] { 1, 1, 1, 0 };
            nandPerceptron.Train(inputs, nandDesiredOutputs);
            Console.Write($"Nand weights: {nandPerceptron.Weights[0]}, {nandPerceptron.Weights[1]}, B: {nandPerceptron.BiasWeight} ");
            Console.WriteLine(nandPerceptron.Check(inputs, nandDesiredOutputs));

            Perceptron norPerceptron = new Perceptron(2);
            double[] norDesiredOutputs = new double[] { 1, 0, 0, 0 };
            norPerceptron.Train(inputs, norDesiredOutputs);
            Console.Write($"Nor weights: {norPerceptron.Weights[0]}, {norPerceptron.Weights[1]}, B: {norPerceptron.BiasWeight} ");
            Console.WriteLine(norPerceptron.Check(inputs, norDesiredOutputs));

            Console.ReadKey();
        }
    }
}
