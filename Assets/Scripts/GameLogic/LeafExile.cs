using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using UnityEditor.Presets;
using UnityEngine;
using static UnityEngine.UI.Image;

public class LeafExile : MonoBehaviour
{
	private LeafCoverer leafCoverer;

	private PressGesture pressGesture;

	void Start()
	{
		leafCoverer = FindAnyObjectByType<LeafCoverer>();

		pressGesture = GetComponent<PressGesture>();
		pressGesture.Pressed += PressGesture_Pressed;
	}

	private bool pressed = false;
	private void PressGesture_Pressed(object sender, System.EventArgs e)
	{
		if (pressed) return;
		pressed = true;
		Vector3 pressPosition = GetPressPosition();

		leafCoverer.HitLeaf(pressPosition);
	}

	private Vector3 GetPressPosition()
	{
		Ray ray = Camera.main.ScreenPointToRay(pressGesture.ScreenPosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, Mathf.Infinity))
		{
			return hit.point;
		}
		return Vector3.zero;
	}

	private bool isflying = false;

	public void Kik(Vector3 kikPoint)
	{
		if(isflying) return;

		StartCoroutine(goOut(kikPoint));
	}

	private IEnumerator goOut(Vector3 kikPoint)
	{
		isflying = true;

		float moveDistance = 20.0f;
		float moveSpeed = 10.0f;

		Vector3 startPosition = transform.position;
		Vector3 direction = (startPosition - kikPoint).normalized;

		float targetDistance = moveDistance;

		while (Vector3.Distance(startPosition, transform.position) < targetDistance)
		{
			transform.position += direction * moveSpeed * Time.deltaTime;
			yield return null;
		}

		Destroy(gameObject);
	}
}
