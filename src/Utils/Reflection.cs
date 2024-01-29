using Blish_HUD.Content;
using Blish_HUD.Controls;
using System.Reflection;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace HexedHero.Blish_HUD.FlipperHelper.Utils
{

    // Fixes an issue where you cannot update the window's background after its been set,
    // even if you you a AsyncTexture2D and swap the texture later and I need to resize the background
    // and cannot without making everything wait for the AsyncTexture2D to complete.
    // I'm sorry...
    public class Reflection
    {

        public static void InjectNewBackground(TabbedWindow2 Window, AsyncTexture2D backgroundTexture, Rectangle Bounds)
        {

            InjectNewBackground(Window, (Texture2D)backgroundTexture, Bounds);

        }

        public static void InjectNewBackground(TabbedWindow2 Window, Texture2D backgroundTexture, Rectangle Bounds)
        {

            Type baseType = Window.GetType().BaseType;

            // Force set the WindowBackground
            PropertyInfo windowBackgroundPropertyInfo = baseType.GetProperty("WindowBackground", BindingFlags.NonPublic | BindingFlags.Instance);
            windowBackgroundPropertyInfo.SetValue(Window, (AsyncTexture2D)backgroundTexture);

            // Update the background bounds
            InjectNewBackgroundBounds(Window, Bounds);

        }

        public static void InjectNewBackgroundBounds(TabbedWindow2 Window, Rectangle Bounds)
        {

            Type baseType = Window.GetType().BaseType;

            // Force set the background image bounds
            PropertyInfo windowContainerPropertyInfo = baseType.GetProperty("BackgroundDestinationBounds", BindingFlags.NonPublic | BindingFlags.Instance);
            windowContainerPropertyInfo.SetValue(Window, Bounds);

        }

    }
}
