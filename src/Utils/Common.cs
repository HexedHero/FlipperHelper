using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexedHero.Blish_HUD.FlipperHelper.Utils
{

    public class Common
    {

        public static Texture2D ResizeTexture2D(Texture2D originalTexture, int newWidth, int newHeight)
        {

            // Create a render target to draw the original texture onto
            RenderTarget2D renderTarget = new RenderTarget2D(
                originalTexture.GraphicsDevice,
                newWidth,
                newHeight);

            // Set the render target
            originalTexture.GraphicsDevice.SetRenderTarget(renderTarget);

            // Clear the render target
            originalTexture.GraphicsDevice.Clear(Color.Transparent);

            // Draw the original texture onto the render target with the new size
            using (SpriteBatch spriteBatch = new SpriteBatch(originalTexture.GraphicsDevice))
            {

                spriteBatch.Begin();
                spriteBatch.Draw(originalTexture, new Rectangle(0, 0, newWidth, newHeight), Color.White);
                spriteBatch.End();
                spriteBatch.Dispose();

            }

            // Reset the render target
            originalTexture.GraphicsDevice.SetRenderTarget(null);

            // Get the data from the render target
            Color[] data = new Color[newWidth * newHeight];
            renderTarget.GetData(data);

            // Create the resized texture using the data from the render target
            Texture2D resizedTexture = new Texture2D(originalTexture.GraphicsDevice, newWidth, newHeight);
            resizedTexture.SetData(data);

            return resizedTexture;

        }


    }

}
