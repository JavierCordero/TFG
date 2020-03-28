using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
	Vector3 v = new Vector3(0, 0, 1);

    public void cubeForward()
	{
		transform.localPosition += v;
	}

	public void cubeBackward()
	{
		transform.localPosition -= v;
	}

}
