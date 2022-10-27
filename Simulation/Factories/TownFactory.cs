using Game.DataStructures;
using Game.Helpers;
using SFML.Graphics;
using System;
using System.Collections.Generic;

namespace Game.Factories
{
    public static class TownFactory
    {
        private static Random random = new Random();

        public static List<Town> GetTowns()
        {
            var towns = new List<Town>();

            for(int i = 0; i < TownHelper.TownPositions.Count; i++)
            {
                var townPosition = TownHelper.TownPositions[i];

                towns.Add(new Town(townPosition, new Texture(GetTownTexture(i))));
            }

            return towns;
        }

        private static string GetTownTexture(int i)
        {
            if (Configuration.UseRandomTowns)
            {
                return $"../../Resources/Town_{random.Next(1, 10)}.png";
            }
            else
            {
                return $"../../Resources/Town_{i + 1}.png";
            }
        }
    }
}
