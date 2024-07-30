using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using UnityEngine;

public class AnimalPathElement : MonoBehaviour
{
	[SerializeField] private Color qizmosColor = Color.yellow;

//	[SerializeField] private GameObject goodHit;

	[SerializeField] private float eatingAnimationSpeed;

	private AudioSource player;
	private GameMusicManager gameMusicManager;

	public int pointName = 0;
	public float inclineScale = 1.0f;
	public float offsetScale = 0.1f;

	private ScaleAppear scaleAppearing;

	private AnimalPath _path;
	private PressGesture pressGesture;

	private AnimateColor animateShineColor;
	ImpulseAnimationVector3 impulseAnimation;

	private bool marked = false;

	public void SetPathRoot(AnimalPath pr)
	{
		_path = pr;
	}

	public void Eat()
	{
		scaleAppearing.SetAnimationSpeed(eatingAnimationSpeed);
		scaleAppearing.Appear(false);
	}

	// Start is called before the first frame update
	void Start()
	{
		transform.localScale = new Vector3(
			transform.localScale.x * pointName * inclineScale + offsetScale,
			transform.localScale.y * pointName * inclineScale + offsetScale,
			transform.localScale.z * pointName * inclineScale + offsetScale);

		player = gameObject.AddComponent<AudioSource>();
		gameMusicManager = FindAnyObjectByType<GameMusicManager>();

		//player.volume = gameMusicManager.EffectsVolume;

		pressGesture = GetComponent<PressGesture>();
		if(pressGesture != null)
			pressGesture.Pressed += PressGesture_Pressed;

		//animateShineColor = new AnimateColor
		//{
		//	duration = 0.7f,
		//	valueA = fadedColor,
		//	//valueB = cShineEmission,
		//	valueB = shineColor,
		//	OnAnimationStep = (c) =>
		//	{
		//		changeColor(renderersToShine, c);
		//	}
		//};

		impulseAnimation = new ImpulseAnimationVector3
		{
			duration = 0.4f,
			OnAnimationStep = (x) =>
			{
				transform.localScale = x;
			}
		};

		StartCoroutine(init());
	}

	private bool goingToShine = false;

	private IEnumerator init()
	{
		yield return new WaitForEndOfFrame();
		scaleAppearing = GetComponent<ScaleAppear>();
	}

	private void PressGesture_Pressed(object sender, System.EventArgs e)
	{
		if (marked) return;
		//if(goingToShine) return;
		_path.numberIsPressed(this);
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = qizmosColor;
		Gizmos.DrawSphere(transform.position, 0.1f);
	}

	public void WrongHit()
	{
		if (isImpulsing) return;
		if (marked) return;
		//if (goingToShine) return;

		StartCoroutine(startImpulse());
	}

	public void GoodHit()
	{
		marked = true;

		//Instantiate(goodHit, transform.position, Quaternion.identity);
		//player.clip = gameMusicManager.NextGoodHit();

		//player.pitch = Random.Range(0.7f, 1.2f);

		//player.Play();
	}

	private bool originScaleSaved = false;
	private bool isImpulsing = false;
	private IEnumerator startImpulse()
	{
		isImpulsing = true;

		if(!originScaleSaved)
		{
			impulseAnimation.originalValue = transform.localScale;
		}

		//player.clip = gameMusicManager.NextBadHit();
		//player.Play();

		impulseAnimation.impulseMagnitudeY = Random.Range(0.2f, 0.6f);
		impulseAnimation.impulseMagnitudeX = Random.Range(0.2f, 0.6f);
		impulseAnimation.impulseMagnitudeZ = 0;

		while (impulseAnimation.update(null))
			yield return null;

		isImpulsing = false;
	}
}
