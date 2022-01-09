using Model;
using UnityEngine;

namespace Controller {
    public class WorldController : MonoBehaviour {
        private static WorldController _instance;
        public static WorldController Instance { get; protected set; }

        public Sprite groundSprite;
        public Sprite waterSprite;
        public Sprite treeSprite;
        public Sprite fruitSprite;

        public World  World { get; protected set; }

        // Start is called before the first frame update
        private void Start(){
            if (Instance != null) {
                Debug.LogError("There should not exist more than one world controller");
                return;
            }

            Instance = this;

            //Create a world with empty tiles
            World = new World();

            //Creates a GameObject for each of our tiles, so they appear visually
            for (var x = 0; x < World.Width; x++) {
                for (var y = 0; y < World.Height; y++) {
                    var tileData = World.GetTileAt(x, y);

                    var tileGo = new GameObject {
                        name = "Tile_" + x + "_" + y,
                        transform = {
                            position = new Vector3(tileData.X, tileData.Y, 0)
                        }
                    };
                    tileGo.transform.SetParent(this.transform, true);

                    //Add a sprite renderer, but dont bother setting a sprite, because all the tiles are empty
                    tileGo.AddComponent<SpriteRenderer>();
                    tileData.RegisterTileTypeChangedCallback((tile) => { OnTileTypeChanged(tile, tileGo); });
                }
            }

            World.RandomizeTiles();
            RandomizeTrees();
        }

        private void RandomizeTrees(){
            for (var x = 0; x < World.Width; x++) {
                for (var y = 0; y < World.Height; y++) {
                    if (!World.GetTileAt(x, y).Type.Equals(Tile.TileType.Ground)) continue;
                    if (Random.Range(0, 2) != 0) continue;
                    var tree = new GameObject("Tree_" + x + "_" + y) {
                        transform = {
                            position = new Vector3(x, y, 0)
                        }
                    };
                    tree.transform.SetParent(this.transform, true);
                    tree.AddComponent<SpriteRenderer>();
                    tree.GetComponent<SpriteRenderer>().sprite = treeSprite;
                    if (Random.Range(0, 2) != 0) continue;
                    var fruit = new GameObject("Fruit_" + x + "_" + y) {
                        transform = {
                            position = new Vector3(x, y, 0)
                        }
                    };
                    fruit.transform.SetParent(this.transform, true);
                    fruit.AddComponent<SpriteRenderer>();
                    fruit.GetComponent<SpriteRenderer>().sprite = fruitSprite;
                }
            }
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