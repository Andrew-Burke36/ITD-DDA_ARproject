using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class TouchInput : MonoBehaviour
{
    public GameObject objectToPlace;
    public ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Update()
    {
        // 1️⃣ Check if the user has touched the screen
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // 2️⃣ Check if this is a new tap
            if (touch.phase == TouchPhase.Began)
            {
                // 3️⃣ Fire a raycast from the tap position into the AR world
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    // 4️⃣ Get the hit position (Pose)
                    Pose hitPose = hits[0].pose;

                    // 5️⃣ Place or move your object
                    Instantiate(objectToPlace, hitPose.position, hitPose.rotation);
                }
            }
        }
    }
}
