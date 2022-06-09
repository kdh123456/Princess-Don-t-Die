using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Player
{
	[SerializeField]
	private int hp;
	[SerializeField]
	private float attackDamage;

	private bool isAttack;
	private int CountAttack;

	EventParam eventParam1;

	protected override void Start()
	{
		base.Start();
		EventManager.StartListening("IsAttack", AttackEvent);
		EventManager.StartListening("PlayerDamage", hit);
	}

	private void Update()
	{
		if (isAttack)
		{
			Attack();
		}

		EventManager.TriggerEvent("Attaking", eventParam1);
	}
	private void PlayerAttackAnimationEnd()
	{
		CheckAttackPhase();
	}

	private void Attack()
	{
		if (CountAttack < 1)
		{
			ani.SetInteger("IsAttackCount", 1);
			CountAttack = 1;
			eventParam1.eventint = CountAttack;
		}
		if (ani.GetCurrentAnimatorStateInfo(1).normalizedTime >= 0.6f && ani.GetCurrentAnimatorStateInfo(1).normalizedTime < 1f)
		{
			CountAttack++;
		}
	}

	private void CheckAttackPhase()
	{
		if(ani.GetCurrentAnimatorStateInfo(1).IsName("First Attack"))
		{
			if (CountAttack > 1)
			{
				ani.SetInteger("IsAttackCount", 2);
				CountAttack = 2;
				eventParam1.eventint = CountAttack;
			}
			else
			{
				ResetAttackPhase();
			}
		}
		else if (ani.GetCurrentAnimatorStateInfo(1).IsName("Secound Attack"))
		{
			if (CountAttack > 2)
			{
				ani.SetInteger("IsAttackCount", 3);
				CountAttack = 3;
				eventParam1.eventint = CountAttack;
			}
			else
			{
				ResetAttackPhase();
			}
		}
		else if (ani.GetCurrentAnimatorStateInfo(1).IsName("Third Attack"))
		{
			ResetAttackPhase();
		}
	}

	private void ResetAttackPhase()
	{
		CountAttack = 0;
		ani.SetInteger("IsAttackCount", 0);
		eventParam1.eventint = CountAttack;
	}

	private void AttackEvent(EventParam eventParam)
	{
		isAttack = eventParam.isAttack;
	}

	private void hit(EventParam eventParam)
	{
		hp=hp - eventParam.eventint;
		ani.SetBool("Damage", true);
		if (hp < 0)
			this.gameObject.SetActive(false);
	}

	private void OnGUI()
	{
		GUIStyle gUIStyle = new GUIStyle();
		gUIStyle.fontSize = 40;

		GUI.Label(new Rect(0, 100, 5, 5), hp.ToString(), gUIStyle);
	}
}
