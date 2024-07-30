using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{
	event EventHandler FirstPassDone;
	event EventHandler GameWin;
	void NextPath();
	void RunFirstPass();
	void RunSecondPass();
}
