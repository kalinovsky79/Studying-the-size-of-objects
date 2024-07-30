using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowerLine
{
	//void GeneratePoints()
	//{
	//	generatedPoints.Clear();
	//	Vector3 direction = (pointB.position - pointA.position).normalized;
	//	float distance = Vector3.Distance(pointA.position, pointB.position);
	//	int numberOfPoints = Mathf.FloorToInt(distance / interval);

	//	for (int i = 0; i <= numberOfPoints; i++)
	//	{
	//		Vector3 point = pointA.position + direction * (i * interval);
	//		generatedPoints.Add(point);
	//		Debug.Log($"Generated Point: {point}");
	//	}

	//	// Add the final pointB position if it's not exactly on the interval
	//	if (Vector3.Distance(generatedPoints[generatedPoints.Count - 1], pointB.position) > 0.01f)
	//	{
	//		generatedPoints.Add(pointB.position);
	//		Debug.Log($"Added final point: {pointB.position}");
	//	}
	//}

	//void GeneratePoints()
	//{
	//	generatedPoints.Clear();
	//	if (pointA == null || pointB == null) return;

	//	Vector3 direction = (pointB.position - pointA.position).normalized;
	//	float distance = Vector3.Distance(pointA.position, pointB.position);
	//	int numberOfPoints = Mathf.FloorToInt(distance / interval);

	//	for (int i = 0; i < numberOfPoints; i++)
	//	{
	//		Vector3 startPoint = pointA.position + direction * (i * interval);
	//		Vector3 endPoint = pointA.position + direction * ((i + 1) * interval);
	//		Vector3 midPoint = (startPoint + endPoint) / 2.0f;
	//		generatedPoints.Add(midPoint);
	//	}

	//	// Add the final midpoint if there's remaining distance
	//	if (numberOfPoints > 0)
	//	{
	//		Vector3 finalStartPoint = pointA.position + direction * (numberOfPoints * interval);
	//		Vector3 finalEndPoint = pointB.position;
	//		Vector3 finalMidPoint = (finalStartPoint + finalEndPoint) / 2.0f;
	//		generatedPoints.Add(finalMidPoint);
	//	}
	//}

	//void GeneratePoints()
	//{
	//	generatedPoints.Clear();
	//	if (pointA == null || pointB == null) return;

	//	Vector3 centerPoint = (pointA.position + pointB.position) / 2.0f;
	//	Vector3 directionAtoB = (pointB.position - pointA.position).normalized;
	//	Vector3 directionBtoA = (pointA.position - pointB.position).normalized;

	//	float halfDistance = Vector3.Distance(pointA.position, centerPoint);
	//	int numberOfPoints = Mathf.FloorToInt(halfDistance / interval);

	//	for (int i = 0; i <= numberOfPoints; i++)
	//	{
	//		Vector3 pointTowardsA = centerPoint + directionBtoA * (i * interval);
	//		Vector3 pointTowardsB = centerPoint + directionAtoB * (i * interval);

	//		if (!generatedPoints.Contains(pointTowardsA))
	//		{
	//			generatedPoints.Add(pointTowardsA);
	//		}

	//		if (!generatedPoints.Contains(pointTowardsB))
	//		{
	//			generatedPoints.Add(pointTowardsB);
	//		}
	//	}

	//	// Ensure the endpoints are included
	//	if (!generatedPoints.Contains(pointA.position))
	//	{
	//		generatedPoints.Add(pointA.position);
	//	}

	//	if (!generatedPoints.Contains(pointB.position))
	//	{
	//		generatedPoints.Add(pointB.position);
	//	}
	//}

	public List<Vector3> GeneratePoints(Vector3 pointA, Vector3 pointB, float interval, float skipRadius)
	{
		var generatedPoints = new List<Vector3>();
		if (pointA == null || pointB == null) return null;

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

		// Ensure the endpoints are included if they are outside the skip radius
		//if (Vector3.Distance(pointA.position, centerPoint) > skipRadius && !generatedPoints.Contains(pointA.position))
		//{
		//	generatedPoints.Add(pointA.position);
		//}

		//if (Vector3.Distance(pointB.position, centerPoint) > skipRadius && !generatedPoints.Contains(pointB.position))
		//{
		//	generatedPoints.Add(pointB.position);
		//}

		return generatedPoints;
	}

	//void OnDrawGizmos()
	//{
	//	if (pointA != null && pointB != null)
	//	{
	//		Gizmos.color = Color.red;
	//		Gizmos.DrawLine(pointA.position, pointB.position);

	//		if (generatedPoints != null)
	//		{
	//			Gizmos.color = Color.blue;
	//			foreach (Vector3 point in generatedPoints)
	//			{
	//				Gizmos.DrawSphere(point, 0.1f);
	//			}
	//		}
	//	}
	//}
}
