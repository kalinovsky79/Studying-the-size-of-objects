using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI timeText;

	public float delay = 5.0f;
	public bool showGoOnEnd = false;

	public event EventHandler Expired;

	private bool stopped = false;

	public void Go()
	{
		StartCoroutine(delayCoroutine());
	}

	public void StopTimer()
	{
		stopped = true;
	}

	private IEnumerator delayCoroutine()
	{
		stopped = false;
		timeText.gameObject.SetActive(true);

		float remainingTime = delay;

		while (remainingTime > 0 && !stopped)
		{
			timeText.text = remainingTime.ToString("0");
			yield return new WaitForSeconds(1f);
			remainingTime--;
		}

		if (!stopped)
		{
			timeText.text = "GO!";
			yield return new WaitForSeconds(1f);
			timeText.gameObject.SetActive(false);

			Expired?.Invoke(this, EventArgs.Empty);
		}
	}
}
