using Model;
using UnityEngine;

namespace Controller {
    public class CameraController : MonoBehaviour {
        private float _minX;
        private float _minY;
        private float _maxX;
        private float _maxY;
        private float _vertExtent;
        private float _mapSizeX;
        private float _mapSizeY;

        // Start is called before the first frame update
        private void Start(){ }

        // Update is called once per frame
        private void Update(){
            if (_mapSizeX == 0 || _mapSizeY == 0) {
                Debug.Log("changed Mapsize var");
                _mapSizeY = WorldController.Instance.World.Height;
                _mapSizeX = WorldController.Instance.World.Width;
            }

            if (Camera.main != null) _vertExtent = Camera.main.orthographicSize;
            var horzExtent = _vertExtent * Screen.width / Screen.height;

            _minX = horzExtent - 0.0f / 2.0f;
            _maxX = (_mapSizeX * 2) / 2.0f - horzExtent;
            _minY = _vertExtent - 0.0f / 2.0f;
            _maxY = (_mapSizeY * 2) / 2.0f - _vertExtent;
        }

        private void LateUpdate(){
            var v3 = transform.position;
            v3.x = Mathf.Clamp(v3.x, _minX, _maxX);
            v3.y = Mathf.Clamp(v3.y, _minY, _maxY);
            transform.position = v3;
        }
    }
}