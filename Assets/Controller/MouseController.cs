using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {
    // Start is called before the first frame update

    private Vector3 _lastFramePosition;
    void Start() {
        
    }

    // Update is called once per frame
    private void Update(){
        //Checks if right or middle mouse button is being pressed
        if (Camera.main == null) return;
        if (Input.GetMouseButton(2) || Input.GetMouseButton(1)) {
            var diff = _lastFramePosition - Camera.main.ScreenToWorldPoint(Input.mousePosition);;
            //Moves camera using the difference
            Camera.main.transform.Translate(diff);
        }
        //Input.mousePosition gets mouse position (in pixels, not in world position)
        //Needs to be transformed with ScreenToWorldPoint
        if (Camera.main != null) _lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
