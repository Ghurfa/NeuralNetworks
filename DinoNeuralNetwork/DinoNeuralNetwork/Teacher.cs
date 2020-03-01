using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetworkLibrary
{
    public class Teacher
    {
        protected Func<double, double> activationFunc;
        protected Func<Network, double> fitnessFunction;
        protected int inputCount;
        protected int[] neuronsPerLayer;

        public Teacher(Func<double, double> activationFunc, int inputCount, Func<Network, double> fitnessFunction, params int[] neuronsPerLayer)
        {
            this.activationFunc = activationFunc;
            this.inputCount = inputCount;
            this.fitnessFunction = fitnessFunction;
            this.neuronsPerLayer = neuronsPerLayer;
        }
        public virtual Network[] CreatePopulation(int populationSize)
        {
            Network[] population = new Network[populationSize];
            for(int i = 0; i < populationSize; i++)
            {
                population[i] = new Network(activationFunc, inputCount, neuronsPerLayer);
            }
            return population;
        }
        public virtual void RandomizePopulation(Network[] population, Random random)
        {
            for(int i = 0; i < population.Length; i++)
            {
                population[i].Randomize(random);
            }
        }
        public virtual int TrainPopulation(Network[] population, Random random, double goalFitness, double mutationRate)
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
        public virtual void EvolvePopulation((Network network, double fitness)[] population, Random random, double mutationRate)
        {
            //Sorts best to worst
            Array.Sort(population, (a, b) => b.fitness.CompareTo(a.fitness));

            int upperBound = (int)(0.1 * population.Length);
            int lowerBound = (int)(0.9 * population.Length);

            for (int i = upperBound; i < lowerBound; i++)
            {
                Network superiorNetwork = population[random.Next(0, upperBound)].network;
                population[i].network.Crossover(superiorNetwork, random, CrossoverType.SinglePoint);
                population[i].network.Mutate(random, 0.3, MutateWeightType.AddRandNum, MutateWeightType.ChangeByPercent, MutateWeightType.ChangeSign);
            }
            for (int i = lowerBound; i < population.Length; i++)
            {
                population[i].network.Randomize(random);
            }
        }
    }
}
