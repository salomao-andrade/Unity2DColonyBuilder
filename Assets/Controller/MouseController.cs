using Model;
using UnityEngine;

namespace Controller {
    public class MouseController : MonoBehaviour {
        // Start is called before the first frame update

        public GameObject squareCursor;
        public GameObject circleCursor;

        private Vector3 _lastFramePosition;
        private Vector3 _dragStartPosition;


        private void Start(){
            circleCursor.SetActive(false);
        }

        // Update is called once per frame
        private void Update(){
            if (Camera.main == null) return;
            var currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currFramePosition.z = 0;

            var tileUnderMouse = GetTileAtWorldCoord(currFramePosition);
            if (tileUnderMouse != null) {
                squareCursor.SetActive(true);
                squareCursor.transform.position = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
            } else {
                squareCursor.SetActive(false);
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0f) {
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - Input.GetAxis("Mouse ScrollWheel"),
                                                        2f,
                                                        10f);
            }

            //Handle left mouse clicks
            //Start drag
            if (Input.GetMouseButtonDown(0)) {
                _dragStartPosition = currFramePosition;
                squareCursor.SetActive(false);
                circleCursor.SetActive(true);
                circleCursor.transform.position = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
            }

            if (Input.GetMouseButton(0)) {
                squareCursor.SetActive(false);
                circleCursor.SetActive(true);
                var newCircleSR = circleCursor.GetComponent<SpriteRenderer>();
                newCircleSR.sortingOrder = 0;
                newCircleSR.sortingLayerName = "TileUI";
                circleCursor.transform.position = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
            }

            //End drag
            if (Input.GetMouseButtonUp(0)) {
                circleCursor.SetActive(false);
                var startX = Mathf.FloorToInt(_dragStartPosition.x);
                var endX = Mathf.FloorToInt(currFramePosition.x);
                if (endX < startX) {
                    //Change variables around if end is smaller than start
                    (endX, startX) = (startX, endX);
                }

                var startY = Mathf.FloorToInt(_dragStartPosition.y);
                var endY = Mathf.FloorToInt(currFramePosition.y);
                if (endY < startY) {
                    //Change variables around if end is smaller than start
                    (endY, startY) = (startY, endY);
                }

                for (var x = startX; x <= endX; x++) {
                    for (int y = startY; y <= endY; y++) {
                        var t = WorldController.Instance.World.GetTileAt(x, y);
                        if (t != null) {
                            t.Type = Tile.TileType.Ground;
                        }
                    }
                }
            }

            //Checks if right or middle mouse button is being pressed
            if (Input.GetMouseButton(2) || Input.GetMouseButton(1)) {
                var diff = _lastFramePosition - currFramePosition;
                //Moves camera using the difference
                Camera.main.transform.Translate(diff);
            }

            //Input.mousePosition gets mouse position (in pixels, not in world position)
            //Needs to be transformed with ScreenToWorldPoint. Z level must be aligned so that it is visible to the camera
            if (Camera.main != null) _lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _lastFramePosition.z = 0;
        }

        private static Tile GetTileAtWorldCoord(Vector3 coord){
            var x = Mathf.FloorToInt(coord.x);
            var y = Mathf.FloorToInt(coord.y);

            var world = WorldController.Instance.World;

            return world.GetTileAt(x, y);
        }
    }
}