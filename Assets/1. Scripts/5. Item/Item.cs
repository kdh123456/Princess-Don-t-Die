using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	private void Start()
	{
		EventManager.StartListening("ExecuteItem", CheckItem);
	}

	private void CheckItem(EventParam eventParam)
	{
		switch (eventParam.items)
		{
			case Items.HPPositon:
				HpUp();
				break;
			case Items.ManaPositon:
				ManaUp();
				break;
			default:
				Debug.Log(eventParam.items);
				break;
		}

	}

	public void HpUp()
	{
		GameManager.Instance.PlayerData.hp += 10;
	}
	public void ManaUp()
	{
		GameManager.Instance.PlayerData.Mp += 10;
	}
}
