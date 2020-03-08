using NeuralNetworkLibrary;
using System;

namespace GradientDescent
{
    class Program
    {
        static bool CheckNetwork(Network network, double[][] inputs, double[][] desiredOutputs)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                if (network.MSE(inputs[i], desiredOutputs[i]) > 0.01)
                {
                    return false;
                }
            }
            return true;
        }
        static void xorTrain()
        {
            Network network = new Network(ActivationType.Sigmoid, 2, 2, 1);
            double[][] inputs =
            {
                new double[] { 0, 0 },
                new double[] { 0, 1 },
                new double[] { 1, 0 },
                new double[] { 1, 1 }
            };
            double[][] xorOutputs =
            {
                new double[] { 0 },
                new double[] { 1 },
                new double[] { 1 },
                new double[] { 0 }
            };
            while (true)
            {
                network.Randomize(new Random());
                GradientDescentTeacher teacher = new GradientDescentTeacher(network);
                int numOfGens = teacher.TrainNetwork(inputs, xorOutputs, 0.01);
                Console.WriteLine($"Num of generations: {numOfGens}");
                network.PrintWeights();
                if (CheckNetwork(network, inputs, xorOutputs))
                {
                    Console.WriteLine("XOR pass");
                }
                else
                {
                    Console.WriteLine("XOR fail");
                }
                Console.ReadKey();
                Console.Clear();
            }
        }
        static void sinTrain()
        {
            Network network = new Network(ActivationType.TanH, 1, 7, 1);
            double[][] inputs = new double[64][];
            double[][] outputs = new double[64][];
            for (int i = 0; i < 64; i++)
            {
                inputs[i] = new double[] { (double)i / 64 * Math.PI * 2 };
            }
            for (int i = 0; i < 64; i++)
            {
                outputs[i] = new double[] { Math.Sin(inputs[i][0]) };
            }
            while (true)
            {
                network.Randomize(new Random());
                GradientDescentTeacher teacher = new GradientDescentTeacher(network);
                int numOfGens = teacher.TrainNetwork(inputs, outputs, 0.03, 1);
                Console.WriteLine($"Num of generations: {numOfGens}");
                desmosPrint1N1(network, 0, Math.PI * 2);
                Console.ReadKey();
                Console.Clear();
            }
        }
        public static void desmosPrint1N1(Network network, double domainLower, double domainHigher)
        {
            Layer nLayer = network.Layers[0];
            for(int i = 0; i < nLayer.Neurons.Length; i++)
            {
                Neuron neuron = nLayer.Neurons[i];
                Console.WriteLine($"n_{{{i}}}(x) = a({neuron.Weights[0]}x + {neuron.Bias})");
            }
            Neuron outputNeuron = network.Layers[1].Neurons[0];
            Console.Write("y = a(");
            for(int i = 0; i < outputNeuron.Weights.Length; i++)
            {
                Console.Write($"n_{{{i}}}(x) * {outputNeuron.Weights[i]} + ");
            }
            Console.WriteLine($"{outputNeuron.Bias})\\left\\{{{domainLower}<x<{domainHigher}\\pi\\right\\}}");
        }
        static void Main(string[] args)
        {
            sinTrain();
        }
    }
}
