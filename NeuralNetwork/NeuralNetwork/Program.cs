using System;
using System.Linq;
using NeuralNetworkLibrary;

namespace NeuralNetwork
{
    class Program
    {
        /*static void TrainNetwork(Network network, double[][] inputs, double[][] desiredOutputs)
        {

            Random random = new Random();
            bool passes = false;
            while (!passes)
            {
                passes = true;
                for (int i = 0; i < inputs.Length; i++)
                {
                    if (network.MAE(inputs[i], desiredOutputs[i]) > 0)
                    {
                        passes = false;
                        break;
                    }
                }
                if (!passes)
                {
                    network.Randomize(random);
                }
            }
        }*/
        static bool CheckNetwork(Network network, double[][] inputs, double[][] desiredOutputs)
        {
            for(int i = 0; i < inputs.Length; i++)
            {
                if (network.MAE(inputs[i], desiredOutputs[i]) > 0)
                {
                    return false;
                }
            }
            return true;
        }
        static void Main(string[] args)
        {
            Func<double, double> binaryStep = (input) => { return input > 0 ? 1 : 0; };
            double[][] inputs = {
                new double[]{ 0, 0 },
                new double[]{ 0, 1 },
                new double[]{ 1, 0 },
                new double[]{ 1, 1 }
            };
            double[][] xorOutputs = {
                new double[]{ 0 },
                new double[]{ 1 },
                new double[]{ 1 },
                new double[]{ 0 }
            };
            Func<Network, double> xorFitnessFunction = (network) =>
            {
                double totalMAE = 0;
                for (int i = 0; i < inputs.Length; i++)
                {
                    totalMAE += network.MAE(inputs[i], xorOutputs[i]);
                }
                return 4 - totalMAE;
            };

            Random random = new Random();
            GeneticTeacher xorTeacher = new GeneticTeacher(ActivationType.BinaryStep, xorFitnessFunction, 2, 2, 1);
            Network[] population = xorTeacher.CreatePopulation(50);
            xorTeacher.RandomizePopulation(population, random);
            int numOfGenerations = xorTeacher.TrainPopulation(population, random, 4, 0.3);
            Console.WriteLine($"Number of Generations: {numOfGenerations}");
            if(CheckNetwork(population[0], inputs, xorOutputs))
            {
                Console.WriteLine("XOR pass");
            }
            else
            {
                Console.WriteLine("XOR fail");
            }
            Console.ReadKey();
        }
    }
}
