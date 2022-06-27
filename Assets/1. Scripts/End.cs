using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
	EventParam eventParam;
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player") && GameManager.Instance.StageCount == 3)
		{
			EventManager.TriggerEvent("Clear", eventParam);
		}
	}
}
