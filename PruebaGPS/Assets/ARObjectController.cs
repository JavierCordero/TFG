using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Mapbox.Unity.Map;

public class ARObjectController : MonoBehaviour
{
	public Mapbox.Unity.Location.DeviceLocationProvider deviceLocation;

	[SerializeField]
	private AbstractMap _map;

	public GameObject goToInstantiate;



	public void placeObjectInARWORLD()
	{
		double lat = deviceLocation.CurrentLocation.LatitudeLongitude.x;
		double lon = deviceLocation.CurrentLocation.LatitudeLongitude.y;

		Vector3 _targetPosition = _map.Root.TransformPoint(Conversions.GeoToWorldPosition(lat, lon, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz());
		Instantiate(goToInstantiate, _targetPosition, Quaternion.identity);
		
	}
}
