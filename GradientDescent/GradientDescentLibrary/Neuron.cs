using System;
using System.Collections.Generic;
using System.Text;

namespace GradientDescentLibrary
{
    public enum ActivationType
    {
        BinaryStep,
        Sigmoid
    }

    public class Neuron
    {
        public double Bias;
        public double[] Weights;
        public double WeightedSum;
        public double Output;
        public ActivationType ActivationFunc;

        public double PartialDerivative;

        public Neuron(ActivationType activationFunc, int numOfInputs)
        {
            ActivationFunc = activationFunc;
            Weights = new double[numOfInputs];
        }
        public void Randomize(Random rand)
        {
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = rand.NextDouble(-0.5, 0.5);
            }
            Bias = rand.NextDouble();
        }
        public double Compute(double[] inputs)
        {
            WeightedSum = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                WeightedSum += Weights[i] * inputs[i];
            }
            WeightedSum += Bias;
            Output = activate(WeightedSum);
            return Output;
        }
        private double activate(double value)
        {
            switch (ActivationFunc)
            {
                case ActivationType.BinaryStep:
                    return value > 0 ? 1 : 0;
                case ActivationType.Sigmoid:
                    {
                        return 1 / (1 + Math.Pow(Math.E, -value));
                    }
                default:
                    throw new InvalidOperationException();
            }
        }
        public void CopyTo(Neuron other)
        {
            other.Bias = Bias;
            Weights.CopyTo(other.Weights, 0);
            //other.ActivationFunc = ActivationFunc;
        }

        public void CalculatePartialDerivative(double error)
        {
            double activationDerivative;
            double input = WeightedSum;
            switch (ActivationFunc)
            {
                case ActivationType.Sigmoid:
                    activationDerivative = Output * (1 - Output);
                    break;
                default:
                    throw new NotImplementedException();
            }

            PartialDerivative = error * (activationDerivative);
        }
    }
}
