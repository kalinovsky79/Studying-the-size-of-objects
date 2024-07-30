using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
	[SerializeField] private DarkenPage darkenPage;
	[SerializeField] private GamePage gamePage;
	[SerializeField] private FinalPage finalPage;
	[SerializeField] private RulesPage rulesPage;

	[SerializeField] private GameManager gameManager;
	[SerializeField] private GameMusicManager gameMusicManager;
	[SerializeField] private CountdownTimer cdTimer;
	[SerializeField] private CountUpTimer upTimer;

	[SerializeField] private TextMeshProUGUI cdTextPro;

	public float gameDelay = 5;

	private AudioSource player;

	// Start is called before the first frame update
	void Start()
	{

		//startPage.EnterPage();
		//rulesPage.ShowInstantly($"make a path from 1 to {gameManager.PathPointsCount()}");
		StartCoroutine(waitOneFrame());

	}

	private IEnumerator waitOneFrame()
	{
		yield return new WaitForEndOfFrame();

		player = gameObject.AddComponent<AudioSource>();

		//startPage.StartGame += StartPage_StartGame;
		rulesPage.StartGame += RulesPage_StartGame;
		finalPage.RestartClicked += FinalPage_RestartClicked;

		cdTimer.Expired += CdTimer_Expired;

		gameManager.FirstPassDone += GameManager_FirstPassDone;
		gameManager.GameWin += GameManager_GameWin;

		rulesPage.ShowInstantly($"make a path from 1 to {gameManager.PathPointsCount()}");

		player.volume = gameMusicManager.MusicVolume;
		player.clip = gameMusicManager.NextBgMusic();
		player.Play();
	}

	private void RulesPage_StartGame(object sender, System.EventArgs e)
	{
		if(gameManager.passNo == 0)
		{
			StartCoroutine(firstPassGame());
		}
		else if(gameManager.passNo == 1)
			StartCoroutine(secondPassGame());
	}

	private void FinalPage_RestartClicked(object sender, System.EventArgs e)
	{
		StartCoroutine(restartGame());
	}

	private void GameManager_FirstPassDone(object sender, System.EventArgs e)
	{
		StartCoroutine(onFirstPassDone());
	}

	private void GameManager_GameWin(object sender, System.EventArgs e)
	{
		StartCoroutine(toFinalPage());
	}

	private IEnumerator rsgStarting()
	{
		cdTextPro.color = Color.yellow;
		cdTextPro.text = "ready";
		cdTextPro.gameObject.SetActive(true);
		yield return new WaitForSeconds(0.5f);

		cdTextPro.color = new Color(1.0f, 0.5f, 0.0f);
		cdTextPro.text = "set";
		yield return new WaitForSeconds(0.5f);

		cdTextPro.color = Color.red;
		cdTextPro.text = "go!";
		yield return new WaitForSeconds(0.5f);

		cdTextPro.gameObject.SetActive(false);
	}

	private IEnumerator firstPassGame()
	{
		yield return StartCoroutine(rulesPage.ExitPage());
		yield return new WaitForSeconds(0.3f);
		yield return StartCoroutine(rsgStarting());
		gamePage.EnterPage();

		gameManager.RunFirstPass();
		upTimer.Go();
	}

	private IEnumerator secondPassGame()
	{
		yield return StartCoroutine(rulesPage.ExitPage());
		yield return new WaitForSeconds(0.3f);
		yield return StartCoroutine(rsgStarting());

		gameManager.RunSecondPass();
		upTimer.ResumeTimer();
	}

	private IEnumerator onFirstPassDone()
	{
		upTimer.PauseTimer();
		yield return StartCoroutine(rulesPage.EnterPage($"make a path from {gameManager.PathPointsCount()} to 1"));
	}

	private IEnumerator restartGame()
	{
		player.Stop();
		yield return StartCoroutine(darkenPage.DarkenScreen(1.0f, 0.7f));

		gameManager.NextPath();

		gamePage.ExitPage();
		finalPage.ExitPage();
		rulesPage.ShowInstantly($"make a path from 1 to {gameManager.PathPointsCount()}");

		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine(darkenPage.UndarkenScreen(0.7f));

		player.clip = gameMusicManager.NextBgMusic();
		player.Play();
	}

	private IEnumerator toFinalPage()
	{
		yield return new WaitForSeconds(1.5f);

		upTimer.StopTimer();
		gameManager.StopGame();

		finalPage.GreatingText($"Brilliant! You helped the frog in {upTimer.Seconds} seconds!!!");
		finalPage.EnterPage();
	}

	private void CdTimer_Expired(object sender, System.EventArgs e)
	{
		gameManager.RunFirstPass();
		upTimer.Go();
	}
}
