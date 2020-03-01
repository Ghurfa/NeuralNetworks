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
        static void Main(string[] args)
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
            while(true)
            {
                network.Randomize(new Random());
                GradientDescentTeacher teacher = new GradientDescentTeacher(network);
                int numOfGens = teacher.TrainNetwork(inputs, xorOutputs, 0.01);
                Console.WriteLine($"Num of generations: {numOfGens}");
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
    }
}
