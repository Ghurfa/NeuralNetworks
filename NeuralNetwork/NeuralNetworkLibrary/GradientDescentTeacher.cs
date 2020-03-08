using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetworkLibrary
{
    public class GradientDescentTeacher
    {
        private Network network;
        private Dictionary<Neuron, double> partialDerivatives;
        private Dictionary<Neuron, double[]> prevWeightUpdates;
        private Dictionary<Neuron, double> prevBiasUpdates;
        private double[][][] weightUpdates;
        private double[][] biasUpdates;
        public GradientDescentTeacher(Network network)
        {
            this.network = network;
            partialDerivatives = new Dictionary<Neuron, double>();

            weightUpdates = new double[network.Layers.Length][][];
            biasUpdates = new double[network.Layers.Length][];

            for (int i = 0; i < network.Layers.Length; i++)
            {
                Layer layer = network.Layers[i];
                weightUpdates[i] = new double[layer.Neurons.Length][];
                for (int j = 0; j < layer.Neurons.Length; j++)
                {
                    Neuron neuron = layer.Neurons[j];
                    weightUpdates[i][j] = new double[neuron.Weights.Length];
                }
            }

            for (int i = 0; i < network.Layers.Length; i++)
            {
                Layer layer = network.Layers[i];
                biasUpdates[i] = new double[layer.Neurons.Length];
            }

            prevWeightUpdates = new Dictionary<Neuron, double[]>();
            foreach (Layer layer in network.Layers)
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    prevWeightUpdates[neuron] = new double[neuron.Weights.Length];
                }
            }

            prevBiasUpdates = new Dictionary<Neuron, double>();
            foreach (Layer layer in network.Layers)
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    prevBiasUpdates[neuron] = 0;
                }
            }
        }
        public int TrainNetwork(double[][] inputs, double[][] desiredOutputs, double goalError, int batchSize = -1)
        {
            int numOfGenerations = 0;
            double totalError = 1;
            if (batchSize < 0)
            {
                while (totalError > goalError)
                {
                    GradientDescent(inputs, desiredOutputs);
                    totalError = 0;
                    for (int i = 0; i < inputs.Length; i++)
                    {
                        totalError += network.MSE(inputs[i], desiredOutputs[i]);
                    }
                    //Console.WriteLine(totalError);
                    //network.PrintWeights();
                    numOfGenerations++;
                }
                return numOfGenerations;
            }
            else
            {
                while (totalError > goalError)
                {
                    MiniBatch(inputs, desiredOutputs, batchSize);
                    totalError = 0;
                    for (int i = 0; i < inputs.Length; i++)
                    {
                        totalError += network.MSE(inputs[i], desiredOutputs[i]);
                    }
                    //Console.WriteLine(totalError);
                    //network.PrintWeights();
                    numOfGenerations++;
                }
                return numOfGenerations;
            }
        }
        public void MiniBatch(double[][] inputs, double[][] desiredOutputs, int batchSize)
        {
            ReadOnlySpan<double[]> inputsSpan = inputs.AsSpan();
            ReadOnlySpan<double[]> desiredOutputsSpan = desiredOutputs.AsSpan();
            for(int i = 0; i < inputs.Length; i += batchSize)
            {
                ReadOnlySpan<double[]> batchInputs = inputsSpan.Slice(i, batchSize);
                ReadOnlySpan<double[]> batchDesiredOutputs = desiredOutputsSpan.Slice(i, batchSize);
                GradientDescent(batchInputs.ToArray(), batchDesiredOutputs.ToArray());
            }
        }
        public void GradientDescent(double[][] inputs, double[][] desiredOutputs)
        {
            partialDerivatives.Clear();
            for (int i = 0; i < inputs.Length; i++)
            {
                network.Compute(inputs[i]);
                calculateDerivatives(desiredOutputs[i]);
                calculateWeightUpdates(inputs[i], 0.05);
            }
            applyWeightUpdates(0.5);
        }
        private void calculatePartialDerivative(Neuron neuron, double error)
        {
            double activationDerivative;
            double input = neuron.WeightedSum;
            switch (network.ActivationFunc)
            {
                case ActivationType.Sigmoid:
                    activationDerivative = neuron.Output * (1 - neuron.Output);
                    break;
                case ActivationType.TanH:
                    activationDerivative = 1 - neuron.Output * neuron.Output;
                    break;
                default:
                    throw new NotImplementedException();
            }
            partialDerivatives[neuron] = error * activationDerivative;
        }
        private void calculateDerivatives(double[] desiredOutputs)
        {
            Layer outputLayer = network.Layers[network.Layers.Length - 1];
            for (int i = 0; i < outputLayer.Neurons.Length; i++)
            {
                double desiredOutput = desiredOutputs[i];
                Neuron neuron = outputLayer.Neurons[i];

                double error = desiredOutput - neuron.Output;
                calculatePartialDerivative(neuron, error);
            }

            for (int i = network.Layers.Length - 2; i >= 0; i--)
            {
                Layer layer = network.Layers[i];
                Layer nextLayer = network.Layers[i + 1];
                for (int j = 0; j < layer.Neurons.Length; j++)
                {
                    Neuron neuron = layer.Neurons[j];
                    double error = 0;
                    foreach (Neuron nextNeuron in nextLayer.Neurons)
                    {
                        error += partialDerivatives[nextNeuron] * nextNeuron.Weights[j];
                    }
                    calculatePartialDerivative(neuron, error);
                }
            }
        }

        private void calculateWeightUpdates(double[] inputs, double learningRate)
        {
            Layer inputLayer = network.Layers[0];
            for (int i = 0; i < inputLayer.Neurons.Length; i++)
            {
                Neuron neuron = inputLayer.Neurons[i];
                for (int j = 0; j < inputs.Length; j++)
                {
                    weightUpdates[0][i][j] += learningRate * partialDerivatives[neuron] * inputs[j];
                }
                biasUpdates[0][i] += learningRate * partialDerivatives[neuron];
            }

            for (int i = 1; i < network.Layers.Length; i++)
            {
                Layer layer = network.Layers[i];
                Layer prevLayer = network.Layers[i - 1];
                for (int j = 0; j < layer.Neurons.Length; j++)
                {
                    Neuron neuron = layer.Neurons[j];
                    for (int k = 0; k < neuron.Weights.Length; k++)
                    {
                        weightUpdates[i][j][k] += learningRate * partialDerivatives[neuron] * prevLayer.Output[k];
                    }
                    biasUpdates[i][j] += learningRate * partialDerivatives[neuron];
                }
            }
        }
        private void applyWeightUpdates(double momentum)
        {
            for (int i = 0; i < network.Layers.Length; i++)
            {
                Layer layer = network.Layers[i];
                for (int j = 0; j < layer.Neurons.Length; j++)
                {
                    Neuron neuron = layer.Neurons[j];
                    for (int k = 0; k < neuron.Weights.Length; k++)
                    {
                        double weightChange = weightUpdates[i][j][k] + prevWeightUpdates[neuron][k] * momentum;
                        prevWeightUpdates[neuron][k] = weightChange;
                        neuron.Weights[k] += weightChange;

                        weightUpdates[i][j][k] = 0;
                    }
                    double biasChange = biasUpdates[i][j] + prevBiasUpdates[neuron] * momentum;
                    prevBiasUpdates[neuron] = biasChange;
                    neuron.Bias += biasChange;

                    biasUpdates[i][j] = 0;
                }
            }
        }
    }
}
