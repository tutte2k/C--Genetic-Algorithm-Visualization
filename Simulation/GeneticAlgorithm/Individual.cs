using Game.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.GeneticAlgorithm
{
    public class Individual
    {
        public List<int> Sequence { get; set; }

        public int Rank { get; set; }

        public double CrowdingDistance { get; set; }

        public float DistanceFitness { get; set; }

        public float TimeFitness { get; set; }

        public float NormalizedDistanceFitness { get; set; }

        public float NormalizedTimeFitness { get; set; }

        public Individual(List<int> sequence)
        {
            this.Sequence = sequence;

            DistanceFitness = GetTotalDistance();
            TimeFitness = GetTotalTime();
        }

        public float GetTotalDistance()
        {
            var totalDistance = 0.0f;

            for (int i = 1; i < this.Sequence.Count(); i++)
            {
                var fromTown = TownHelper.TownPositions[Sequence[i - 1]];
                var toTown = TownHelper.TownPositions[Sequence[i]];

                var x = toTown.X - fromTown.X;
                var y = toTown.Y - fromTown.Y;

                var d = (float)Math.Sqrt(x * x + y * y);

                totalDistance += d;
            }

            return totalDistance;
        }

        public float GetTotalTime()
        {
            var totalTime = 0.0f;

            for (int i = 1; i < this.Sequence.Count(); i++)
            {
                var fromTown = TownHelper.TownPositions[Sequence[i - 1]];
                var toTown = TownHelper.TownPositions[Sequence[i]];

                var x = toTown.X - fromTown.X;
                var y = toTown.Y - fromTown.Y;

                var d = (float)Math.Sqrt(x * x + y * y);

                totalTime += d / TownHelper.PathSpeedLimits[(i - 1, i)];
            }

            return totalTime;
        }

        public override bool Equals(object obj)
        {
            if (obj is Individual individual)
            {
                return this.Sequence.SequenceEqual(individual.Sequence);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(Individual a, Individual b)
        {
            return a.Sequence.SequenceEqual(b.Sequence);
        }

        public static bool operator !=(Individual a, Individual b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
