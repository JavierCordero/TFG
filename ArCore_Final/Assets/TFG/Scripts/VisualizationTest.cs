using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class VisualizationTest : MonoBehaviour
{
	public GameObject[] HideObjects;
	public GameObject TestButton, PointCloudButton, NeedPointCloudWarning, Boxes;
	public GoogleARCore.ARCoreBackgroundRenderer BGRenderer;

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
		List<GameObject> aux = new List<GameObject>();

		for (int k = 0; k < PointCloud.Count; k++)
		{
			aux.Add(Instantiate(Shapes[0], PointCloud[k], Quaternion.identity));
			//yield return null;
		}

		for (int i = 0; i < Shapes.Length; i++)
		{
			Mesh m = Shapes[i].GetComponent<MeshFilter>().sharedMesh;

			for (int k = 0; k < PointCloud.Count; k++)
			{
				aux[k].GetComponent<MeshFilter>().mesh = m;
			}

			for (int j = 0; j < Sizes.Length; j++)
			{
				Vector3 size = new Vector3(Sizes[j], Sizes[j], Sizes[j]);

				for (int k = 0; k < PointCloud.Count; k++)
				{
					aux[k].transform.localScale = size;
				}

				yield return new WaitForEndOfFrame();

				SS.VisualizationTestScreenShots(Shapes[i].name, Sizes[j].ToString());

				yield return new WaitForEndOfFrame();

			}
		}

		foreach (GameObject g in aux)
			Destroy(g);

		Boxes.SetActive(false);

		yield return new WaitForEndOfFrame();

		SS.VisualizationTestScreenShots("Original", "Image");

		yield return new WaitForEndOfFrame();

		Boxes.SetActive(true);

		BGRenderer.enabled = false;

		yield return new WaitForEndOfFrame();

		SS.VisualizationTestScreenShots("NoBG", "Cubes");

		yield return new WaitForEndOfFrame();

		BGRenderer.enabled = true;

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
