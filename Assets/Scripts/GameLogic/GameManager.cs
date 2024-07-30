using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * управление объектами игры
 */
public class GameManager : MonoBehaviour, IGameManager
{
	[SerializeField] private GameObject startPoint;

	// набор путей для одной сцены; один путь берется на одну сессию
	[SerializeField] private AnimalPath[] paths;
	[SerializeField] private RabbitThinker rabbit;

	public event EventHandler FirstPassDone;
	public event EventHandler GameWin;

	private RandomList<AnimalPath> randomListPath;

	private AnimalPath currentPath;

	public int passNo { get; private set; } = 0;

	public int PathPointsCount()
	{
		return currentPath.PointsCount();
	}

	// Start is called before the first frame update
	void Start()
	{
		randomListPath = new RandomList<AnimalPath>(paths);
		rabbit.WayCompleted += CurrentFrog_WayCompleted;
	}

	public void NextPath()
	{
		if (currentPath != null)
		{
			currentPath.WayIsBult -= CurrentPath_WayIsBult;
			Destroy(currentPath.gameObject);
		}

		currentPath = Instantiate(randomListPath.Next(), startPoint.transform.position, Quaternion.identity);
		currentPath.WayIsBult += CurrentPath_WayIsBult;
	}

	public void RunFirstPass()
	{
		currentPath.TurnIncreaseBuildWay();
		currentPath.RestartBuilding();
	}

	private void CurrentPath_WayIsBult(object sender, EventArgs e)
	{
		rabbit.GoBuiltPath(currentPath);
	}

	public void RunSecondPass()
	{
		currentPath.RestartBuilding();
		currentPath.TurnDecreaseBuildWay();
	}

	//private IEnumerator startFirstPass()
	//{
	//	yield return StartCoroutine(rulesPage.startPage("make a path from 1 to 10"));

	//	currentPath.AppearNumbers(true);
	//	currentPath.TurnIncreaseBuildWay();
	//	currentPath.TurnOn();
	//}

	//private IEnumerator startSecondPass()
	//{
	//	yield return StartCoroutine(rulesPage.startPage("make a path from 10 to 1"));

	//	currentPath.ResetWay();
	//	currentPath.TurnDecreaseBuildWay();
	//}

	public void StopGame()
	{
		currentPath.StopBuilding();
	}

	private void CurrentPath_UserAnswer(object sender, bool e)
	{
		//if (e)
		//{
		//	if(okEffect != null)
		//		Instantiate(okEffect, motivePoint.position, Quaternion.identity);
		//}
		//else
		//{
		//	if (wrongEffect != null)
		//		Instantiate(wrongEffect, motivePoint.position, Quaternion.identity);
		//}
	}

	private void CurrentFrog_WayCompleted(object sender, System.EventArgs e)
	{
		if(passNo == 0)
		{
			currentPath.DisappearWay();
			currentPath.StopBuilding();
			FirstPassDone?.Invoke(this, EventArgs.Empty);
		}
		else if(passNo == 1)
		{
			currentPath.DisappearWay();
			currentPath.StopBuilding();
			passNo = 0;
			GameWin?.Invoke(this, EventArgs.Empty);
			return;
		}

		passNo++;
	}
}
