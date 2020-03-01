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
    public class GeneticTeacher
    {
        ActivationType activationFunc;
        Func<Network, double> fitnessFunction;
        int inputCount;
        int[] neuronsPerLayer;

        public GeneticTeacher(ActivationType activationFunc, Func<Network, double> fitnessFunction, int inputCount, params int[] neuronsPerLayer)
        {
            this.activationFunc = activationFunc;
            this.inputCount = inputCount;
            this.fitnessFunction = fitnessFunction;
            this.neuronsPerLayer = neuronsPerLayer;
        }
        public Network[] CreatePopulation(int populationSize)
        {
            Network[] population = new Network[populationSize];
            for(int i = 0; i < populationSize; i++)
            {
                population[i] = new Network(activationFunc, inputCount, neuronsPerLayer);
            }
            return population;
        }
        public void RandomizePopulation(Network[] population, Random random)
        {
            for(int i = 0; i < population.Length; i++)
            {
                population[i].Randomize(random);
            }
        }
        public int TrainPopulation(Network[] population, Random random, double goalFitness, double mutationRate)
        {
            double topFitness = 0;
            (Network network, double fitness)[] populationWithFitness = new (Network, double)[population.Length];
            for (int i = 0; i < population.Length; i++)
            {
                populationWithFitness[i] = (population[i], 0);
            }
            int numOfGenerations = 0;
            while (topFitness < goalFitness)
            {
                for (int i = 0; i < populationWithFitness.Length; i++)
                {
                    populationWithFitness[i] = (population[i], fitnessFunction(population[i]));
                }
                EvolvePopulation(populationWithFitness, random, mutationRate);
                topFitness = populationWithFitness[0].fitness;
                numOfGenerations++;
            }
            for(int i = 0; i < population.Length; i++)
            {
                population[i] = populationWithFitness[i].network;
            }
            return numOfGenerations;
        }
        public void EvolvePopulation((Network network, double fitness)[] population, Random random, double mutationRate)
        {
            //Sorts best to worst
            Array.Sort(population, (a, b) => b.fitness.CompareTo(a.fitness));

            int upperBound = (int)(0.1 * population.Length);
            int lowerBound = (int)(0.9 * population.Length);

            for (int i = upperBound; i < lowerBound; i++)
            {
                Network superiorNetwork = population[random.Next(0, upperBound)].network;
                crossoverNetwork(population[i].network, superiorNetwork, random, CrossoverType.SinglePoint);
                mutateNetwork(population[i].network, random, 0.3, MutateWeightType.AddRandNum, MutateWeightType.ChangeByPercent, MutateWeightType.ChangeSign);
            }
            for (int i = lowerBound; i < population.Length; i++)
            {
                population[i].network.Randomize(random);
            }
        }
        private void crossoverNetwork(Network inferiorNetwork, Network superiorNetwork, Random rand, CrossoverType crossoverType)
        {
            switch (crossoverType)
            {
                case CrossoverType.SwapWeights:
                    throw new NotImplementedException();
                    break;
                case CrossoverType.SwapNeurons:
                    for (int i = 0; i < inferiorNetwork.Layers.Length; i++)
                    {
                        Layer superiorLayer = superiorNetwork.Layers[i];
                        Layer inferiorLayer = inferiorNetwork.Layers[i];

                        for (int j = 0; j < inferiorLayer.Neurons.Length; j++)
                        {
                            double swapRate = 0.4;
                            if (rand.NextDouble() > swapRate)
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
                    inferiorNetwork.Layers[whichLayer] = newLayer;
                    break;
                case CrossoverType.SinglePoint:
                    for (int i = 0; i < inferiorNetwork.Layers.Length; i++)
                    {
                        Layer superiorLayer = superiorNetwork.Layers[i];
                        Layer inferiorLayer = inferiorNetwork.Layers[i];

                        int cutPoint = rand.Next(inferiorLayer.Neurons.Length + 1); //Cut point = 1 is point between Neurons[0] and Neurons[1]
                        bool flip = rand.Next(2) == 1; //1 = forward

                        if (flip)
                        {
                            for (int j = cutPoint; j < inferiorLayer.Neurons.Length; j++)
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
        private void mutateNetwork(Network network, Random rand, double mutationRate, params MutateWeightType[] mutateTypes)
        {
            foreach (Layer layer in network.Layers)
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    for (int i = 0; i < neuron.Weights.Length; i++)
                    {
                        if (rand.NextDouble() < mutationRate)
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
    }
}
