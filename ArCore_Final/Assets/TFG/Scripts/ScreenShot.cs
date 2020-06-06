using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
	PointCloudEditor pce;
	VisualizationTest VT;
	public GameObject PhotoTakenAdvise;
	public GameObject Warning;

	public float ButtonCooldown = 1;

	private bool CanTakeAnotherScreenshot = true;

	// Start is called before the first frame update
	void Start()
    {
		VT = FindObjectOfType<VisualizationTest>();
	}

	public void TakeScreenShot()
	{
		if (CanTakeAnotherScreenshot)
		{
			pce = FindObjectOfType<PointCloudEditor>();
			if (pce)
			{
				CanTakeAnotherScreenshot = false;
				ScreenCapture.CaptureScreenshot("Screenshot_" + pce.PrefabsToInstantiate[pce.actualPrefabToInstantiate].name + "_" + pce.MaxPointsToAddPerFrame[pce.actualMaxPointPerFrameCount].ToString() + "_" + pce.m_MaxPointCount[pce.actualMaxPointCount].ToString() + ".jpg");
				PhotoTakenAdvise.SetActive(true);
				Invoke("FinishCooldown", ButtonCooldown);
			}

			else
			{
				CanTakeAnotherScreenshot = false;
				Warning.SetActive(true);
				Invoke("cancelWarning", ButtonCooldown);
				Invoke("FinishCooldown", ButtonCooldown);
			}
		}
	}

	private void cancelWarning()
	{
		Warning.SetActive(false);
	}

	public void VisualizationTestScreenShots(string shape, string size)
	{
		ScreenCapture.CaptureScreenshot("Screenshot_Shape_" + shape + "_Size_" + size + ".jpg");
	}

	private void FinishCooldown()
	{
		CanTakeAnotherScreenshot = true;
		PhotoTakenAdvise.SetActive(false);
	}

}
