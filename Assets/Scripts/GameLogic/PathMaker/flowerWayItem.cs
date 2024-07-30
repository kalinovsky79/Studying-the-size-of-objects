using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerWayItem : MonoBehaviour
{
	private AnimateVector3 animateVector3;

	private Vector3 originScale;
	private bool originScaleSaved = false;

	// Start is called before the first frame update
	void Start()
	{
		animateVector3 = new AnimateVector3
		{
			duration = 1.0f,
			OnAnimationStep = (x) =>
			{
				transform.localScale = x;
			}
		};

		StartCoroutine(appear(true));
	}

	private IEnumerator appear(bool appearMe)
	{
		if (!originScaleSaved)
		{
			originScale = transform.localScale;
			originScaleSaved = true;
		}

		if (appearMe)
		{
			animateVector3.valueA = Vector3.zero;
			animateVector3.valueB = originScale;
			transform.localScale = Vector3.zero;
		}
		else
		{
			animateVector3.valueA = originScale;
			animateVector3.valueB = Vector3.zero;
			transform.localScale = originScale;
		}

		while(animateVector3.update(null))
			yield return null;

		if (!appearMe) Destroy(gameObject);
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void Disappear()
	{
		StartCoroutine(appear(false));
	}
}
