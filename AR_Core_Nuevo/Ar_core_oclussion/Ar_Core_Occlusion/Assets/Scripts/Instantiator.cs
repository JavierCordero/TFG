using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Instantiator : MonoBehaviour
{
	public GameObject instatiateObject;

	private ARRaycastManager arRaycastManager;
	private Vector2 touchPosition;
	private List<ARRaycastHit> hits;

	private void Awake()
	{
		hits = new List<ARRaycastHit>();
		arRaycastManager = GetComponent<ARRaycastManager>();
	}

	// Update is called once per frame
	void Update()
    {
        if(Input.touchCount > 0)
		{
			hits.Clear();

			Touch touch = Input.GetTouch(0);

			touchPosition = touch.position;

			if(touch.phase == TouchPhase.Began)
			{
				
				if(arRaycastManager.Raycast(touchPosition, hits))
				{
					Pose hitPose = hits[0].pose;
					Instantiate(instatiateObject, hitPose.position, Quaternion.identity);

				}
			}
		}
    }
}
