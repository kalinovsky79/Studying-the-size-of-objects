using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TouchScript.Gestures;
using UnityEngine;
using UnityEngine.UI;

public class RulesPage : MonoBehaviour
{
	[SerializeField] private GameObject rulePageElements;
	[SerializeField] private Image rulesPageBackground;
	[SerializeField] private GameObject rulesPageImage;
	[SerializeField] private TextMeshProUGUI msgText;

	[SerializeField] private TapGesture StartingButton;

	public event EventHandler StartGame;

	private ChainRunner _chainRunnerAppear;
	private ChainRunner _chainRunnerDisappear;

	// Start is called before the first frame update
	void Start()
	{
		StartingButton.Tapped += StartingButton_Tapped;

		_chainRunnerAppear = new ChainRunner();
		_chainRunnerDisappear = new ChainRunner();

		_chainRunnerAppear
			.AddStep(new OperationStep
			{
				Do = () =>
				{
					Color newColor = rulesPageBackground.color;
					newColor.a = 0.0f;
					rulesPageBackground.color = newColor;

					rulesPageImage.SetActive(false);
					StartingButton.gameObject.SetActive(false);
					rulePageElements.SetActive(true);
				}
			})
			.AddStep(new AnimateFloat
			{
				duration = 0.8f,
				valueA = 0.0f,
				valueB = 0.9f,
				OnAnimationStep = (x) =>
				{
					Color newColor = rulesPageBackground.color;
					newColor.a = x;
					rulesPageBackground.color = newColor;
				}
			})
			.AddStep(new OperationStep
			{
				Do = () =>
				{
					rulesPageImage.SetActive(true);
					StartingButton.gameObject.SetActive(true);
				}
			});

		_chainRunnerDisappear
			.AddStep(new OperationStep
			{
				Do = () =>
				{
					rulesPageImage.SetActive(false);
					StartingButton.gameObject.SetActive(false);
				}
			})
			.AddStep(new AnimateFloat
			{
				duration = 0.8f,
				valueA = 0.9f,
				valueB = 0.0f,
				OnAnimationStep = (x) =>
				{
					Color newColor = rulesPageBackground.color;
					newColor.a = x;
					rulesPageBackground.color = newColor;
				}
			})
			.AddStep(new OperationStep
			{
				Do = () =>
				{
					rulePageElements.SetActive(false);
				}
			});

		init = true;
	}

	private void StartingButton_Tapped(object sender, EventArgs e)
	{
		StartGame?.Invoke(sender, EventArgs.Empty);
	}

	private bool init = false;

	public void ShowInstantly(string msg)
	{
		Color newColor = rulesPageBackground.color;
		newColor.a = 0.9f;
		rulesPageBackground.color = newColor;

		msgText.text = msg;

		rulesPageBackground.gameObject.SetActive(true);
		StartingButton.gameObject.SetActive(true);
		rulesPageImage.SetActive(true);
		rulePageElements.SetActive(true);
	}

	public IEnumerator EnterPage(string msg)
	{
		msgText.text = msg;

		//yield return new WaitUntil(() => init);

		_chainRunnerAppear.StartChain();
		while (_chainRunnerAppear.Update()) yield return null;
	}

	public IEnumerator ExitPage()
	{
		_chainRunnerDisappear.StartChain();
		while (_chainRunnerDisappear.Update()) yield return null;
	}

	//private IEnumerator showPage()
	//{
	//	yield return new WaitUntil(() => init);

	//	_chainRunnerAppear.StartChain();
	//	while (_chainRunnerAppear.Update())
	//	{
	//		yield return null;
	//	}

	//	yield return new WaitForSeconds(5.0f);

	//	_chainRunnerDisappear.StartChain();
	//	while (_chainRunnerDisappear.Update())
	//	{
	//		yield return null;
	//	}
	//}

	//public IEnumerator startPage(string msg)
	//{
	//	msgText.text = msg;
	//	gameObject.SetActive(true);
	//	yield return StartCoroutine(showPage());
	//	gameObject.SetActive(false);
	//}
}
