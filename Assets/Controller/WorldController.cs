using Model;
using UnityEngine;

namespace Controller {
    public class WorldController : MonoBehaviour {
        public Sprite groundSprite;
        public Sprite waterSprite;

        private World _world;

        // Start is called before the first frame update
        private void Start(){
            //Create a world with empty tiles
            _world = new World();

            //Creates a GameObject for each of our tiles, so they appear visually
            for (var x = 0; x < _world.Width; x++) {
                for (var y = 0; y < _world.Height; y++) {
                    var tileData = _world.GetTileAt(x, y);

                    var tileGo = new GameObject {
                        name = "Tile_" + x + "_" + y,
                        transform = {
                            position = new Vector3(tileData.X, tileData.Y, 0)
                        }
                    };
                    tileGo.transform.SetParent(this.transform, true);
                    
                    //Add a sprite renderer, but dont bother setting a sprite, because all the tiles are empty
                    tileGo.AddComponent<SpriteRenderer>();
                    tileData.RegisterTileTypeChangedCallback((tile) => {
                        OnTileTypeChanged(tile, tileGo);
                    });
                }
            }

            _world.RandomizeTiles();
        }


        // Update is called once per frame
        private void Update(){
            
        }

        private void OnTileTypeChanged(Tile tileData, GameObject tileGo){
            tileGo.GetComponent<SpriteRenderer>().sprite = tileData.Type switch {
                Tile.TileType.Ground => groundSprite,
                Tile.TileType.Water => waterSprite,
                _ => groundSprite
            };
        }
    }
}