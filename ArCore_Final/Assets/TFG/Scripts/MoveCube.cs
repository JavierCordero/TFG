using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
	public GameObject Cube, Camera;

	private void Update()
	{
		foreach (Touch touch in Input.touches)
		{
			if (touch.phase == TouchPhase.Began)
			{
				Instantiate(Cube, Camera.transform.position, Quaternion.identity);
			}
		}

		if(Input.GetMouseButtonDown(0))
			Instantiate(Cube, Camera.transform.position, Quaternion.identity);

	}

}
