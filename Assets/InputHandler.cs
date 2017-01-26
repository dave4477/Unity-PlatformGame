using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

    [SerializeField]
    GameObject player;

    float speed = 1;

	// Use this for initialization
	void Start () {
	
	}

	
	void Update()
    {
    }

    private void HandleTouch(int touchFingerId, Vector3 touchPosition, TouchPhase touchPhase)
    {
        Ray screenRay = Camera.main.ScreenPointToRay(touchPosition); 

        RaycastHit hit;

        switch (touchPhase)
        {
            case TouchPhase.Began:
                
                if (Physics.Raycast(screenRay, out hit))
                {
                    print("began on:" +hit.collider.gameObject.name);
                }

                player.GetComponentInChildren<PlayerController>().WalkRight();
                // TODO
                break;
            case TouchPhase.Moved:
                if (Physics.Raycast(screenRay, out hit))
                {
                    print("moved on:"  +hit.collider.gameObject.name);
                }
                player.GetComponentInChildren<PlayerController>().WalkRight();

                // TODO
                break;
            case TouchPhase.Ended:
                if (Physics.Raycast(screenRay, out hit))
                {
                    print("Ended on:"  +hit.collider.gameObject.name);
                }

                // TODO
                break;
        }
    }
}
