using Game.ExtensionMethods;
using Game.Helpers;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Game.GeneticAlgorithm
{
    public class World
    {
        private static Random random = new Random();

        public List<Individual> Population { get; set; }

        public List<double> FitnessOverTime { get; private set; }

        public int GenerationCount { get; private set; } = 0;

        private float previousConvergenceArea = float.MaxValue;
        public int NoImprovementCount { get; private set; } = 0;

        public bool HasConverged =>
              GenerationCount > GAConfig.MaxGenerations
            || NoImprovementCount > GAConfig.MaxNoImprovementCount;

        public World()
        {
            Population = new List<Individual>();
            FitnessOverTime = new List<double>();
        }

        public void Spawn()
        {
            this.Population.AddRange(WorldHelper.SpawnPopulation());
        }

        public void DoGeneration()
        {
            GenerationCount++;

            var offspring = new List<Individual>();

            while (offspring.Count < GAConfig.PopulationCount)
            {
                var mother = GetParent();
                var father = GetParent();

                while (mother == father)
                {
                    father = GetParent();
                }

                var (offspringA, offspringB) = GetOffspring(mother, father);

                (offspringA, offspringB) = Mutate(offspringA, offspringB);

                offspring.Add(offspringA);
                offspring.Add(offspringB);
            }

            Population.AddRange(offspring);

            MultiObjectiveHelper.UpdatePopulationFitness(Population);

            var newPopulation = new List<Individual>();
            
            foreach (var individual in Population.OrderBy(i => i.Rank).ThenByDescending(i => i.CrowdingDistance))
            {
                if (!newPopulation.Contains(individual))
                {
                    newPopulation.Add(individual);
                }
            }

            

            newPopulation = newPopulation.Take(GAConfig.PopulationCount).ToList();

            Population.Clear();
            newPopulation.ForEach(i => Population.Add(i));


            var firstRank = Population.Where(p => p.Rank == 1).OrderBy(p => p.TimeFitness);

            var currentArea = MultiObjectiveHelper.CalculateArea(firstRank);

            if (Math.Abs(previousConvergenceArea-currentArea)> 0.1)
            {

                NoImprovementCount++;

            }
            else
            {
                NoImprovementCount = 0;
                previousConvergenceArea = currentArea;
            }

        }

        public Individual GetBestIndividual()
        {
            var firstRank = Population.GroupBy(i => i.Rank).First().ToArray();
            return firstRank[random.Next(firstRank.Length)];
        }

        private (Individual, Individual) Mutate(Individual individualA, Individual individualB)
        {
            return WorldHelper.Mutate(individualA, individualB);
        }

        private (Individual, Individual) GetOffspring(Individual individualA, Individual individualB)
        {
            var offspringA = DoCrossover(individualA, individualB);
            var offspringB = DoCrossover(individualB, individualA);

            return (offspringA, offspringB);
        }

        private Individual DoCrossover(Individual individualA, Individual individualB)
        {
            return WorldHelper.DoCrossover(individualA, individualB);
        }

        private Individual GetParent()
        {
            var (candidate1, candidate2) = WorldHelper.GetCandidateParents(this.Population);

            return WorldHelper.TournamentSelection(candidate1, candidate2);
        }
    }
}