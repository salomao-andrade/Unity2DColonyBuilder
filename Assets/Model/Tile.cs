using System;

namespace Model
{
    public class Tile {
        public enum TileType { Empty, Ground, Mountain, Water }

        private TileType _type = TileType.Empty;

        private Action<Tile> cbTileTypeChanged;
        
        public TileType Type {
            get => _type;
            set {
                var oldType = _type;
                _type = value;
                //Call the callback and let things know we've changed.
                if (cbTileTypeChanged != null && oldType != _type)
                    cbTileTypeChanged(this);
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
