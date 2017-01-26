using UnityEngine;
using System.Collections.Generic;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    Camera parallaxCamera;

    [SerializeField]
    List<GameObject> parallaxLayers;

    float offsetYBackground = 50f;
	
	// Update is called once per frame
	void Update ()
    {
        if (parallaxLayers[0])
        {
            Vector3 CameraTransformFarBack = new Vector3(parallaxCamera.transform.position.x - 200, parallaxCamera.transform.position.y, parallaxLayers[0].transform.position.z);
            parallaxLayers[0].transform.position = -(CameraTransformFarBack / 20);
        }
        if (parallaxLayers[1])
        {
            Vector3 CameraTransformBack = new Vector3(parallaxCamera.transform.position.x - 300, parallaxCamera.transform.position.y + offsetYBackground, parallaxLayers[1].transform.position.z);
            parallaxLayers[1].transform.position = -(CameraTransformBack / 10);
        }
        if (parallaxLayers[2])
        {
            Vector3 CameraTransformFront = new Vector3(parallaxCamera.transform.position.x, parallaxLayers[2].transform.position.y + 14, parallaxLayers[2].transform.position.z);
            parallaxLayers[2].transform.position = -(CameraTransformFront / 2);
        }
        //parallaxLayers[1].transform.position = CameraTransform;
    }
}
