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
	}

	private void hitEnd()
	{
		ani.SetBool("Damage", false);
	}
}
