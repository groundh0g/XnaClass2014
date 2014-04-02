using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceGame
{
    public static class GamePadEx
    {
        public static bool DPadMimicsThumbstickLeft { get; set; }

        #region " standard GamePad functionality "

        public static GamePadCapabilities GetCapabilities(PlayerIndex playerIndex)
        {
            return GamePad.GetCapabilities(playerIndex);
        }

        public static GamePadState GetState(PlayerIndex playerIndex)
        {
            return GetState(playerIndex, GamePadDeadZone.IndependentAxes);
        }

        public static GamePadState GetState(PlayerIndex playerIndex, GamePadDeadZone deadZoneMode)
        {
            return Combine(
                GamePad.GetState(playerIndex, deadZoneMode),
                GetPressedKeyMappedButtons(playerIndex)
            );
        }

        public static bool SetVibration(PlayerIndex playerIndex, float leftMotor, float rightMotor)
        {
            return GamePad.SetVibration(playerIndex, leftMotor, rightMotor);
        }

        #endregion

        #region " KeyBoard Mappings "

        private static Dictionary<PlayerIndex, Dictionary<Keys, Buttons>> KeyMappings =
            new Dictionary<PlayerIndex, Dictionary<Keys, Buttons>>() 
		{
			{ PlayerIndex.One, new Dictionary<Keys, Buttons> () },
			{ PlayerIndex.Two, new Dictionary<Keys, Buttons> () },
			{ PlayerIndex.Three, new Dictionary<Keys, Buttons> () },
			{ PlayerIndex.Four, new Dictionary<Keys, Buttons> () },
		};

        public static void ClearKeyMappings()
        {
            ClearKeyMappings(PlayerIndex.One);
        }

        public static void ClearKeyMappingsForAll()
        {
            ClearKeyMappings(PlayerIndex.One);
            ClearKeyMappings(PlayerIndex.Two);
            ClearKeyMappings(PlayerIndex.Three);
            ClearKeyMappings(PlayerIndex.Four);
        }

        public static void ClearKeyMappings(PlayerIndex playerIndex)
        {
            KeyMappings[playerIndex].Clear();
        }

        public static void AddKeyMap(Keys key, Buttons button)
        {
            AddKeyMap(PlayerIndex.One, key, button);
        }

        public static void AddKeyMap(PlayerIndex playerIndex, Dictionary<Keys, Buttons> keyMap)
        {
            ClearKeyMappings(playerIndex);
            if (keyMap != null && keyMap.Count > 0)
            {
                foreach (Keys key in keyMap.Keys)
                {
                    AddKeyMap(key, keyMap[key]);
                }
            }
        }

        public static void AddKeyMap(PlayerIndex playerIndex, Keys key, Buttons buttons)
        {
            if (key == 0)
            {
                throw new Exception("You must specify exactly one keyboard key.");
            }

            var numBits = NumberOfSetBits((int)buttons);
            if (numBits < 1)
            {
                throw new Exception("You must specify at least one gamepad button.");
            }

            if (KeyMappings[playerIndex].ContainsKey(key))
            {
                KeyMappings[playerIndex].Remove(key);
            }

            KeyMappings[playerIndex].Add(key, buttons);
        }

        private static int NumberOfSetBits(int i)
        {
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }

        public static Buttons GetPressedKeyMappedButtons(PlayerIndex playerIndex)
        {
            Buttons buttons = 0;
            var keyMappings = KeyMappings[playerIndex];
            var keyboard = Keyboard.GetState();
            foreach (Keys key in keyMappings.Keys)
            {
                if (keyboard.IsKeyDown(key))
                {
                    buttons |= keyMappings[key];
                }
            }
            return buttons;
        }

        public static void UseDefaultKeyMappings()
        {
            GamePadEx.ClearKeyMappingsForAll();

            GamePadEx.AddKeyMap(Keys.Space, Buttons.A);
            GamePadEx.AddKeyMap(Keys.C, Buttons.B);
            GamePadEx.AddKeyMap(Keys.X, Buttons.X);
            GamePadEx.AddKeyMap(Keys.Z, Buttons.Y);
            GamePadEx.AddKeyMap(Keys.Enter, Buttons.Start);
            GamePadEx.AddKeyMap(Keys.Escape, Buttons.Back);
            GamePadEx.AddKeyMap(Keys.Up, Buttons.DPadUp | Buttons.LeftThumbstickUp);
            GamePadEx.AddKeyMap(Keys.Down, Buttons.DPadDown | Buttons.LeftThumbstickDown);
            GamePadEx.AddKeyMap(Keys.Left, Buttons.DPadLeft | Buttons.LeftThumbstickLeft);
            GamePadEx.AddKeyMap(Keys.Right, Buttons.DPadRight | Buttons.LeftThumbstickRight);

            GamePadEx.DPadMimicsThumbstickLeft = true;
        }


        #endregion

        private static readonly Buttons GAMEPAD_DIGITAL_BUTTONS =
            Buttons.A |
            Buttons.B |
            Buttons.X |
            Buttons.Y |
            Buttons.Back |
            Buttons.Start |
            Buttons.BigButton |
            Buttons.LeftShoulder |
            Buttons.RightShoulder |
            Buttons.LeftStick |
            Buttons.RightStick |
            Buttons.BigButton;

        private static GamePadState Combine(GamePadState gamepad, Buttons buttons)
        {
            var result = gamepad;

            if (buttons > 0)
            {
                var gpThumbSticks =
                    new GamePadThumbSticks(
                        new Vector2(
                            (buttons & Buttons.LeftThumbstickLeft) > 0 ? -1 : (buttons & Buttons.LeftThumbstickRight) > 0 ? 1 : gamepad.ThumbSticks.Left.X,
                            (buttons & Buttons.LeftThumbstickDown) > 0 ? -1 : (buttons & Buttons.LeftThumbstickUp) > 0 ? 1 : gamepad.ThumbSticks.Left.Y),
                        new Vector2(
                            (buttons & Buttons.RightThumbstickLeft) > 0 ? -1 : (buttons & Buttons.RightThumbstickRight) > 0 ? 1 : gamepad.ThumbSticks.Right.X,
                            (buttons & Buttons.RightThumbstickDown) > 0 ? -1 : (buttons & Buttons.RightThumbstickUp) > 0 ? 1 : gamepad.ThumbSticks.Right.Y)
                    );

                var gpTriggers =
                    new GamePadTriggers(
                        Math.Max(gamepad.Triggers.Left, (buttons & Buttons.LeftTrigger) > 0 ? 1.0f : 0.0f),
                        Math.Max(gamepad.Triggers.Right, (buttons & Buttons.RightTrigger) > 0 ? 1.0f : 0.0f)
                    );

                var gpDPad =
                    new GamePadDPad(
                        (buttons & Buttons.DPadUp) > 0 ? ButtonState.Pressed : gamepad.DPad.Up,
                        (buttons & Buttons.DPadDown) > 0 ? ButtonState.Pressed : gamepad.DPad.Down,
                        (buttons & Buttons.DPadLeft) > 0 ? ButtonState.Pressed : gamepad.DPad.Left,
                        (buttons & Buttons.DPadRight) > 0 ? ButtonState.Pressed : gamepad.DPad.Right
                    );

                var gpButtons = new GamePadButtons(buttons & GAMEPAD_DIGITAL_BUTTONS);

                result = new GamePadState(gpThumbSticks, gpTriggers, gpButtons, gpDPad);
            }

            if (GamePadEx.DPadMimicsThumbstickLeft)
            {

                var left = result.ThumbSticks.Left;

                if (result.DPad.Left == ButtonState.Pressed) { left.X = -1.0f; }
                if (result.DPad.Right == ButtonState.Pressed) { left.X = 1.0f; }
                if (result.DPad.Up == ButtonState.Pressed) { left.Y = 1.0f; }
                if (result.DPad.Down == ButtonState.Pressed) { left.Y = -1.0f; }

                result =
                    new GamePadState(
                        new GamePadThumbSticks(left, result.ThumbSticks.Right),
                        result.Triggers,
                        result.Buttons,
                        result.DPad
                    );
            }

            return result;
        }

    }
}

