using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class RabbitThinker : MonoBehaviour
{
	public Animator animator;
	public float LinearSpeed = 1.0f;

	private RotateAndGoSection sectionRunner;

	public event EventHandler WayCompleted;

	private GameMusicManager gameMusicManager;
	private AudioSource player;

	// Start is called before the first frame update
	void Start()
	{
		player = gameObject.AddComponent<AudioSource>();
		gameMusicManager = FindAnyObjectByType<GameMusicManager>();

		//player.volume = gameMusicManager.EffectsVolume;
		//player.clip = gameMusicManager.jumpFrog;

		sectionRunner = new RotateAndGoSection(transform);

		sectionRunner.Speed = LinearSpeed;
		sectionRunner.RotationSpeed = 12.0f;

		sectionRunner.OnStartRotation = () =>
		{

		};
		sectionRunner.OnStartLinear = () =>
		{
			animator.SetTrigger("walk");
		};
		sectionRunner.OnEndLinear = () =>
		{
			animator.SetTrigger("eat");
		};
	}

	public void GoAB(AnimalPath path)
	{
		StartCoroutine(goForward(path));
	}

	public void GoBA(AnimalPath path)
	{
		StartCoroutine(goBack(path));
	}

	public void GoBuiltPath(AnimalPath path)
	{
		StartCoroutine(go(path));
	}

	private IEnumerator goForward(AnimalPath path)
	{
		int currentPointIndex = 0;

		while(currentPointIndex < path.Points.Length)
		{
			sectionRunner.NextTarget(path.Points[currentPointIndex].transform.position);

			while (sectionRunner.Update())
				yield return null;

			path.Points[currentPointIndex].Eat();

			yield return new WaitForSeconds(1.0f);

			currentPointIndex++;
		}

		sectionRunner.NextTarget(path.lastPoint.position);

		while (sectionRunner.Update())
			yield return null;

		animator.SetTrigger("idle");

		WayCompleted?.Invoke(this, EventArgs.Empty);
	}

	private IEnumerator goBack(AnimalPath path)
	{
		int currentPointIndex = path.Points.Length - 1;

		while (currentPointIndex >= 0)
		{
			sectionRunner.NextTarget(path.Points[currentPointIndex].transform.position);

			while (sectionRunner.Update())
				yield return null;

			path.Points[currentPointIndex].Eat();
			yield return new WaitForSeconds(1.0f);

			animator.SetTrigger("idle");

			currentPointIndex--;
		}

		sectionRunner.NextTarget(path.startPoint.position);

		while (sectionRunner.Update())
			yield return null;

		animator.SetTrigger("idle");

		WayCompleted?.Invoke(this, EventArgs.Empty);
	}

	private IEnumerator go(AnimalPath path)
	{
		int currentPointIndex = 0;

		while (currentPointIndex < path.Points.Length)
		{
			sectionRunner.NextTarget(path.builtPath[currentPointIndex].transform.position);

			while (sectionRunner.Update())
				yield return null;

			path.builtPath[currentPointIndex].Eat();
			yield return new WaitForSeconds(1.0f);
			animator.SetTrigger("idle");
			yield return new WaitForSeconds(1.0f);

			currentPointIndex++;
		}

		sectionRunner.NextTarget(path.finishPoint);

		while (sectionRunner.Update())
			yield return null;

		animator.SetTrigger("idle");

		WayCompleted?.Invoke(this, EventArgs.Empty);
	}

	bool isGoing = false;
	// Update is called once per frame
	void Update()
	{

	}
}