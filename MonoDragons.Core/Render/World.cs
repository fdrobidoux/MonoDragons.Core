﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Memory;
using MonoDragons.Core.Physics;
using MonoDragons.Core.Text;
using MonoDragons.Core.UserInterface;

namespace MonoDragons.Core.Render
{
    public static class World
    {
        private static readonly ColoredRectangle _darken;
        
        private static SpriteBatch _spriteBatch;

        static World()
        {
            _darken = new ColoredRectangle
            {
                Color = Color.FromNonPremultiplied(0, 0, 0, 130),
                Transform = new Transform2(new Size2(1920, 1080))
            };
        }

        public static void Init(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            DefaultFont.Load(CurrentGame.ContentManager);
        }

        public static void DrawBackgroundColor(Color color)
        {
            CurrentGame.GraphicsDevice.Clear(color);
        }

        public static void Draw(Texture2D texture, Rectangle rectangle, Color tint)
        {
            _spriteBatch.Draw(texture, ScaleRectangle(rectangle), tint);
        }
        
        public static void Draw(Texture2D texture, Rectangle destination, Rectangle source, Color color)
        {
            _spriteBatch.Draw(texture, ScaleRectangle(destination), source, color);
        }

        public static void Draw(string imageName, Vector2 pixelPosition)
        {
            var resource = Resources.Load<Texture2D>(imageName);
            _spriteBatch.Draw(resource, new Rectangle(ScalePoint(pixelPosition), ScalePoint(resource.Width, resource.Height)), Color.White);
        }

        public static void Draw(string imageName, Transform2 transform)
        {
            Draw(imageName, transform.ToRectangle());
        }

        public static void Draw(string imageName, Transform2 transform, Color tint)
        {
            _spriteBatch.Draw(Resources.Load<Texture2D>(imageName), ScaleRectangle(transform.ToRectangle()), tint);
        }

        public static void Draw(string imageName, Rectangle rectPostion)
        {
            _spriteBatch.Draw(Resources.Load<Texture2D>(imageName), ScaleRectangle(rectPostion), Color.White);
        }

        public static void Draw(string imageName, Transform2 transform, Color tint, SpriteEffects fx)
        {
            var texture = Resources.Load<Texture2D>(imageName);
            _spriteBatch.Draw(texture: texture, 
                destinationRectangle: ScaleRectangle(transform.ToRectangle()), 
                sourceRectangle: null, 
                color: tint, 
                rotation: 0.0f, 
                origin: null, 
                effects: fx, 
                layerDepth: 0.0f);
        }
        
        public static void Draw(string imageName, Vector2 size, Anchor anchor)
        {
            _spriteBatch.Draw(Resources.Load<Texture2D>(imageName), ScaleRectangle(new Rectangle(
                    new Point(
                        anchor.AnchorFromLeft ? anchor.HorizontalOffset : (int)Math.Round(CurrentDisplay.GameWidth / CurrentDisplay.Scale - anchor.HorizontalOffset),
                        anchor.AnchorFromTop ? anchor.VerticalOffset : (int)Math.Round(CurrentDisplay.GameHeight / CurrentDisplay.Scale - anchor.VerticalOffset)),
                    size.ToPoint())),
                Color.White);
        }
        
        public static void DrawRotatedFromCenter(string name, Transform2 transform)
        {
            var resource = Resources.Load<Texture2D>(name);
            var x = transform.Rotation.Value;
            _spriteBatch.Draw(resource, null, ScaleRectangle(transform.ToRectangle()), null, new Vector2(resource.Width / 2, resource.Height / 2),
                transform.Rotation.Value * .017453292519f, new Vector2(1, 1));
        }

        public static void Draw(Texture2D texture, Vector2 pixelPosition)
        {
            _spriteBatch.Draw(texture, new Rectangle(ScalePoint(pixelPosition), ScalePoint(texture.Width, texture.Height)), Color.White);
        }

        public static void Draw(Texture2D texture, Rectangle rectPosition)
        {
            if (texture.Height > 1 && texture.Width > 1) // Clever hack to prevent from disposing of RectangleTextures
                Resources.Put(texture.GetHashCode().ToString(), texture);
            _spriteBatch.Draw(texture, ScaleRectangle(rectPosition), Color.White);
        }

        public static void Draw(Texture2D texture, Transform2 transform)
        {
            Draw(texture, transform.ToRectangle());
        }

        public static void DrawRotatedFromCenter(Texture2D texture, Rectangle rectPosition, Rotation2 rotation)
        {
            Resources.Put(texture.GetHashCode().ToString(), texture);
            var scaledRect = ScaleRectangle(rectPosition);
            _spriteBatch.Draw(texture, null, scaledRect, null, new Vector2(scaledRect.Width / 2, scaledRect.Height / 2),
                rotation.Value * .017453292519f, new Vector2(1, 1));
        }
        
        public static void Darken()
        {
            _darken.Draw(Transform2.Zero);
        }

        private static Rectangle ScaleRectangle(Rectangle rectangle)
        {
            return new Rectangle(ScalePoint(rectangle.Location), ScalePoint(rectangle.Size));
        }

        private static Point ScalePoint(float x, float y)
        {
            return ScalePoint(new Vector2(x, y));
        }

        private static Point ScalePoint(Vector2 vector)
        {
            return new Point((int)Math.Round(vector.X * CurrentDisplay.Scale), (int)Math.Round(vector.Y * CurrentDisplay.Scale));
        }

        private static Point ScalePoint(Point point)
        {
            return ScalePoint(point.ToVector2());
        }
    }
}
