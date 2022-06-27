using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkil : MonoBehaviour
{
	[SerializeField]
	private GameObject Storepanel;
	[SerializeField]
	private Text text;
	private void Start()
	{
		EventManager.StartListening("ONSTORE", OnStore);
	}
	private void OnStore(EventParam eventParam)
	{
		Storepanel.SetActive(eventParam.input.isStore);
	}

	public void UpAtk()
	{
		if(GameManager.Instance.PlayerData.Money-10 > 0)
		{
			GameManager.Instance.PlayerData.damage += 10;
			GameManager.Instance.PlayerData.Money -= 10;
		}
	}

	public void UpHp()
	{
		if (GameManager.Instance.PlayerData.Money - 10 > 0)
		{
			GameManager.Instance.PlayerData.maxHp += 10;
			GameManager.Instance.PlayerData.Money -= 10;
		}
	}
	private void Update()
	{
		UpdateText();
	}

	public void UpdateText()
	{
		text.text = "스킬포인트 : " + GameManager.Instance.PlayerData.Money;
	}
	private void OnDestroy()
	{
		EventManager.StopListening("ONSTORE", OnStore);
	}
}
