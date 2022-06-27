using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum PrincessState { None, Idle, Move, Wait, GoTarget, Atk, Damage, Die }
public class Princess : MonoBehaviour,OnHIt
{

	[Header("타켓 위치")]
	public Transform playertransform;

	private Transform princessTransform;

	public Vector3 posTarget = Vector3.zero;

	public Vector3 posLookAt = Vector3.zero;

	#region Basic
	[Header("기본속성")]
	public PrincessState princessState = PrincessState.None;

	public float spdMove = 1f;

	public float basicSpeed = 1f;
	public float runSpeed = 1f;

	private Animator princessAnimator = null;
	#endregion

	#region Fight

	[Header("전투속성")]
	public int atk = 0;
	public int Atk { get => atk; set => atk = value; }

	public float RunRange = 0f;
	public float IdleRange = 1.5f;

	public GameObject effectDamage = null;

	public GameObject effectDie = null;

	[SerializeField]
	private SkinnedMeshRenderer skinnedMeshRenderer = null;
	#endregion

	EventParam eventParam;

	// Start is called before the first frame update
	void Start()
	{
		//처음 상태 대기상태
		princessState = PrincessState.Idle;
		princessTransform = this.transform;
		//애니메이, 트랜스폼 컴포넌트 캐싱 : 쓸때마다 찾아 만들지 않게
		princessAnimator = GetComponent<Animator>();
	}

	private void Update()
	{
		CkState();

		princessTransform.LookAt(playertransform);
	}

	/// <summary>
	/// 해골 상태에 따라 동작을 제어하는 함수 
	/// </summary>
	void CkState()
	{
		switch (princessState)
		{
			case PrincessState.Idle:
				//이동에 관련된 RayCast값
				setIdle();
				princessAnimator.SetBool("isWalk", false);
				break;
			case PrincessState.GoTarget:
			case PrincessState.Move:
				setMove();
				princessAnimator.SetBool("isWalk", true);
				break;

			default:
				break;
		}
	}

	void setIdle()
	{
		if (playertransform == null)
		{
			posTarget = new Vector3(princessTransform.position.x + Random.Range(-10f, 10f),
									princessTransform.position.y + 1000f,
									princessTransform.position.z + Random.Range(-10f, 10f)
				);
			Ray ray = new Ray(posTarget, Vector3.down);
			RaycastHit infoRayCast = new RaycastHit();
			if (Physics.Raycast(ray, out infoRayCast, Mathf.Infinity) == true)
			{
				posTarget.y = infoRayCast.point.y;
			}
			princessState = PrincessState.Move;
		}
		else
		{
			if (IdleRange < Mathf.Abs(playertransform.position.magnitude - princessTransform.position.magnitude))
				princessState = PrincessState.GoTarget;
		}
	}

	/// <summary>
	/// 해골 상태가 이동 일 때 동 
	/// </summary>
	void setMove()
	{
		//출발점 도착점 두 벡터의 차이 
		Vector3 distance = Vector3.zero;

		//해골 상태
		switch (princessState)
		{
			//해골이 돌아다니는 경우
			case PrincessState.Move:
				//만약 랜덤 위치 값이 제로가 아니면
				if (posTarget != Vector3.zero)
				{
					//목표 위치에서 해골 있는 위치 차를 구하고
					distance = posTarget - princessTransform.position;

					//만약에 움직이는 동안 해골이 목표로 한 지점 보다 작으 
					if (distance.magnitude <= IdleRange)
					{
						//대기 동작 함수를 호출
						princessState = PrincessState.Idle;
						//여기서 끝냄
						return;
					}
					else if(distance.magnitude > RunRange)
					{
						spdMove = runSpeed;
					}
					else
					{
						spdMove = basicSpeed;
					}
				}
				break;
			//캐릭터를 향해서 가는 돌아다니는  경우
			case PrincessState.GoTarget:
				//목표 캐릭터가 있을 땟
				if (playertransform != null)
				{
					//목표 위치에서 해골 있는 위치 차를 구하고
					distance = playertransform.transform.position - princessTransform.position;
					//만약에 움직이는 동안 해골이 목표로 한 지점 보다 작으 
					if (distance.magnitude < IdleRange)
					{
						//공격상태로 변경합니.
						princessState = PrincessState.Idle;
						return;
					}
					else if (distance.magnitude > RunRange)
					{
						spdMove = runSpeed;
					}
					else
					{
						spdMove = basicSpeed;
					}
				}
				break;
			default:
				break;

		}

		//해골 이동할 방향에 크기를 없애고 방향만 가진(normalized)
		Vector3 direction = distance.normalized;

		//방향은 x,z 사용 y는 땅을 파고 들어갈거라 안함
		direction = new Vector3(direction.x, 0f, direction.z);

		//이동량 방향 구하기
		Vector3 amount = direction * spdMove * Time.deltaTime;

		//캐릭터 컨트롤이 아닌 트랜스폼으로 월드 좌표 이용하여 이동
		princessTransform.Translate(amount, Space.World);
		//캐릭터 방향 정하기
		//princessTransform.LookAt(posLookAt);
	}

	public void OnHit(int atk)
	{
		//데미지 받은거 해야하고
		if(!princessAnimator.GetBool("isDamage"))
		{
			princessAnimator.SetBool("isDamage", true);
			GameManager.Instance.PrincessData.hp -= atk;
			EventManager.TriggerEvent("HPPrincess", eventParam);
			//공주 hp바도 해주고
			if (GameManager.Instance.PrincessData.hp <= 0)
			{
				this.gameObject.SetActive(false);
			}
		}
	}

	private void OnGUI()
	{
		GUIStyle gUIStyle = new GUIStyle();
		gUIStyle.fontSize = 40;
		gUIStyle.normal.textColor = Color.red;

		GUI.Label(new Rect(0, 100, 5, 5), "공주 HP = " + GameManager.Instance.PrincessData.hp.ToString(), gUIStyle);
	}

	public void OnDmgAnmationFinished()
	{
		princessAnimator.SetBool("isDamage", false);
	}
}
