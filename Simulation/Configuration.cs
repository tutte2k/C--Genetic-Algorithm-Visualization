﻿using Game.Helpers;
using SFML.Graphics;
using SFML.Window;
using static SFML.Window.Keyboard;

namespace Game
{
    public static class Configuration
    {
        public static float Scale => UseRandomTowns ? 4f : 1f;

        public static uint Height => (uint)(2160 * Scale);

        public static uint Width => (uint)(3840 * Scale);

        public static Color Background => new Color(233, 233, 233);

        public static bool AllowCameraMovement => true;

        public static float CameraMovementSpeed => 1200f;

        public static float CameraZoomSpeed => 0.05f;

        public static float CameraRotationSpeed => 45f;

        public static Keyboard.Key QuitKey => Keyboard.Key.Q;

        public static Keyboard.Key ParetoVisualisationKey => Keyboard.Key.Space;

        public static Key PanLeft => Key.A;

        public static Key PanRight => Key.D;

        public static Key PanUp => Key.W;

        public static Key PanDown => Key.S;

        public static Key ZoomIn => Key.Z;

        public static Key ZoomOut => Key.X;

        public static Key RotateRight => Key.Num1;

        public static Key RotateLeft => Key.Num2;

        public static FloatRect SinglePlayer => new FloatRect(0, 0, 1, 1);

        public static FloatRect SinglePlayerCentered => new FloatRect(0.25f, 0.25f, 0.5f, 0.5f);

        public static FloatRect TwoPlayerLeft => new FloatRect(0, 0, 0.5f, 1);

        public static FloatRect TwoPlayerRight => new FloatRect(0.5f, 0, 0.5f, 1);

        public static FloatRect FourPlayerTopLeft => new FloatRect(0, 0, 0.5f, 0.5f);

        public static FloatRect FourPlayerTopRight => new FloatRect(0.5f, 0, 0.5f, 0.5f);

        public static FloatRect FourPlayerBottomLeft => new FloatRect(0, 0.5f, 0.5f, 0.5f);

        public static FloatRect FourPlayerBottomRight => new FloatRect(0.5f, 0.5f, 0.5f, 0.5f);

        public static bool UseRandomTowns => true;

        public static int RandomTownCount => 20;

        public static int TownCount => UseRandomTowns ? RandomTownCount : TownHelper.TownPositions.Count;
    }
}