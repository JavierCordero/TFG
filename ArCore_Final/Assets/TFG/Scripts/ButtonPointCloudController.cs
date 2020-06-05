using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPointCloudController : MonoBehaviour
{
	private PointCloudController PCC;
	private bool pointCloudEnable = false;
	void Start()
	{
		PCC = FindObjectOfType<PointCloudController>();
	}

	public void OnClick()
	{
		if (!pointCloudEnable)
		{
			pointCloudEnable = true;
			PCC.EnablePointCloud();
		}

		else
		{
			pointCloudEnable = false;
			PCC.DisablePointCloud();
		}
	}
}
