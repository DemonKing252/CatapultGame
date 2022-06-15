using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public RectTransform levelBoundary;
    public Transform followTarget;

    private CatapultController catapult;

    // Start is called before the first frame update
    void Start()
    {
        catapult = FindObjectOfType<CatapultController>();
        /// Log for the purposes of checking if the boundary limits are working
        //Debug.Log("Boundary -X: " + (levelBoundary.position.x - levelBoundary.rect.width / 2 + (((float)Camera.main.orthographicSize) * Camera.main.aspect)) + "\n" +
        //          "Boundary +X: " + (levelBoundary.position.x + levelBoundary.rect.width / 2 - (((float)Camera.main.orthographicSize) * Camera.main.aspect)) + "\n" +
        //          "Boundary -Y: " + (levelBoundary.position.y - levelBoundary.rect.height / 2 + ((float)Camera.main.orthographicSize)) + "\n" +
        //          "Boundary +Y: " + (levelBoundary.position.y + levelBoundary.rect.height / 2 - ((float)Camera.main.orthographicSize)) + "\n");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!catapult.ballHeld)
            ClampCameraPosition();

        /// Checking camera position to match with the boundary logs
        //Debug.Log("Camera Position X: " + transform.position.x + "\n" +
        //          "Camera Position Y: " + transform.position.y);
    }

    void ClampCameraPosition()
    {
        // The position of the camera will be based on the positon of the ball falling within the preset boundary of the given level.
        // It will grab the width and height of the levelBoundary variable in the level then will offset the camera width and height
        transform.position = new Vector3(Mathf.Clamp(followTarget.position.x, levelBoundary.position.x - levelBoundary.rect.width / 2 + (((float)Camera.main.orthographicSize) * Camera.main.aspect),
                                                                              levelBoundary.position.x + levelBoundary.rect.width / 2 - (((float)Camera.main.orthographicSize) * Camera.main.aspect)),
                                         Mathf.Clamp(followTarget.position.y, levelBoundary.position.y - levelBoundary.rect.height / 2 + ((float)Camera.main.orthographicSize),
                                                                              levelBoundary.position.y + levelBoundary.rect.height / 2 - ((float)Camera.main.orthographicSize)),
                                                        transform.position.z);

    }
}
