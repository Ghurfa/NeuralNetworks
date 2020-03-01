using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetworkLibrary
{
    public class Network
    {
        public Layer[] Layers { get; set; }
        public double[] Output { get; private set; }

        public ActivationType ActivationFunc;

        public Network(ActivationType activationFunc, int inputCount, params int[] neuronsPerLayer)
        {
            Layers = new Layer[neuronsPerLayer.Length];
            int prevOutputCount = inputCount;
            for (int i = 0; i < Layers.Length; i++)
            {
                Layers[i] = new Layer(activationFunc, prevOutputCount, neuronsPerLayer[i]);
                prevOutputCount = neuronsPerLayer[i];
            }
            ActivationFunc = activationFunc;
        }
        public void Randomize(Random rand)
        {
            foreach (Layer layer in Layers)
            {
                layer.Randomize(rand);
            }
        }
        public double[] Compute(double[] input)
        {
            double[] prevOutput = input;
            for (int i = 0; i < Layers.Length; i++)
            {
                prevOutput = Layers[i].Compute(prevOutput);
            }
            Output = prevOutput;
            return prevOutput;
        }

        public double MAE(double[] input, double[] desiredOutput)
        {
            Compute(input);
            double sum = 0;
            for (int i = 0; i < Output.Length; i++)
            {
                sum += Math.Abs(Output[i] - desiredOutput[i]);
            }
            return sum / Output.Length;
        }
        public double MSE(double[] input, double[] desiredOutput)
        {
            Compute(input);
            double sum = 0;
            for (int i = 0; i < Output.Length; i++)
            {
                double error = Output[i] - desiredOutput[i];
                sum += error * error;
            }
            return sum / Output.Length;
        }

        public void PrintWeights()
        {
            foreach (Layer layer in Layers)
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    foreach (double weight in neuron.Weights)
                    {
                        Console.Write($" {weight} ");
                    }
                    Console.Write($" {neuron.Bias} | ");
                }
            }
            Console.WriteLine();
        }
    }
}
