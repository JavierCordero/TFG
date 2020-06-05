using UnityEngine;

public class PointCloudController : MonoBehaviour
{
	public PointCloudEditor myPointCloud;

	public void EnablePointCloud()
	{
		myPointCloud.gameObject.SetActive(true);
	}

	public void DisablePointCloud()
	{
		myPointCloud.gameObject.SetActive(false);
	}

}
