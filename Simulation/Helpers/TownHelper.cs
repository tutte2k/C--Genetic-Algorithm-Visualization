using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using Game.ExtensionMethods;

namespace Game.Helpers
{
    public static class TownHelper
    {
        private const int Linethickness = 4;
        private const int PathOffsetFromTown = 180;
        private const int MinimumSpeedInPixels = 10;
        private const int MaximumSpeedInPixels = 100;
        private const int SpeedRangeInPixels = MaximumSpeedInPixels - MinimumSpeedInPixels;
        private static Random random = new Random();

        public static List<ConvexShape> GetTownSequencePath(List<int> townSequence)
        {
            var paths = new List<ConvexShape>();

            for(int i = 1; i < townSequence.Count; i++)
            {
                var fromTown = TownPositions[townSequence[i - 1]];
                var toTown = TownPositions[townSequence[i]];

                var directionVector = (toTown - fromTown).Normalize();

                var startingPoint = fromTown + (directionVector * PathOffsetFromTown);
                var endingPoint = toTown - (directionVector * PathOffsetFromTown);

                var lumination = Convert.ToByte((200.0 / TownPositions.Count) * (i - 1));

                paths.Add(SFMLGraphicsHelper.GetLine(startingPoint, endingPoint, Linethickness, new Color(lumination, lumination, lumination)));
            }

            return paths;
        }

        public static void Initialize()
        {
            PopulateTowns();
            PopulateSpeedLimits();
        }

        private static void PopulateSpeedLimits()
        {
            var localRandom = new Random(17);
            PathSpeedLimits = new Dictionary<(int, int), float>();

            for (int fromTown = 0; fromTown < Configuration.TownCount; fromTown++)
            {
                for (int toTown = 0; toTown < Configuration.TownCount; toTown++)
                {
                    if (fromTown == toTown)
                    {
                        continue;
                    }

                    var pathDistance = TownPositions[toTown].Distance(TownPositions[fromTown]);

                    PathSpeedLimits.Add(
                        (fromTown, toTown), 
                        (float)(MinimumSpeedInPixels + SpeedRangeInPixels * localRandom.NextDouble() * pathDistance / 1000));
                }
            }
        }

        private static void PopulateTowns()
        {
            if (Configuration.UseRandomTowns)
            {
                for(int i = 0; i < Configuration.RandomTownCount; i++)
                {
                    TownPositions.Add(GeneratRandomTownPosition());
                }
            }
            else
            {
                TownPositions.AddRange(townPositions);
            }
        }

        private static Vector2f GeneratRandomTownPosition()
        {
            return new Vector2f
            {
                X = 100 + ((float)random.NextDouble() * (Configuration.Width - 100)),
                Y = 100 + ((float)random.NextDouble() * (Configuration.Height - 100))
            };
        }
        public static List<Vector2f> TownPositions = new List<Vector2f>();

        public static Dictionary<(int, int), float> PathSpeedLimits { get; set; }

        private static List<Vector2f> townPositions = new List<Vector2f>()
        {
            new Vector2f(3060, 1300),
            new Vector2f(1050, 450),
            new Vector2f(450, 750),
            new Vector2f(690, 1890),
            new Vector2f(1410, 1830),
            new Vector2f(2070, 1560),
            new Vector2f(1725, 1080),
            new Vector2f(3360, 810),
            new Vector2f(3450, 1770),
            new Vector2f(2460, 240),
        };
    }
}
