using System;
using System.Collections.Generic;
using System.Text;

namespace GradientDescentLibrary
{
    public class Layer
    {
        public Neuron[] Neurons;
        public double[] Output;

        public Layer(ActivationType activationFunc, int inputCount, int neuronCount)
        {
            Neurons = new Neuron[neuronCount];
            for(int i = 0; i < neuronCount; i++)
            {
                Neurons[i] = new Neuron(activationFunc, inputCount);
            }
            Output = new double[neuronCount];
        }
        public void Randomize(Random rand)
        {
            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i].Randomize(rand);
            }
        }
        public double[] Compute(double[] input)
        {
            for(int i = 0; i < Output.Length; i++)
            {
                Output[i] = Neurons[i].Compute(input);
            }
            return Output;
        }
        public void CopyTo(Layer other)
        {
            for(int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i].CopyTo(other.Neurons[i]);
            }
        }
    }
}
