using Game.Factories;
using Game.Helpers;
using Game.SFML_Text;
using SFML.Graphics;
using System.Collections.Generic;
using Game.GeneticAlgorithm;
using Game.ViewTools;
using Game.ExtensionMethods;
using SFML.System;

namespace Game.Screens
{
    public class SimulationScreen : Screen
    {
        private readonly List<ConvexShape> pathLines;

        private static Vector2f totalDistanceStringPosition = new Vector2f(Configuration.Width / 2, 50);

        private static Vector2f generationStringPosition = new Vector2f(50, 50);

        private static Vector2f quitStringPosition = new Vector2f(450, Configuration.Height - 100);

        private List<RectangleShape> townVisuals;

        private FontText totalDistanceString;

        private FontText quitString;

        private object pathLock = new object();

        private object distanceStringLock = new object();

        private FontText generationString;
        public FontText GenerationString 
        {
            get => generationString;
            set => generationString = value;
        }

        public SimulationScreen(
            RenderWindow window,
            FloatRect configuration)
            : base(window, configuration)
        {
            this.townVisuals = new List<RectangleShape>();
            this.pathLines = new List<ConvexShape>();

            TownHelper.Initialize();

            TownFactory.GetTowns().ForEach(t => this.townVisuals.Add(t.Shape));

            Camera = new Camera(Configuration.SinglePlayer);

            totalDistanceString = new FontText(new Font("font.ttf"), string.Empty, Color.Black, 3);
            GenerationString = new FontText(new Font("font.ttf"), $"Generation: {0}", Color.Black, 3);
            quitString = new FontText(new Font("font.ttf"), "Press 'Q' to quit.", Color.Black, 3);
        }

        public void UpdateSequence(Individual individual)
        {
            lock (pathLock)
            {
                pathLines.Clear();
                pathLines.AddRange(TownHelper.GetTownSequencePath(individual.Sequence));
            }

            lock (distanceStringLock)
            {
                totalDistanceString.StringText =
                    $"Distance: {individual.DistanceFitness:#.##}\t\t" +
                    $"Time: {individual.TimeFitness:#.##}";
            }
        }

        public override void Update(float deltaT)
        {
            base.Update(deltaT);
        }

        public override void Draw(float deltaT)
        {
            townVisuals.ForEach(t => window.Draw(t));

            lock (pathLock)
            {
                pathLines.ForEach(p => window.Draw(p));
            }
            lock (distanceStringLock)
            {
                window.DrawString(totalDistanceString, totalDistanceStringPosition);
            }

            window.DrawString(GenerationString, generationStringPosition, false);

            window.DrawString(quitString, quitStringPosition);
        }

        public void SetGACompleted()
        {
            totalDistanceString.TextColour = Color.Green;
        }
    }
}