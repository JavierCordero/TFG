using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
	//Gyro
	private Gyroscope gyro;
	private GameObject cameraContainer;
	private Quaternion rotation;

	//cam
	private WebCamTexture cam;
	public RawImage background;
	public AspectRatioFitter fit;

	private bool arReady = false;

	private void Start()
	{
		//check if support
		//gyrpo
		if (!SystemInfo.supportsGyroscope)
		{
			Debug.Log("Devide does not have gyroscope");
			Application.Quit();
			//return;
		}

		//BackCamera

		//for(int i = 0; i < WebCamTexture.devices.Length; i++)
		//{
		//	if (!WebCamTexture.devices[i].isFrontFacing)
		//	{
				
		//		break;
		//	}
		//}

		cam = new WebCamTexture(Screen.width, Screen.height);

		//Si no tenemos camara...
		if (cam == null)
		{
			Debug.Log("No hay camara");
			Application.Quit();
		}

		//Todos los servicios están disponibles, así que hay que acticvarlos

		cameraContainer = new GameObject("CameraContainer");
		cameraContainer.transform.position = transform.position;
		transform.SetParent(cameraContainer.transform);

		//Activa el gyro
		gyro = Input.gyro;
		gyro.enabled = true;
		cameraContainer.transform.rotation = Quaternion.Euler(90, 0, 0);
		rotation = new Quaternion(0, 0, 1, 0);

		//Activa la camara y la pone en la textura
		cam.Play();
		background.texture = cam;

		arReady = true;
	}

	private void Update()
	{
		if (arReady)
		{
			//Update de la camara
			float ratio = (float)cam.width / (float)cam.height;
			fit.aspectRatio = ratio;

			float scaleY = cam.videoVerticallyMirrored ? -1.0f : 1.0f;
			background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

			int orient = -cam.videoRotationAngle;
			background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

			// Update gyro
			transform.localRotation = gyro.attitude * rotation;
		}
	}
}
