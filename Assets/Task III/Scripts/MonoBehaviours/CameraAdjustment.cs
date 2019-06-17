using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjustment : MonoBehaviour
{
    public Bootstrap Bootstrapper;
	private Vector3 mouseOrigin;	
	private bool isRotating;
    private bool isScrolling;

    void Update () 
	{
		if(Input.GetMouseButtonDown(1))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isRotating = true;
		}

		if(Input.mouseScrollDelta.y != 0f)
		{
            isScrolling = true;
        }
	
		if (!Input.GetMouseButton(1)) isRotating = false;
		if(Input.mouseScrollDelta.y == 0) isScrolling = false;

        // Rotate cube along X and Y axis
        if (isRotating)
		{
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            Bootstrapper.SpaceshipPrefab.transform.Rotate(Vector3.up, -10f * pos.x);
			Bootstrapper.SpaceshipPrefab.transform.Rotate(Vector3.right, 10f * pos.y);
        }

		if(isScrolling)
		{
            Camera.main.fieldOfView += 10f * Input.mouseScrollDelta.y;
        }
		
	}
}
