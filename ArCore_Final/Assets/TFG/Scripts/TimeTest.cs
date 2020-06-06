using UnityEngine;
using UnityEngine.UI;

public class TimeTest : MonoBehaviour
{
	public Text TimeTextCanvas;

	private PointCloudController PCC;
	private float startTime, elapsedTime;

	private void Start()
	{
		PCC = FindObjectOfType<PointCloudController>();
		TimeTextCanvas.text = "0.0s";
	}

	public void StartCountingTime()
	{
		PCC.EnablePointCloud();
		TimeTextCanvas.text = "Timing";
		startTime = Time.time;
	}

	public void EndCountingTime()
	{
		PCC.DisablePointCloud();
		elapsedTime = Time.time - startTime;
		TimeTextCanvas.text = elapsedTime.ToString() + "s";

	}
}
