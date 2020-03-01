using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetworkLibrary
{
    public enum MutateWeightType
    {
        ReplaceValue,
        ChangeByPercent,
        AddRandNum,
        ChangeSign,
        SwapWeights
    }
    public enum CrossoverType
    {
        SwapWeights,
        SwapNeurons,
        SwapLayers,
        SinglePoint,
        DoublePoint
    }
    public class Network
    {
        public Layer[] Layers;
        public double[] Output { get; private set; }

        public Network(Func<double, double> activationFunc, int inputCount, params int[] neuronsPerLayer)
        {
            Layers = new Layer[neuronsPerLayer.Length];
            int prevOutputCount = inputCount;
            for (int i = 0; i < Layers.Length; i++)
            {
                Layers[i] = new Layer(activationFunc, prevOutputCount, neuronsPerLayer[i]);
                prevOutputCount = neuronsPerLayer[i];
            }
        }
        public void Randomize(Random rand)
        {
            foreach(Layer layer in Layers)
            {
                layer.Randomize(rand);
            }
        }
        public double[] Compute(double[] input)
        {
            double[] prevOutput = input;
            for(int i = 0; i < Layers.Length; i++)
            {
                prevOutput = Layers[i].Compute(prevOutput);
            }
            return prevOutput;
        }
        public void Mutate(Random rand, double mutationRate, params MutateWeightType[] mutateTypes)
        {
            foreach(Layer layer in Layers)
            {
                foreach(Neuron neuron in layer.Neurons)
                {
                    for(int i = 0; i < neuron.Weights.Length; i++)
                    {
                        if(rand.NextDouble() < mutationRate)
                        {
                            int mutateChoice = rand.Next(mutateTypes.Length);
                            MutateWeightType mutateType = mutateTypes[mutateChoice];
                            switch (mutateType)
                            {
                                case MutateWeightType.ReplaceValue:
                                    neuron.Weights[i] = rand.NextDouble(-0.5, 0.5);
                                    break;
                                case MutateWeightType.ChangeByPercent:
                                    double ratio = rand.NextDouble(0.5, 2.5);
                                    neuron.Weights[i] *= ratio;
                                    break;
                                case MutateWeightType.AddRandNum:
                                    double addend = rand.NextDouble(-1, 1);
                                    neuron.Weights[i] += addend;
                                    break;
                                case MutateWeightType.ChangeSign:
                                    neuron.Weights[i] *= -1;
                                    break;
                                case MutateWeightType.SwapWeights:
                                    throw new NotImplementedException();
                                    break;
                            }
                        }
                    }
                    if (rand.NextDouble() < mutationRate)
                    {
                        int mutateChoice = rand.Next(mutateTypes.Length);
                        MutateWeightType mutateType = mutateTypes[mutateChoice];
                        switch (mutateType)
                        {
                            case MutateWeightType.ReplaceValue:
                                neuron.Bias = rand.NextDouble(-0.5, 0.5);
                                break;
                            case MutateWeightType.ChangeByPercent:
                                double ratio = rand.NextDouble(0.5, 2.5);
                                neuron.Bias *= ratio;
                                break;
                            case MutateWeightType.AddRandNum:
                                double addend = rand.NextDouble(-1, 1);
                                neuron.Bias += addend;
                                break;
                            case MutateWeightType.ChangeSign:
                                neuron.Bias *= -1;
                                break;
                            case MutateWeightType.SwapWeights:
                                throw new NotImplementedException();
                                break;
                        }
                    }
                }
            }
        }
        public void Crossover(Network superiorNetwork, Random rand, CrossoverType crossoverType)
        {
            switch (crossoverType)
            {
                case CrossoverType.SwapWeights:
                    throw new NotImplementedException();
                    break;
                case CrossoverType.SwapNeurons:
                    for (int i = 0; i < Layers.Length; i++)
                    {
                        Layer superiorLayer = superiorNetwork.Layers[i];
                        Layer inferiorLayer = Layers[i];

                        for(int j = 0; j < inferiorLayer.Neurons.Length; j++)
                        {
                            double swapRate = 0.4;
                            if(rand.NextDouble() > swapRate)
                            {
                                superiorLayer.Neurons[j].CopyTo(inferiorLayer.Neurons[j]);
                            }
                        }
                    }
                    break;
                case CrossoverType.SwapLayers:
                    int whichLayer = rand.Next(superiorNetwork.Layers.Length);
                    Layer layerToCopy = superiorNetwork.Layers[whichLayer];
                    Layer newLayer = new Layer(layerToCopy.Neurons[0].ActivationFunc, layerToCopy.Neurons[0].Weights.Length, layerToCopy.Neurons.Length);
                    layerToCopy.CopyTo(newLayer);
                    Layers[whichLayer] = newLayer;
                    break;
                case CrossoverType.SinglePoint:
                    for(int i = 0; i < Layers.Length; i++)
                    {
                        Layer superiorLayer = superiorNetwork.Layers[i];
                        Layer inferiorLayer = Layers[i];

                        int cutPoint = rand.Next(inferiorLayer.Neurons.Length + 1); //Cut point = 1 is point between Neurons[0] and Neurons[1]
                        bool flip = rand.Next(2) == 1; //1 = forward

                        if(flip)
                        {
                            for(int j = cutPoint; j < inferiorLayer.Neurons.Length; j++)
                            {
                                superiorLayer.Neurons[j].CopyTo(inferiorLayer.Neurons[j]);
                            }
                        }
                        else
                        {
                            for (int j = cutPoint - 1; j >= 0; j--)
                            {
                                superiorLayer.Neurons[j].CopyTo(inferiorLayer.Neurons[j]);
                            }
                        }
                    }
                    break;
                case CrossoverType.DoublePoint:
                    throw new NotImplementedException();
                    break;
            }

        }
        public double MAE(double[] input, double[] desiredOutput)
        {
            double[] computedOutput = Compute(input);
            double sum = 0;
            for(int i = 0; i < computedOutput.Length; i++)
            {
                sum += Math.Abs(computedOutput[i] - desiredOutput[i]);
            }
            return sum / computedOutput.Length;
        }
    }
}
