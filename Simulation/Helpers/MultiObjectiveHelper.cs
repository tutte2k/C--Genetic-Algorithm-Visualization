using Game.DataStructures;
using Game.ExtensionMethods;
using Game.GeneticAlgorithm;
using SFML.System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Helpers
{
    public static class MultiObjectiveHelper
    {
        public static void UpdatePopulationFitness(List<Individual> population)
        {

            foreach (var individual in population)
            {
                individual.Rank = -1;
                individual.CrowdingDistance = -1;
            }

            NormalizeFitnessValues(population);

            var remainingToBeRanked = population.ToList();

            int rank = 1;
            while (remainingToBeRanked.Any())
            {
                var individualsInRank = new List<Individual>();

                for (int i = 0; i < remainingToBeRanked.Count; i++)
                {
                    var individual = remainingToBeRanked[i];
                    if (IsNotDominated(individual, remainingToBeRanked))
                    {
                        individual.Rank = rank;
                        individualsInRank.Add(individual);
                    }
                }

                individualsInRank.ForEach(i => remainingToBeRanked.Remove(i));

                rank++;
            }

            var ranks = population.GroupBy(p => p.Rank);
            foreach (var singleRank in ranks)
            {
                CalculateCrowdingDistance(singleRank);
            }
        }
        private static void NormalizeFitnessValues(List<Individual> population)
        {
            var maxDistance = population.Max(i => i.DistanceFitness);
            var maxTime = population.Max(i => i.TimeFitness);

            population.ForEach(i => i.NormalizedDistanceFitness = i.DistanceFitness / maxDistance);
            population.ForEach(i => i.NormalizedTimeFitness = i.TimeFitness / maxTime);
        }

        private static void CalculateCrowdingDistance(IGrouping<int, Individual> singleRank)
        {
            var orderedIndividuals = singleRank.Select(i => i).OrderBy(i => i.NormalizedDistanceFitness).ToArray();
            var individualsInFront = orderedIndividuals.Count();

            for (int i = 0; i < individualsInFront; i++)
            {

                if (i == 0 || i == individualsInFront - 1)
                {
                    orderedIndividuals[i].CrowdingDistance = double.PositiveInfinity;
                }
                else
                {

                    var current = orderedIndividuals[i];
                    var left = orderedIndividuals[i - 1];
                    var right = orderedIndividuals[i + 1];


                    var currentPosition = new Vector2f(current.NormalizedTimeFitness, current.NormalizedDistanceFitness);
                    var leftPosition = new Vector2f(left.NormalizedTimeFitness, left.NormalizedDistanceFitness);
                    var rightPosition = new Vector2f(right.NormalizedTimeFitness, right.NormalizedDistanceFitness);


                    var distanceLeft = currentPosition.Distance(leftPosition);
                    var distanceRight = currentPosition.Distance(rightPosition);


                    orderedIndividuals[i].CrowdingDistance = distanceLeft + distanceRight;
                }
            }
        }

        internal static float CalculateArea(IOrderedEnumerable<Individual> firstRank)
        {
            var slices = GetSlices(firstRank);
            return slices.Sum(i => i.Area);
        }

        private static IEnumerable<Slice> GetSlices(IOrderedEnumerable<Individual> firstRank)
        {
            var previousSlice = new Slice(0, 0, 0, 0);
            foreach (var individual in firstRank)
            {
                previousSlice = new Slice(previousSlice.XUpper, 0, individual.TimeFitness, individual.DistanceFitness);
            }
            yield return previousSlice;
        }

        private static bool IsNotDominated(Individual individualA, List<Individual> remainingToBeRanked)
        {
            foreach (var individualB in remainingToBeRanked)
            {
                if (individualA == individualB)
                {
                    continue;
                }
                if (individualB.DistanceFitness <= individualA.DistanceFitness &&
                    individualB.TimeFitness <= individualA.TimeFitness)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
