using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScoresManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI ScoresText;

	public int Scores {get; private set; }

	public void AddScore(int scores)
	{
		Scores += scores;

		ScoresText.text = Scores.ToString();
	}

	public void ResetScores()
	{
		Scores = 0;
		ScoresText.text = Scores.ToString();
	}
}
