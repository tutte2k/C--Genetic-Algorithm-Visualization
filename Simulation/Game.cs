using Game.GeneticAlgorithm;
using Game.Helpers;
using Game.Screens;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Game
{
    public class Game
    {
        private readonly RenderWindow window;
        private readonly ScreenManager screenManager;
        private readonly SimulationScreen pathScreen;
        private readonly ParetoVisualScreen paretoScreen;
        private readonly Clock clock;
        private readonly Clock generationClock;
        private readonly float generationTime = 0.2f;
        private readonly World world;
        private Task doGeneration;

        public Game()
        {
            window = new RenderWindow(
                new VideoMode(Configuration.Width, Configuration.Height),
                "World Simulation",
                Styles.Fullscreen,
                new ContextSettings() { AntialiasingLevel = 8 });

            window.SetFramerateLimit(60);

            window.Closed += OnClose;
            window.Resized += OnResize;

            world = new World();

            screenManager = new ScreenManager(window);

            pathScreen = new SimulationScreen(window, Configuration.SinglePlayer);
            paretoScreen = new ParetoVisualScreen(window, Configuration.SinglePlayer, world.Population);

            screenManager.AddScreen(paretoScreen);
            screenManager.AddScreen(pathScreen);

            SetParetoConfiguration();

            clock = new Clock();

            generationClock = new Clock();
        }

        public void Run()
        {
            world.Spawn();

            int generation = 0;
            generationClock.Restart();

            while (window.IsOpen)
            {
                float deltaT = clock.Restart().AsMicroseconds() / 1000000f;

                window.Clear(Configuration.Background);

                window.DispatchEvents();

                screenManager.UpdateCamera(deltaT);

                screenManager.Draw(deltaT);

                window.Display();

                if (generationClock.ElapsedTime.AsSeconds() > generationTime &&
                    (doGeneration?.IsCompleted ?? true) &&
                    !world.HasConverged)
                {
                    doGeneration = Task.Run(() =>
                    {
                        world.DoGeneration();

                        generationClock.Restart();

                        pathScreen.GenerationString.StringText = $"Generation: {++generation}";

                        pathScreen.UpdateSequence(world.GetBestIndividual());

                        screenManager.Update(deltaT);
                    });
                }

                if (world.GenerationCount == GAConfig.MaxGenerations ||
                    world.NoImprovementCount == GAConfig.MaxNoImprovementCount)
                {
                    pathScreen.SetGACompleted();

                    try
                    {
                        Clipboard.SetText(string.Join(",", world.FitnessOverTime));
                    }
                    catch (Exception)
                    {


                    }

                }
                this.ProcessUserInput();

                if (Keyboard.IsKeyPressed(Configuration.QuitKey) ||
                    Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                {
                    return;
                }
            }
        }

        private void ProcessUserInput()
        {
            if (KeyboardHelper.IsKeyJustPressed(Configuration.ParetoVisualisationKey))
            {
                paretoScreen.SetActiveState(!paretoScreen.IsDraw);
            }
        }

        private void SetParetoConfiguration()
        {
            if (Configuration.UseRandomTowns)
            {
                paretoScreen.Camera.Position =
                    new Vector2f(
                        (Configuration.Width / Configuration.Scale) / 2,
                        (Configuration.Height / Configuration.Scale) / 2);

                paretoScreen.Camera.GetView().Zoom(0.3f);
            }

            paretoScreen.IsDraw = false;
        }

        private void OnResize(object sender, SizeEventArgs e)
        {
            var window = (RenderWindow)sender;
            pathScreen.Camera.ScaleToWindow(window.Size.X, window.Size.Y);
        }

        private static void OnClose(object sender, EventArgs e)
        {
            ((RenderWindow)sender).Close();
        }
    }
}