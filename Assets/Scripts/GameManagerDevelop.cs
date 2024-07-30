using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerDevelop : MonoBehaviour
{
	private IGameManager _gameManager;

	[SerializeField] private GameManager gameManager;
	[SerializeField] private LeafCoverer leafCoverer;

	// Start is called before the first frame update
	void Start()
	{
		_gameManager = gameManager;

		_gameManager.FirstPassDone += GameManager_FirstPassDone;
		_gameManager.GameWin += GameManager_GameWin;

		StartCoroutine(init());
	}

	private IEnumerator init()
	{
		yield return new WaitForEndOfFrame();

		leafCoverer.GenerateField();
		_gameManager.NextPath();
		_gameManager.RunFirstPass();
	}

	private void GameManager_GameWin(object sender, System.EventArgs e)
	{
		Debug.Log("YOU WIN!!!");

		StartCoroutine(restart());
	}

	private void GameManager_FirstPassDone(object sender, System.EventArgs e)
	{
		StartCoroutine(goSecondPass());
	}

	private IEnumerator goSecondPass()
	{
		leafCoverer.Clear();

		yield return new WaitForSeconds(2.0f);

		leafCoverer.GenerateField();
		_gameManager.NextPath();
		_gameManager.RunSecondPass();
	}

	private IEnumerator restart()
	{
		leafCoverer.Clear();

		yield return new WaitForSeconds(2.0f);

		leafCoverer.GenerateField();
		_gameManager.NextPath();
		_gameManager.RunFirstPass();
	}
}
