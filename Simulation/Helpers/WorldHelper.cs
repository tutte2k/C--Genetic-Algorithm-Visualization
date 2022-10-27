using Game.ExtensionMethods;
using Game.GeneticAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Helpers
{

    public static class WorldHelper
    {
        private static Random random = new Random();

        public static List<Individual> SpawnPopulation()
        {
            var population = new List<Individual>();

            while (population.Count < GAConfig.PopulationCount)
            {
                var individual = GenerateIndividual(Configuration.TownCount);
                if (!population.Contains(individual))
                {
                    population.Add(individual);
                }
            }

            return population;
        }

        public static Individual GenerateIndividual(int sequenceLength)
        {

            var sequence = Enumerable.Range(0, sequenceLength).ToList();


            sequence.Shuffle();

            return new Individual(sequence);
        }

        public static (Individual, Individual) GetCandidateParents(List<Individual> population)
        {
            var candidateA = population[random.Next(population.Count())];
            var candidateB = population[random.Next(population.Count())];

            while (candidateA == candidateB)
            {
                candidateB = population[random.Next(population.Count())];
            }

            return (candidateA, candidateB);
        }

        public static Individual TournamentSelection(Individual candidateA, Individual candidateB)
        {
            if (candidateA.Rank < candidateB.Rank)
            {
                return candidateA;
            }
            else if (candidateA.Rank == candidateB.Rank)
            {
                return candidateA.CrowdingDistance > candidateB.CrowdingDistance
                    ? candidateA
                    : candidateB;
            }
            else
            {
                return candidateB;
            }
        }

        public static Individual DoCrossover(Individual individualA, Individual individualB, int crossoverPosition = -1)
        {
            crossoverPosition = crossoverPosition == -1
                ? random.Next(1, individualA.Sequence.Count - 1)
                : crossoverPosition;

            var offspringSequence = individualA.Sequence.Take(crossoverPosition).ToList();

            var appeared = offspringSequence.ToHashSet();

            foreach (var town in individualB.Sequence)
            {
                if (appeared.Contains(town))
                {
                    continue;
                }

                offspringSequence.Add(town);
            }

            return new Individual(offspringSequence);
        }

        public static (int, int) GetUniqueTowns(List<int> sequence)
        {
            var townA = random.Next(sequence.Count());
            var townB = random.Next(sequence.Count());

            while (townB == townA)
            {
                townB = random.Next(sequence.Count());
            }

            return (townA, townB);
        }

        public static Individual DoRotateMutate(Individual individual)
        {
            var (townA, townB) = GetUniqueTowns(individual.Sequence);

            var sequence = individual.Sequence;

            int firstIndex = townA < townB ? townA : townB;
            int secondIndex = townA > townB ? townA : townB;

            var newSequence = sequence.Take(firstIndex).ToList();

            var middle = sequence.Skip(firstIndex).Take(secondIndex - firstIndex).Reverse();

            var end = sequence.Skip(secondIndex).ToList();

            newSequence.AddRange(middle);
            newSequence.AddRange(end);

            return new Individual(newSequence);
        }

        public static Individual DoSwapMutate(Individual individual)
        {
            var sequence = individual.Sequence.ToList();

            var (townA, townB) = GetUniqueTowns(individual.Sequence);

            sequence.SwapInPlace(townA, townB);

            return new Individual(sequence);
        }

        public static (Individual, Individual) Mutate(Individual individualA, Individual individualB)
        {
            var newIndividualA = new Individual(individualA.Sequence);
            var newindividualB = new Individual(individualB.Sequence);

            if (random.NextDouble() < GAConfig.MutationChance)
            {
                newIndividualA = DoMutate(individualA);
            }

            if (random.NextDouble() < GAConfig.MutationChance)
            {
                newindividualB = DoMutate(individualB);
            }

            return (newIndividualA, newindividualB);
        }

        private static Individual DoMutate(Individual individual)
        {
            if (random.NextDouble() > 0.5)
            {
                return DoSwapMutate(individual);
            }
            else
            {
                return DoRotateMutate(individual);
            }
        }
    }
}
