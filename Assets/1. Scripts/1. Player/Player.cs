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

	protected virtual void Awake()
	{
		this.gameObject.transform.position = new Vector3(117.8f, 0f, 142.6f);
	}
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
			EventManager.TriggerEvent("PUPDATESLIDER", eventParam);
			if (GameManager.Instance.PlayerData.hp <= 0)
			{
				EventManager.TriggerEvent("Dead", eventParam);
				this.gameObject.SetActive(false);
			}
		}
	}
}
