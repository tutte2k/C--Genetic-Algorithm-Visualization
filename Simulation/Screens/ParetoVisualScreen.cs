using Game.GeneticAlgorithm;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Screens
{
    public class ParetoVisualScreen : Screen
    {
        private List<Individual> population;
        private RectangleShape fitnessGraph;
        private RectangleShape fitnessGraphBackdrop;
        private List<CircleShape> individualVisuals;

        private static Vector2f zeroOffset = new Vector2f(1508, 1304);
        private static Vector2f graphSize = new Vector2f(670, 670);

        public ParetoVisualScreen(
            RenderWindow window,
            FloatRect configuration,
            List<Individual> population) : base(window, configuration)
        {
            this.population = population;

            this.individualVisuals = new List<CircleShape>();
            for (int i = 0; i < GAConfig.PopulationCount; i++)
            {
                individualVisuals.Add(new CircleShape(10));
            }

            this.fitnessGraph = new RectangleShape(new Vector2f(960, 960))
            {
                Texture = new Texture("Graph.png"),
                Position = new Vector2f(1400, 500)
            };

            this.fitnessGraphBackdrop = new RectangleShape(new Vector2f(1060, 960))
            {
                Position = new Vector2f(1350, 500),
                FillColor = new Color(0xee, 0xee, 0xee),
                OutlineColor = new Color(0x1e, 0x1e, 0x1e),
                OutlineThickness = 2
            };
        }

        public override void Update(float deltaT)
        {
            var minTime = population.Min(i => i.NormalizedTimeFitness);
            var maxTime = population.Max(i => i.NormalizedTimeFitness);
            var diffTime = maxTime - minTime;

            var minDistance = population.Min(i => i.NormalizedDistanceFitness);
            var maxDistance = population.Max(i => i.NormalizedDistanceFitness);
            var diffdistance = maxDistance - minDistance;

            var maxRank = population.Max(p => p.Rank);
            var step = 255 / maxRank;

            for (int i = 0; i < population.Count; i++)
            {
                var visual = individualVisuals[i];
                var individual = population[i];

                var x = (individual.NormalizedTimeFitness - minTime) / diffTime * graphSize.X;
                var y = -(individual.NormalizedDistanceFitness - minDistance) / diffdistance * graphSize.Y;
                visual.Position = new Vector2f(x, y) + zeroOffset;

                var intensity = individual.Rank * step;
                visual.FillColor = new Color((byte)intensity, 0, (byte)(255 - intensity));
            }
        }

        public override void Draw(float deltaT)
        {
            window.Draw(this.fitnessGraphBackdrop);

            window.Draw(this.fitnessGraph);

            individualVisuals.ForEach(v => window.Draw(v));
        }
    }
}

