using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class VisualizationTest : MonoBehaviour
{
	public GameObject[] HideObjects;
	public GameObject TestButton, PointCloudButton, NeedPointCloudWarning;

	private PointCloudEditor PCE;
	private PointCloudController PCC;
	private List<Vector3> PointCloud;
	private ScreenShot SS;

	public GameObject[] Shapes;
	public float[] Sizes;
	[HideInInspector]
	public int actualShape = 0;
	[HideInInspector]
	public int actualSize = 0;

	public void StartVisualizationTest()
	{
		TestButton.GetComponent<Button>().enabled = false;

		PCE = FindObjectOfType<PointCloudEditor>();
		PCC = FindObjectOfType<PointCloudController>();
		SS = FindObjectOfType<ScreenShot>();

		if (PCE)
		{
			PointCloud = PCE.getPointCloud();
			PCC.DisablePointCloud();

			PointCloudButton.GetComponent<ButtonController>().OnClick();

			Invoke("TrulyStart", 1);
		}

		else
		{
			foreach (GameObject g in HideObjects)
				g.SetActive(false);

			NeedPointCloudWarning.SetActive(true);
			Invoke("DisableWarning", 2);

		}

	}

	private void DisableWarning()
	{
		NeedPointCloudWarning.SetActive(false);

		EndVisualizationTest();

	} 

	private void TrulyStart()
	{
		foreach (GameObject g in HideObjects)
			g.SetActive(false);

		StartCoroutine(StartTest());		
	}

	IEnumerator StartTest()
	{
		int maxNumTest = Shapes.Length * Sizes.Length;
		int currentTest = 1;

		for(int i = 0; i < Shapes.Length; i++)
		{
			for(int j = 0; j < Sizes.Length; j++)
			{
				List<GameObject> aux = new List<GameObject>();

				for(int k = 0; k < PointCloud.Count; k++)
				{
					aux.Add(Instantiate(Shapes[i], PointCloud[i], Quaternion.identity));
					aux[aux.Count - 1].transform.localScale = new Vector3(Sizes[j], Sizes[j], Sizes[j]);
					yield return null;
				}

				SS.VisualizationTestScreenShots(Shapes[i].name, Sizes[j].ToString());

				foreach (GameObject g in aux)
					Destroy(g);

				currentTest++;
			}
		}

		EndVisualizationTest();
		
	}

	private void EndVisualizationTest()
	{
		foreach (GameObject g in HideObjects)
			g.SetActive(true);

		TestButton.GetComponent<Button>().enabled = true;
		TestButton.GetComponent<ButtonController>().OnClick();

	}

}
