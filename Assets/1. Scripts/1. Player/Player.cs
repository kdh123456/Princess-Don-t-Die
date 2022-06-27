using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour,OnHIt
{
	[HideInInspector]
	public bool isAttacked;
	private int atk;
	public int Atk { get => atk; set => atk = value; }

	protected CharacterController playerController;
	protected Animator ani;
	protected Rigidbody rigid;
	protected Collider col;
	protected PlayerState playerState = PlayerState.Idle;

	EventParam eventParam;

	protected virtual void Start()
	{
		atk = (int)GameManager.Instance.PlayerData.damage;
		playerController = GetComponent<CharacterController>();
		rigid = GetComponent<Rigidbody>();
		col = GetComponent<Collider>();
		ani = GetComponent<Animator>();
	}

	private void HitEnd()
	{
		ani.SetBool("Damage", false);
	}
	public void OnHit(int atk)
	{
		if (!ani.GetBool("Damage"))
		{
			ani.SetBool("Damage", true);
			GameManager.Instance.PlayerData.hp -= atk;
			EventManager.TriggerEvent("HPPlayer", eventParam);
			if (GameManager.Instance.PlayerData.hp <= 0)
				this.gameObject.SetActive(false);
		}
	}
	//private void OnGUI()
	//{
	//	GUIStyle gUIStyle = new GUIStyle();
	//	gUIStyle.fontSize = 40;
	//	gUIStyle.normal.textColor = Color.red;

	//	GUI.Label(new Rect(0, 100, 5, 5), "플레이어 HP = " + GameManager.Instance.PlayerData.hp.ToString(), gUIStyle);
	//}
}
