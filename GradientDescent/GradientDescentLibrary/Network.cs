using System;
using System.Collections.Generic;
using System.Text;

namespace GradientDescentLibrary
{
    public class Network
    {
        public Layer[] Layers;
        public double[] Output { get; private set; }

        private Dictionary<Neuron, double> GDChanges;
        private Layer outputLayer;
        public Network(ActivationType activationFunc, int inputCount, params int[] neuronsPerLayer)
        {
            Layers = new Layer[neuronsPerLayer.Length];
            int prevOutputCount = inputCount;
            for (int i = 0; i < Layers.Length; i++)
            {
                Layers[i] = new Layer(activationFunc, prevOutputCount, neuronsPerLayer[i]);
                prevOutputCount = neuronsPerLayer[i];
            }
            outputLayer = Layers[Layers.Length - 1];
            GDChanges = new Dictionary<Neuron, double>();
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
            for (int i = 1; i < Layers.Length; i++)
            {
                prevOutput = Layers[i].Compute(prevOutput);
            }
            return prevOutput;
        }
        public double MSE(double[] inputs, double[] desiredOutputs)
        {
            Compute(inputs);
            double sum = 0;
            for(int i = 0; i < Output.Length; i++)
            {
                sum += (desiredOutputs[i] - Output[i]) * (desiredOutputs[i] - Output[i]);
            }
            return sum;
        }
        

    }
}
