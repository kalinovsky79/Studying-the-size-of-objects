using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using UnityEngine;

public class hhh : MonoBehaviour
{
	private PressGesture PressGesture;

	// Start is called before the first frame update
	void Start()
	{
		PressGesture = GetComponent<PressGesture>();
		PressGesture.Pressed += PressGesture_Pressed;

		Debug.Log($"hhh.Start() {PressGesture.name}");
	}

	private void PressGesture_Pressed(object sender, System.EventArgs e)
	{
		Debug.Log(this.name);
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
