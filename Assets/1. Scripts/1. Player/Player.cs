using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[HideInInspector]
	public bool isAttacked;

	protected CharacterController playerController;
	protected Animator ani;
	protected Rigidbody rigid;
	protected Collider col;
	protected PlayerState playerState = PlayerState.Idle;

	protected virtual void Start()
	{
		playerController = GetComponent<CharacterController>();
		rigid = GetComponent<Rigidbody>();
		col = GetComponent<Collider>();
		ani = GetComponent<Animator>();

		EventManager.StartListening("PlayerDamage", Hit);
	}

	private void HitEnd()
	{
		ani.SetBool("Damage", false);
	}

	private void Hit(EventParam eventParam)
	{
		if(!ani.GetBool("Damage"))
		{
			ani.SetBool("Damage", true);
			GameManager.Instance.PlayerData.hp -= eventParam.eventint;
			EventManager.TriggerEvent("HP", eventParam);
			if (GameManager.Instance.PlayerData.hp <= 0)
				this.gameObject.SetActive(false);
		}
	}
}
