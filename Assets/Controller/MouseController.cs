using System.Collections.Generic;
using Model;
using UnityEngine;
using Util;

namespace Controller {
    public class MouseController : MonoBehaviour {
        // Start is called before the first frame update

        public GameObject squareCursor;
        public GameObject circleCursorPrefab;

        private Vector3 _lastFramePosition;
        private Vector3 _dragStartPosition;
        private Vector3 _currFramePosition;

        private List<GameObject> _dragPreviewGoList;

        private Tile _tileUnderMouse;

        private void Start(){
            // circleCursor.SetActive(false);
            _dragPreviewGoList = new List<GameObject>();
        }

        // Update is called once per frame
        private void Update(){
            _tileUnderMouse = WorldController.Instance.GetTileAtWorldCoord(_currFramePosition);
            if (Camera.main == null) return;
            _currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _currFramePosition.z = 0;

            var tileUnderMouse = UpdateCursor();

            UpdateZoom();

            UpdateDragging();

            UpdateCameraMovement();
        }

        private void UpdateCameraMovement(){
            //Checks if right or middle mouse button is being pressed
            if (Input.GetMouseButton(2) || Input.GetMouseButton(1)) {
                var diff = _lastFramePosition - _currFramePosition;
                //Moves camera using the difference
                Camera.main.transform.Translate(diff);
            }

            //Input.mousePosition gets mouse position (in pixels, not in world position)
            //Needs to be transformed with ScreenToWorldPoint. Z level must be aligned so that it is visible to the camera
            if (Camera.main != null) _lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _lastFramePosition.z = 0;
        }

        private void UpdateDragging(){
            //Handle left mouse clicks
            //Start drag
            if (Input.GetMouseButtonDown(0)) {
                _dragStartPosition = _currFramePosition;
                squareCursor.SetActive(false);
            }

            var startX = Mathf.FloorToInt(_dragStartPosition.x);
            var endX = Mathf.FloorToInt(_currFramePosition.x);
            if (endX < startX) {
                //Change variables around if end is smaller than start
                (endX, startX) = (startX, endX);
            }

            var startY = Mathf.FloorToInt(_dragStartPosition.y);
            var endY = Mathf.FloorToInt(_currFramePosition.y);
            if (endY < startY) {
                //Change variables around if end is smaller than start
                (endY, startY) = (startY, endY);
            }

            //Clean up old drag previews
            foreach (var t in _dragPreviewGoList) {
                SimplePool.Despawn(t);
            }

            _dragPreviewGoList.Clear();

            if (Input.GetMouseButton(0)) {
                squareCursor.SetActive(false);
                for (var x = startX; x <= endX; x++) {
                    for (var y = startY; y <= endY; y++) {
                        var t = WorldController.Instance.World.GetTileAt(x, y);
                        if (t == null) continue;
                        //Display the building hint on top of this tile position
                        var go = SimplePool.Spawn(circleCursorPrefab, new Vector3(x, y, 0),
                            Quaternion.identity);
                        go.transform.SetParent(this.transform, true);
                        _dragPreviewGoList.Add(go);
                    }
                }
            }

            //End drag
            if (!Input.GetMouseButtonUp(0)) return;
            // circleCursor.SetActive(false);
            for (var x = startX; x <= endX; x++) {
                for (var y = startY; y <= endY; y++) {
                    var t = WorldController.Instance.World.GetTileAt(x, y);
                    if (t != null) {
                        t.Type = Tile.TileType.Ground;
                    }
                }
            }
        }

        private static void UpdateZoom(){
            var orthographicSize = Camera.main.orthographicSize;
            orthographicSize -= orthographicSize * Input.GetAxis("Mouse ScrollWheel") * 0.7f;
            Camera.main.orthographicSize = orthographicSize;
            Camera.main.orthographicSize = Mathf.Clamp(
                orthographicSize,
                3f,
                15f);
        }

        private Tile UpdateCursor(){
            var tileUnderMouse = WorldController.Instance.GetTileAtWorldCoord(_currFramePosition);
            if (tileUnderMouse != null) {
                squareCursor.SetActive(true);
                squareCursor.transform.position = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
            } else {
                squareCursor.SetActive(false);
            }

            return tileUnderMouse;
        }
    }
}