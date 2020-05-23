using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
	public DepthCamera camera;

	private Scrollbar scrollbar;

	private void Start()
	{
		scrollbar = GetComponent<Scrollbar>();
	}

	public void scrollMoved()
	{
		camera.depthLevel = scrollbar.value * 3;
	}

}
