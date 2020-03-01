using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetworkLibrary
{
    public class Neuron
    {
        public double Bias;
        public double[] Weights;
        public double Output;
        public Func<double, double> ActivationFunc;

        public Neuron(Func<double, double> activationFunc, int numOfInputs)
        {
            ActivationFunc = activationFunc;
            Weights = new double[numOfInputs];
        }
        public void Randomize(Random rand)
        {
            for(int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = rand.NextDouble(-0.5, 0.5);
            }
            Bias = rand.NextDouble();
        }
        public double Compute(double[] inputs)
        {
            double sum = 0;
            for(int i = 0; i < inputs.Length; i++)
            {
                sum += Weights[i] * inputs[i];
            }
            sum += Bias;
            Output = ActivationFunc(sum);
            return Output;
        }
        public void CopyTo(Neuron other)
        {
            other.Bias = Bias;
            Weights.CopyTo(other.Weights, 0);
            //other.ActivationFunc = ActivationFunc;
        }
    }
}
