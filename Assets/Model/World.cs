using UnityEngine;

namespace Model
{
    public class World {
        Tile[,] tiles;

        public int Width { get; }

        public int Height { get; }


        public World(int width = 100, int height = 100) {
            this.Width = width;
            this.Height = height;

            tiles = new Tile[width, height];

            for (var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    tiles[x, y] = new Tile(this, x, y);
                }
            }
            Debug.Log("World Created with " + (width * height) + " tiles.");
        }

        public void RandomizeTiles() {
            Debug.Log("RandomizeTiles");
            for (var x = 0; x < Width; x++) {
                for (var y = 0; y < Height; y++)
                {
                    tiles[x, y].Type = Random.Range(0,3) == 0 ? Tile.TileType.Ground : Tile.TileType.Water;
                }
            }
        }

        public Tile GetTileAt(int x, int y) {
            if (x <= Width && x >= 0 && y <= Height && y >= 0) return tiles[x, y];
            Debug.LogError("Tile (" + x + ", " + y + ") is out of range");
            return null;
        }
    }
}
