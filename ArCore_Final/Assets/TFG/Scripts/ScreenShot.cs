using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
	PointCloudEditor pce;
	public GameObject PhotoTakenAdvise;

	public float ButtonCooldown = 1;

	private bool CanTakeAnotherScreenshot = true;

	// Start is called before the first frame update
	void Start()
    {
		pce = FindObjectOfType<PointCloudEditor>();
	}

	public void TakeScreenShot()
	{
		if (CanTakeAnotherScreenshot)
		{
			CanTakeAnotherScreenshot = false;
			ScreenCapture.CaptureScreenshot("Screenshot_" + pce.PrefabsToInstantiate[pce.actualPrefabToInstantiate].name + "_" + pce.MaxPointsToAddPerFrame.ToString() + "_" + pce.m_MaxPointCount.ToString() + ".jpg");
			PhotoTakenAdvise.SetActive(true);
			Invoke("FinishCooldown", ButtonCooldown);

			Debug.Log("CENTINELA: " + Application.persistentDataPath);

		}
	}

	private void FinishCooldown()
	{
		CanTakeAnotherScreenshot = true;
		PhotoTakenAdvise.SetActive(false);
	}

}
