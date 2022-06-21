using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : Player
{
	[Header("Player Move Speed")]
	[SerializeField]
	private float moveSpeed;

	[Header("Player Run Speed")]
	[SerializeField]
	private float runSpeed;

	//CharacterController 캐싱 준비
	private CharacterController controllerCharacter = null;

	//캐릭터 CollisionFlags 초기값 설정
	private CollisionFlags collisionFlagsCharacter = CollisionFlags.None;

	//캐릭터 중력값
	private float gravity = 9.8f;

	//캐릭터 중력 속도 값
	private float verticalSpd = 0f;

	//캐릭터 현재 이동 방향 초기값 설정
	private Vector3 MoveDirect = Vector3.zero;
	//캐릭터 CollisionFlages초기값
	private CollisionFlags collisionFlags = CollisionFlags.None;
	//캐릭터 이동 방향 회전 속도 설정
	public float DirectRotateSpd = 100.0f;
	//현재 캐릭터 이동 백터 값 
	private Vector3 vecNowVelocity = Vector3.zero;
	//캐릭터 몸통 회전 속도 설정
	public float BodyRotateSpd = 5.0f;
	//가변 증가값 설정
	[Range(0.01f, 5.0f)]
	public float VelocityChangeSpd = 0.1f;

	private float horizontal;
	private float vertical;
	private bool isRun;

	private EventParam eventParam;

	protected override void Start()
	{
		base.Start();
		EventManager.StartListening("MoveInput", GetMoveInput);
		controllerCharacter = GetComponent<CharacterController>();
	}
	void Update()
	{
		Move();
		BodyDirectChange();
		MoveAnimation();
		setGravity();
	}

	private void Move()
	{
		//백터 내적
		Transform CameraTransform = Camera.main.transform;

		//메인 카메라가 바라보고 있는 방향이 월드상에서 어떤 방향인가...
		Vector3 forward = CameraTransform.TransformDirection(Vector3.forward);
		forward.y = 0;

		//forward.z, forward.x
		Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);      //포워드.z가 양수면 .x가 음수여야함  <----중요!!!


		//캐릭터 이동하고자 하는 '방향'!
		Vector3 targetDirect = horizontal * right + vertical * forward;

		//진행방향, 목표방향, 회전속도, 변경되는 속도
		MoveDirect = Vector3.RotateTowards(MoveDirect, targetDirect, DirectRotateSpd * Mathf.Deg2Rad * Time.deltaTime, 1000.0f);
		MoveDirect = MoveDirect.normalized;

		//이동속도
		float spd = moveSpeed;

		if (isRun)
		{
			spd = runSpeed;
		}

		Vector3 vecGravity = new Vector3(0f, verticalSpd, 0f);

		//프레임이동량
		Vector3 amount = (MoveDirect * spd * Time.deltaTime) + vecGravity;

		//실제이동
		collisionFlags = playerController.Move(amount);
	}


	/// <summary>
	/// 현재 내 케릭터 이동 속도 가져오는 함  
	/// </summary>
	/// <returns>float</returns>
	float getNowVelocityVal()
	{
		//현재 캐릭터가 멈춰 있다면 
		if (playerController.velocity == Vector3.zero)
		{
			//반환 속도 값은 0
			vecNowVelocity = Vector3.zero;
		}
		else
		{
			//반환 속도 값은 현재 /
			Vector3 retVelocity = playerController.velocity;
			retVelocity.y = 0.0f;

			vecNowVelocity = Vector3.Lerp(vecNowVelocity, retVelocity, VelocityChangeSpd * Time.fixedDeltaTime);

		}
		//거리 크기
		return vecNowVelocity.magnitude;
	}

	void BodyDirectChange()
	{
		if (getNowVelocityVal() > 0.0f)
		{
			Vector3 newForward = playerController.velocity; //동기식은 차례대로 멈추지 않고 실행되는 것, 비동기식은 시작함수는 있으나 끝나는 것은 상관없이 동시에 실행
			newForward.y = 0.0f;

			transform.forward = Vector3.Lerp(transform.forward, newForward, BodyRotateSpd * Time.deltaTime);
		}
	}

	private void MoveAnimation()
	{
		if (horizontal != 0 || vertical != 0)
		{
			if (isRun)
			{
				playerState = PlayerState.Run;
				ani.SetBool("IsRun", true);
				return;
			}
			playerState = PlayerState.Walk;
			ani.SetBool("IsWalk", true);
			ani.SetBool("IsRun", false);
		}
		else if (horizontal == 0 && vertical == 0)
		{
			playerState = PlayerState.Idle;
			ani.SetBool("IsWalk", false);
			ani.SetBool("IsRun", false);
		}
	}

	private void GetMoveInput(EventParam eventParam)
	{
		isRun = eventParam.input.isRun;
		horizontal = eventParam.input.moveVector.x;
		vertical = eventParam.input.moveVector.y;
	}

	void setGravity()
	{
		if ((collisionFlagsCharacter & CollisionFlags.CollidedBelow) != 0)
		{
			verticalSpd = 0f;
		}
		else
		{
			verticalSpd -= gravity * Time.deltaTime;
		}
	}
}
