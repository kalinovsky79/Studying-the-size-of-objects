using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using UnityEngine;
using static TouchScript.Behaviors.Cursors.UI.GradientTexture;

public class LeafCoverer : MonoBehaviour
{
	[SerializeField] private GameObject[] coveringLeafs;
	[SerializeField] private Vector3 size;
	[SerializeField] private float spacing;

	private List<LeafExile> leaves = new List<LeafExile>();

	// Start is called before the first frame update
	void Start()
	{
		//generate();
		//GenerateGridPositions();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void GenerateField()
	{
		leaves = GenerateGridPositions();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		
		Gizmos.DrawWireCube(transform.position, size);
	}

	private void generate()
	{
		for (int i = 0; i < 200; i++) 
		{
			// Calculate the minimum and maximum bounds of the area
			Vector3 minBounds = transform.position - size / 2;
			Vector3 maxBounds = transform.position + size / 2;

			// Generate a random position within the bounds
			float randomX = Random.Range(minBounds.x, maxBounds.x);
			float randomY = Random.Range(minBounds.y, maxBounds.y);
			float randomZ = Random.Range(minBounds.z, maxBounds.z);

			//Instantiate(coveringLeafs[Random.Range(0, coveringLeafs.Length)], new Vector3(randomX, randomY, randomZ), Quaternion.identity);


			Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);

			// Generate a random rotation around the Y-axis
			float randomYRotation = Random.Range(0f, 360f);
			Quaternion randomRotation = Quaternion.Euler(0f, randomYRotation, 0f);

			// Instantiate the object with the random position and rotation
			GameObject randomLeaf = coveringLeafs[Random.Range(0, coveringLeafs.Length)];
			Instantiate(randomLeaf, randomPosition, randomRotation);
		}
	}

	public void HitLeaf(Vector3 p)
	{
		BurstObjects(p);
	}

	public float kikRadius = 3.0f;
	public LayerMask layerMask;

	private void BurstObjects(Vector3 point)
	{
		// Find all colliders within the specified radius
		Collider[] colliders = Physics.OverlapSphere(point, kikRadius, layerMask);



		// Iterate through the colliders and get the objects
		foreach (Collider collider in colliders)
		{
			var obj = collider.gameObject.GetComponent<LeafExile>();
			obj.Kik(point);
			//Debug.Log(obj.name);
			//Debug.Log("Found object: " + obj.name);
			// You can perform any additional operations on the found objects here
		}
	}

	public void Clear()
	{
		foreach (var item in leaves)
		{
			if(item != null) Destroy(item.gameObject);
		}

		leaves.Clear();
	}

	public List<LeafExile> GenerateGridPositions()
	{
		List<LeafExile> res = new List<LeafExile>();

		var center = transform.position;

		// Calculate the number of points along each axis
		int numPointsX = Mathf.CeilToInt(size.x / spacing);
		int numPointsY = Mathf.CeilToInt(size.y / spacing);
		int numPointsZ = Mathf.CeilToInt(size.z / spacing);

		// Generate points in the grid
		for (int x = 0; x < numPointsX; x++)
		{
			for (int y = 0; y < numPointsY; y++)
			{
				for (int z = 0; z < numPointsZ; z++)
				{
					// Calculate the position of the point
					float posX = center.x - size.x / 2 + x * spacing;
					float posY = center.y - size.y / 2 + y * spacing;
					float posZ = center.z - size.z / 2 + z * spacing;

					Vector3 gridPosition = new Vector3(posX, posY, posZ);

					// Generate a random rotation around the Y-axis
					float randomYRotation = Random.Range(0f, 360f);
					Quaternion randomRotation = Quaternion.Euler(0f, randomYRotation, 0f);

					// Instantiate the object with the grid position and random rotation
					GameObject randomLeaf = coveringLeafs[Random.Range(0, coveringLeafs.Length)];
					var l = Instantiate(randomLeaf, gridPosition, randomRotation);
					res.Add(l.GetComponent<LeafExile>());
				}
			}
		}

		return res;
	}


}
