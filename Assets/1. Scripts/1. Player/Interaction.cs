using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
	EventParam eventParam;
	private void OnTriggerEnter(Collider other)
	{
		switch (other.tag)
		{
			case "Health":
				eventParam.items = Items.HPPositon;
				EventManager.TriggerEvent("UpdateSlot", eventParam);
				other.gameObject.SetActive(false);
				ObjectPool.Instance.ReturnObject(PoolObjectType.HP, other.gameObject);
				break;
			case "Mana":
				eventParam.items = Items.ManaPositon;
				EventManager.TriggerEvent("UpdateSlot", eventParam);
				ObjectPool.Instance.ReturnObject(PoolObjectType.MANA, other.gameObject);
				break;
		}
	}
}
