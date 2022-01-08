using System;

namespace Model
{
    public class Tile {
        public enum TileType { Ground, Mountain, Water }

        private TileType _type = TileType.Ground;

        private Action<Tile> cbTileTypeChanged;
        

        public TileType Type {
            get => _type;
            set {
                _type = value;
                //Call the callback and let things know we've changed.
                cbTileTypeChanged?.Invoke(this);
            }
        }

        private LooseObject _looseObject;
        private InstalledObject _installedObject;

        private World _world;

        public int X { get; }

        public int Y { get; }

        public Tile(World world, int x, int y) {
            this._world = world;
            this.X = x;
            this.Y = y;
        }

        public void RegisterTileTypeChangedCallback(Action<Tile> callback){
            cbTileTypeChanged += callback;
        }
        public void UnregisterTileTypeChangedCallback(Action<Tile> callback){
            cbTileTypeChanged -= callback;
        }

    }
}
