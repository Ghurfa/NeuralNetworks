using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeuralNetworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinoNeuralNetwork
{
    public class DinoTrainer : Teacher
    {
        Rectangle dinosaurPosition;
        Texture2D spriteSheet;
        AnimationFrame[] dinoFrames;
        Dictionary<Network, ComputerDino> networkOwners;
        public DinoTrainer(Rectangle dinosaurPosition, Texture2D spriteSheet, AnimationFrame[] dinoFrames)
            : base( (input) => { return input > 0 ? 1 : 0; },
                    3,
                    null,
                    4, 2)
        {
            fitnessFunction = (network) => { return networkOwners[network].Score; };
            networkOwners = new Dictionary<Network, ComputerDino>();
            this.dinosaurPosition = dinosaurPosition;
            this.spriteSheet = spriteSheet;
            this.dinoFrames = dinoFrames;
        }

        public ComputerDino[] RecruitArmy(int numOfDinos)
        {
            Network[] brains = CreatePopulation(numOfDinos);
            ComputerDino[] population = new ComputerDino[numOfDinos];
            for(int i= 0; i < brains.Length; i++)
            {
                population[i] = new ComputerDino(dinosaurPosition, spriteSheet, new Vector2(0, 0), dinoFrames, brains[i]);
                networkOwners[brains[i]] = population[i];
            }
            return population;
        }
        public void RandomizeArmy(ComputerDino[] army, Random random)
        {
            foreach(ComputerDino dino in army)
            {
                dino.Brain.Randomize(random);
            }
        }

        public void EvolveArmy(ComputerDino[] army, Random random, double mutationRate)
        {
            (ComputerDino dino, double fitness)[] populationWithFitness = new (ComputerDino, double)[army.Length];
            for (int i = 0; i < army.Length; i++)
            {
                populationWithFitness[i] = (army[i], army[i].Score);
            }

            //Sorts best to worst
            Array.Sort(populationWithFitness, (a, b) => b.fitness.CompareTo(a.fitness));

            int upperBound = (int)(0.1 * army.Length);
            int lowerBound = (int)(0.9 * army.Length);

            for (int i = upperBound; i < lowerBound; i++)
            {
                Network superiorNetwork = populationWithFitness[random.Next(0, upperBound)].dino.Brain;
                populationWithFitness[i].dino.Brain.Crossover(superiorNetwork, random, CrossoverType.SinglePoint);
                populationWithFitness[i].dino.Brain.Mutate(random, 0.3, MutateWeightType.AddRandNum, MutateWeightType.ChangeByPercent, MutateWeightType.ChangeSign);
            }
            for (int i = lowerBound; i < populationWithFitness.Length; i++)
            {
                populationWithFitness[i].dino.Brain.Randomize(random);
            }
        }
    }
}
