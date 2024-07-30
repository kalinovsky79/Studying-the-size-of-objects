using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndGoSection
{
	private Transform movableObject;
	ChainRunner chainRunner;

	private Vector3 targetPoint;

	private RotationStep rotationStep;
	private LinearMoveXZStep linearMoveXZStep;

	public Action OnStartRotation;
	public Action OnStartLinear;
	public Action OnEndLinear;
	public float RotationSpeed
	{
		get { return rotationStep.rotationSpeed; }
		set { rotationStep.rotationSpeed = value; }
	}
	public float Speed
	{
		get { return linearMoveXZStep.speed; }
		set { linearMoveXZStep.speed = value; }
	}

	public RotateAndGoSection(Transform obj)
	{
		chainRunner = new ChainRunner();
		rotationStep = new RotationStep
		{
			movableObject = obj,
			OnStartMotion = (x) =>
			{
				OnStartRotation?.Invoke();
			},
//			rotationSpeed = RotationSpeed
		};
		linearMoveXZStep = new LinearMoveXZStep
		{
			movableObject = obj,
//			speed = Speed,
			OnStartMotion = (x) =>
			{
				OnStartLinear?.Invoke();
			},
			OnEndMotion = (x) =>
			{
				OnEndLinear?.Invoke();
			}
		};

		chainRunner.AddStep(rotationStep);
		chainRunner.AddStep(linearMoveXZStep);
	}

	public void NextTarget(Vector3 t)
	{
		targetPoint = t;

		rotationStep.targetPoint = targetPoint;
		linearMoveXZStep.targetPoint = targetPoint;

		chainRunner.RestartChain();
	}

	public bool Update()
	{
		return chainRunner.Update();
	}
}
