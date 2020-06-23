using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Geocoding;
using Mapbox.Unity;
using Mapbox.Directions;

public class ARObjectController : MonoBehaviour
{
	public Mapbox.Unity.Location.DeviceLocationProvider deviceLocation;

	[SerializeField]
	private AbstractMap _map;

	public GameObject goToInstantiate;

	private Vector2d originalCoordinates, finalCoordinates;

    #region MapboxStuff

    List<Feature> _features;
	ForwardGeocodeResource _resource;

	private Directions _directions;

	[SerializeField]
	Transform[] _waypoints;

	#endregion

	LineRenderer lr;

	private List<Vector3> pathsPositions;

	private void Start()
	{
		_resource = new ForwardGeocodeResource("");
		_directions = MapboxAccess.Instance.Directions;
		lr = gameObject.AddComponent<LineRenderer>();
		pathsPositions = new List<Vector3>();

		StartPath();
	}

	private void StartPath()
    {
		HandleUserInput("Jesus Goldero 9 Madrid Spain");
	}

	public void placeObjectInARWORLD(Vector2d coordinates)
	{
		Vector3 _targetPosition = _map.Root.TransformPoint(Conversions.GeoToWorldPosition(coordinates.x, coordinates.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz());
		Instantiate(goToInstantiate, _targetPosition, Quaternion.identity);
		pathsPositions.Add(_targetPosition);		
	}

    #region FinalPointCalculator
    public void HandleUserInput(string searchString)
	{
		_features = new List<Feature>();

		if (!string.IsNullOrEmpty(searchString))
		{
			_resource.Query = searchString;
			MapboxAccess.Instance.Geocoder.Geocode(_resource, HandleGeocoderResponse);
		}
	}

	void HandleGeocoderResponse(ForwardGeocodeResponse res)
	{
		//null if no internet connection
		if (res != null)
		{
			//null if invalid token
			if (res.Features != null)
			{
				_features = res.Features;

				if (_features.Count > 0) OriginAndDestinationCalc();
				else Debug.Log("No se ha encontrado ninguna localización.");
			}
		}
	}
	#endregion

	#region IntermediatePointsCalculator

	private void OriginAndDestinationCalc()
    {
		originalCoordinates.x = deviceLocation.CurrentLocation.LatitudeLongitude.x;
		originalCoordinates.y = deviceLocation.CurrentLocation.LatitudeLongitude.y;

		finalCoordinates.x = _features[0].Center.x;
		finalCoordinates.y = _features[0].Center.y;

		Query(originalCoordinates, finalCoordinates);
	}

	void Query(Vector2d originCoordinates, Vector2d destinyCoordinates)
	{

		var wp = new Vector2d[2];

		wp[0] = originCoordinates;
		wp[1] = destinyCoordinates;

		var _directionResource = new DirectionResource(wp, RoutingProfile.Walking);
		_directionResource.Steps = true;
		_directions.Query(_directionResource, HandleDirectionsResponse);
	}

	void HandleDirectionsResponse(DirectionsResponse response)
	{
		if (response == null || null == response.Routes || response.Routes.Count < 1)
		{
			return;
		}

		DrawARObjects(response.Routes[0].Legs);
	}
	#endregion

	void DrawARObjects(List<Leg> legs)
    {
		Vector2d v;

		lr.positionCount = 0;

		foreach (Leg l in legs)
		{
			lr.positionCount += l.Steps.Count;
		}

		Debug.Log(lr.positionCount);

		foreach (Leg l in legs)
        {
			foreach(Step s in l.Steps)
            {
				v.x = s.Maneuver.Location.x;
				v.y = s.Maneuver.Location.y;

				placeObjectInARWORLD(v);
			}
        }

		lr.SetPositions(pathsPositions.ToArray());
	}
}
