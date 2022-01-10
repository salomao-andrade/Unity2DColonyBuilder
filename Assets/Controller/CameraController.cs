using UnityEngine;

namespace Controller {
    public class CameraController : MonoBehaviour {

        private float _minX;
        private float _minY;
        private float _maxX;
        private float _maxY;
        private float _vertExtent;

        // Start is called before the first frame update
        private void Start() {
        
        }

        // Update is called once per frame
        void Update() {
            if (Camera.main != null) _vertExtent = Camera.main.orthographicSize;
            var horzExtent = _vertExtent * Screen.width / Screen.height;
        
            _minX = horzExtent - 0.0f / 2.0f;
            _maxX = 200.0f / 2.0f - horzExtent;
            _minY = _vertExtent - 0.0f / 2.0f;
            _maxY = 200.0f / 2.0f - _vertExtent;
        }
        void LateUpdate() {
            var v3 = transform.position;
            v3.x = Mathf.Clamp(v3.x, _minX, _maxX);
            v3.y = Mathf.Clamp(v3.y, _minY, _maxY);
            transform.position = v3;
        }
    }
}
