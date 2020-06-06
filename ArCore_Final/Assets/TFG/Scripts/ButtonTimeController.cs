using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTimeController : MonoBehaviour
{
	private TimeTest TT;
	private bool countingTime = false;

    void Start()
    {
		TT = FindObjectOfType<TimeTest>();
    }

	public void OnClick()
	{
		if (!countingTime)
		{
			countingTime = true;
			TT.StartCountingTime();
		}

		else
		{
			countingTime = false;
			TT.EndCountingTime();
		}
	}
}
