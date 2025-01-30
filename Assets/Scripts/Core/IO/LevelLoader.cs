using UnityEngine;

namespace BEST.BomberMan.Core.IO
{
    public static class LevelLoader
    {
        public static Color[,] LoadLevel(TextAsset imageAsset)
        {
            var texture = new Texture2D(2, 2);
            texture.LoadImage(imageAsset.bytes);
            
            var pixels = texture.GetPixels();
            var pixelArray = new Color[texture.width, texture.height];

            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    Color pixel = pixels[y * texture.width + x];
                    pixelArray[x, y] = pixel;
                }
            }
            return pixelArray;
        }
    }
}
