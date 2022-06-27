using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
	public GameObject inventoryPanel;

	private Slot[] slot;
	public Transform slotHolder;

	private void Start()
	{
		slot = slotHolder.GetComponentsInChildren<Slot>();
		EventManager.StartListening("ONINVENTORY", OnInventory);
		EventManager.StartListening("UpdateSlot", UpdateSlot);
	}

	public void OffInventory()
	{
		inventoryPanel.SetActive(false);
	}
	private void OnInventory(EventParam eventParam)
	{
		inventoryPanel.SetActive(eventParam.input.isInventory);
	}

	private void UpdateSlot(EventParam eventParam)
	{
		for(int i = 0; i<slot.Length; i++)
		{
			if(slot[i].items == eventParam.items && slot[i].count < 99)
			{
				slot[i].count++;
				slot[i].items = eventParam.items;
				slot[i].UpdateSlotUI(false);
				break;
			}
			else if(slot[i].items == Items.None)
			{
				slot[i].count++;
				slot[i].items = eventParam.items;
				slot[i].UpdateSlotUI(false);
				break;
			}
		}

	}
}
