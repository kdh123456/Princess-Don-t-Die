using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
	public GameObject obj;
	public Image image;
	public Text text;
	public int count;
	public Items items = Items.None;

	EventParam eventParam;
	public void ItemsOn()
	{
		eventParam.items = items;
		count--;
		EventManager.TriggerEvent("ExecuteItem", eventParam);
		if(count <=0)
		{
			items = Items.None;
			UpdateSlotUI(true);
		}
		UpdateSlotUI(false);
	}

	public void ActivePanel()
	{
		obj.SetActive(true);
	}

	public void UnActivePanel()
	{
		obj.SetActive(false);
	}

	public void UpdateSlotUI(bool isdone)
	{
		if(isdone)
		{
			text.text = null;
			image.sprite = UIManager.Instance.sprite[0];
		}
		text.text = count.ToString();
		image.sprite = UIManager.Instance.sprite[(int)items];
	}
}
