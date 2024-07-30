using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicManager : MonoBehaviour
{
	[SerializeField, Range(0.0f, 1.0f)] private float effectsVolume = 1.0f;
	[SerializeField, Range(0.0f, 1.0f)] private float musicVolume = 1.0f;

	public float EffectsVolume => effectsVolume;
	public float MusicVolume => musicVolume;

	//public AudioClip hitBaloon;
	//public AudioClip goodBaloon;
	public AudioClip jumpFrog;
	//public AudioClip waterWalk;

	[SerializeField] private AudioClip[] gameMusic;
	[SerializeField] private AudioClip[] goodHits;
	[SerializeField] private AudioClip[] badHits;
	[SerializeField] private AudioClip[] waterWalks;


	RandomList<AudioClip> randomListBgMusic;
	RandomList<AudioClip> randomListGoodHits;
	RandomList<AudioClip> randomListBadHits;
	RandomList<AudioClip> randomListWaterWalks;

	public AudioClip NextBgMusic()
	{
		if(randomListBgMusic == null)
			randomListBgMusic = new RandomList<AudioClip>(gameMusic);

		return randomListBgMusic.Next();
	}

	public AudioClip NextGoodHit()
	{
		if (randomListGoodHits == null)
			randomListGoodHits = new RandomList<AudioClip>(goodHits);

		return randomListGoodHits.Next();
	}

	public AudioClip NextBadHit()
	{
		if (randomListBadHits == null)
			randomListBadHits = new RandomList<AudioClip>(badHits);

		return randomListBadHits.Next();
	}

	public AudioClip NextWaterWalk()
	{
		if (randomListWaterWalks == null)
			randomListWaterWalks = new RandomList<AudioClip>(waterWalks);

		return randomListWaterWalks.Next();
	}
}
