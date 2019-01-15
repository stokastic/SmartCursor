using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;

namespace SmartCursor
{
    public class ModEntry : Mod {
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper) {
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.Input.CursorMoved += OnCursorMoved;
        }

        /// <summary>Raised after the player moves the in-game cursor.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnCursorMoved(object sender, CursorMovedEventArgs e) {
            if (!Context.IsWorldReady || !Context.IsPlayerFree || Game1.player.isMoving() || Game1.player.isCharging || Game1.player.isRafting || Game1.player.isRidingHorse() || Game1.player.UsingTool || Game1.player.IsEmoting || !Game1.player.CanMove) {
                return;
            }

            int dir = Game1.player.FacingDirection;

            Vector2 grabTile = new Vector2(1, 0);
            switch(GetOctant(new Vector2(e.NewPosition.ScreenPixels.X, e.NewPosition.ScreenPixels.Y))) {
                case 0:
                    grabTile = new Vector2(1, 0);
                    dir = Game1.right;
                    break;
                case 1:
                    grabTile = new Vector2(1, -1);
                    dir = Game1.up;
                    break;
                case 2:
                    grabTile = new Vector2(0, -1);
                    dir = Game1.up;
                    break;
                case 3:
                    grabTile = new Vector2(-1, -1);
                    dir = Game1.up;
                    break;
                case 4:
                    grabTile = new Vector2(-1, 0);
                    dir = Game1.left;
                    break;
                case 5:
                    grabTile = new Vector2(-1, 1);
                    dir = Game1.down;
                    break;
                case 6:
                    grabTile = new Vector2(0, 1);
                    dir = Game1.down;
                    break;
                case 7:
                    grabTile = new Vector2(1, 1);
                    dir = Game1.down;
                    break;
                    
            }

            Game1.player.FacingDirection = dir;
            Game1.player.lastClick = new Vector2(Game1.player.getStandingX(), Game1.player.getStandingY()) + grabTile * 64; ;
        }

        private double GetAngle(Vector2 screenPixels) {
            Vector2 absolutePixels = screenPixels + new Vector2(Game1.viewport.X, Game1.viewport.Y);
            Vector2 position = new Vector2((int)Game1.player.Position.X + 30, (int)Game1.player.Position.Y);
            Vector2 delta = absolutePixels - position;
            double angle = Math.Atan2(-delta.Y, delta.X);
            double angle2;

            if (angle < 0.0) {
                angle2 = 2 * Math.PI + angle;
            } else {
                angle2 = angle;
            }
            angle2 = (angle2 / (2.0 * Math.PI)) * 360;
            return angle2;
        }

        private int GetOctant(Vector2 screenPixels) {
            double angle = GetAngle(screenPixels);

            angle += 22.5;

            if (angle < 45) {
                return 0;
            } else if (angle < 90) {
                return 1;
            } else if (angle < 135) {
                return 2;
            } else if (angle < 180) {
                return 3;
            } else if (angle < 225) {
                return 4;
            } else if (angle < 270) {
                return 5;
            } else if (angle < 315) {
                return 6;
            } else if (angle < 360) {
                return 7;
            }
            return 0;            
        }

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e) {
            if (!Context.IsWorldReady || !Context.IsPlayerFree || e.Button != SButton.MouseLeft || Game1.player.isCharging || Game1.player.isRidingHorse() || Game1.player.UsingTool) {
                return;
            }

            ICursorPosition cursor = e.Cursor;

            double angle = GetAngle(cursor.ScreenPixels);
            Vector2 grabTile = Game1.player.getTileLocation();

            angle += 22.5;

            int dir;
            if (angle < 45) {
                grabTile += new Vector2(1, 0);
                dir = Game1.right;
            } else if (angle < 90) {
                grabTile += new Vector2(1, -1);
                dir = Game1.up;
            } else if (angle < 135) {
                grabTile += new Vector2(0, -1);
                dir = Game1.up;
            } else if (angle < 180) {
                grabTile += new Vector2(-1, -1);
                dir = Game1.up;
            } else if (angle < 225) {
                grabTile += new Vector2(-1, 0);
                dir = Game1.left;
            } else if (angle < 270) {
                grabTile += new Vector2(-1, 1);
                dir = Game1.down;
            } else if (angle < 315) {
                grabTile += new Vector2(0, 1);
                dir = Game1.down;
            } else if (angle < 360) {
                grabTile += new Vector2(1, 1);
                dir = Game1.down;
            } else {
                grabTile += new Vector2(1, 0);
                dir = Game1.right;
            }

            Game1.player.lastClick = grabTile * 64 + new Vector2(32, 32);
            Game1.player.FacingDirection = dir;
        }
    }
}
