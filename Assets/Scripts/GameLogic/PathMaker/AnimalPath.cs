using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalPath : MonoBehaviour
{
	[SerializeField] public Transform startPoint;
	[SerializeField] private AnimalPathElement[] points;
	[SerializeField] public Transform lastPoint;

	[SerializeField] private GameObject flowerWayItem;

	public float flowerWayInterval = 0.5f;
	public float flowerSkipRadius = 0.5f;
	public float flowerWayWaterLevel = 0;

	public AnimalPathElement[] Points => points;

	private int pointIndex;
	private bool increasingBuildWay = true;

	private List<FlowerWayItem> flowerWay = new List<FlowerWayItem>();

	private bool pathDone = false;

	public event EventHandler WayIsBult;

	private bool _stopped = true;

	public int PointsCount()
	{
		return points.Length;
	}

	public void StopBuilding()
	{
		_stopped = true;
	}

	public void RestartBuilding()
	{
		pathDone = false;
		_stopped = false;
	}

	private AnimalPathElement[] expectedPoints;

	// Start is called before the first frame update
	void Start()
	{
		flowerWay.Clear();

		foreach (var point in points)
		{
			point.SetPathRoot(this);
		}
	}

	public void DisappearWay()
	{
		foreach (var item in flowerWay)
		{
			item.Disappear();
		}
		flowerWay.Clear();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void TurnIncreaseBuildWay()
	{
		builtPath.Clear();
		increasingBuildWay = true;
		expectedPoints = points.OrderBy(x => x.pointName).ToArray();
		pointIndex = 0;
	}

	public void TurnDecreaseBuildWay()
	{
		builtPath.Clear();
		increasingBuildWay = false;
		expectedPoints = points.OrderByDescending(x => x.pointName).ToArray();
		pointIndex = 0;
	}

	public List<AnimalPathElement> builtPath = new List<AnimalPathElement>();
	public Vector3 finishPoint;

	public void numberIsPressed(AnimalPathElement p)
	{
		if (_stopped) { Debug.LogWarning("Path building is stopped"); return; }
		if(pathDone) { Debug.LogWarning("Path building is done"); return; }

		if (p.pointName == expectedPoints[pointIndex].pointName)
		{
			p.GoodHit();
			//p.SetShine(true);

			builtPath.Add(p);

			if (pointIndex >= 1)
			{
				addFlowerWaySection(expectedPoints[pointIndex], expectedPoints[pointIndex - 1],
					flowerWayInterval, flowerSkipRadius, flowerWayWaterLevel);
			}

			pointIndex++;

			if(pointIndex == expectedPoints.Length)
			{
				pathDone = true;

				if(increasingBuildWay) finishPoint = lastPoint.position;
				else finishPoint = startPoint.position;

				WayIsBult?.Invoke(this, EventArgs.Empty);
			}
		}
		else
		{
			p.WrongHit();
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(startPoint.position, 0.1f);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(lastPoint.position, 0.1f);

		Gizmos.color = Color.magenta;
		for (int i = 0; i < points.Length - 1; i++) 
		{
			Gizmos.DrawLine(points[i].gameObject.transform.position, points[i + 1].gameObject.transform.position);
		}
	}

	private void addFlowerWaySection(AnimalPathElement a, AnimalPathElement b, float interval, float skipRadius, float waterLevel)
	{
		var points = GenerateFlowerPoints(a.transform.position, b.transform.position,
			interval, skipRadius, waterLevel);

		foreach (var point in points)
		{
			var wayItem = Instantiate(flowerWayItem, point, Quaternion.identity);
			flowerWay.Add(wayItem.GetComponent<FlowerWayItem>());
		}
	}

	private List<Vector3> GenerateFlowerPoints(Vector3 pointA, Vector3 pointB, float interval, float skipRadius, float waterLevel)
	{
		var generatedPoints = new List<Vector3>();
		if (pointA == null || pointB == null) return null;

		pointA.y = waterLevel;
		pointB.y = waterLevel;

		Vector3 centerPoint = (pointA + pointB) / 2.0f;
		Vector3 directionAtoB = (pointB - pointA).normalized;
		Vector3 directionBtoA = (pointA - pointB).normalized;

		float halfDistance = Vector3.Distance(pointA, centerPoint);
		int numberOfPoints = Mathf.FloorToInt(halfDistance / interval);

		for (int i = 0; i <= numberOfPoints; i++)
		{
			Vector3 pointTowardsA = centerPoint + directionBtoA * (i * interval);
			Vector3 pointTowardsB = centerPoint + directionAtoB * (i * interval);

			if (Vector3.Distance(pointTowardsA, pointA) > skipRadius && !generatedPoints.Contains(pointTowardsA))
			{
				generatedPoints.Add(pointTowardsA);
			}

			if (Vector3.Distance(pointTowardsB, pointB) > skipRadius && !generatedPoints.Contains(pointTowardsB))
			{
				generatedPoints.Add(pointTowardsB);
			}
		}

		return generatedPoints;
	}
}
