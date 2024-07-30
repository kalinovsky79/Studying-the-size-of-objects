using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomList<T>
{
	private List<T> list = new List<T>();

	private int _lastNumber = -1;
	private int _secondLastNumber = -1;

	public RandomList(T[] clips)
	{
		list.Clear();
		list.AddRange(clips);
	}

	public T Next()
	{
		if (list.Count == 0) return default(T);

		if(list.Count == 1) return list[0];

		int nextNumber;
		do
		{
			nextNumber = Random.Range(0, list.Count);
		} while (nextNumber == _lastNumber && nextNumber == _secondLastNumber);

		_secondLastNumber = _lastNumber;
		_lastNumber = nextNumber;

		return list[nextNumber];
	}
}
