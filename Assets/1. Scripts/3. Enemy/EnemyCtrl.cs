using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class EnemyCtrl : MonoBehaviour,OnHIt
{
	public enum SkullState { None, Idle, Move, Wait, GoTarget, Atk, Damage, Die }
	private int count = 0;
	EventParam eventParam;

	[SerializeField]
	private GameObject objet;
	private Canvas hpbarcanvas;
	private GameObject hpbar;
	private Slider hpBar;

	#region Basic Variable
	[Header("기본 속성")]
	public SkullState skullState = SkullState.None;

	public float spdMove = 1f;
	public GameObject targetCharactor = null;
	public Transform targetTransform = null;
	private Transform skullTransform = null;
	public Vector3 posTarget = Vector3.zero;

	private Animator ani = null;
	#endregion

	#region Fight Variable
	[Header("전투속성")]
	//해골 체력
	public float maxHp;
	public float hp = 100;
	public int atk = 50;
	public int Atk { get => atk;  set => atk = value; }
	//해골 공격 거리
	public float AtkRange = 1.5f;
	public ParticleSystem[] attackparticle;//해골 피격 이펙트
	public float radius;
	public LayerMask layerMask;
	public GameObject effectDamage = null;
	//해골 다이 이펙트
	public GameObject effectDie = null;
	private SkinnedMeshRenderer skinnedMeshRenderer = null;
	#endregion

	private void Awake()
	{
		this.gameObject.transform.position = new Vector3(120, 0f, 142.6f);
	}
	void Start()
	{
		Init();
	}
	void Update()
	{
		CkState();
		AnimationCtrl();
		hpbar.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z));
	}
	public void Init()
	{
		//처음 상태 대기상태
		skullState = SkullState.Idle;

		//애니메이, 트랜스폼 컴포넌트 캐싱 : 쓸때마다 찾아 만들지 않게
		ani = GetComponent<Animator>();
		skullTransform = GetComponent<Transform>();

		//스킨매쉬 캐싱
		skinnedMeshRenderer = skullTransform.Find("SoldierRen").GetComponent<SkinnedMeshRenderer>();
		EventManager.StartListening("Attaking", IsAttacked);

		for (int i = 0; i < attackparticle.Length; i++)
		{
			attackparticle[i].Pause();
		}

		effectDamage.GetComponent<ParticleSystem>().Pause();
		effectDie.GetComponent<ParticleSystem>().Pause();

		hpbarcanvas = GameObject.Find("EnemyCanvas").GetComponent<Canvas>();
		maxHp = hp;
		hpbar = Instantiate(objet, hpbarcanvas.transform);
		hpBar = hpbar.GetComponent<Slider>();
		UpdateSlider();
		hpBar.gameObject.SetActive(false);
	}

	#region Animation
	void AnimationCtrl()
	{
		//해골의 상태에 따라서 애니메이션 적용
		switch (skullState)
		{
			//대기와 준비할 때 애니메이션 같.
			case SkullState.Wait:
			case SkullState.Idle:
				//준비 애니메이션 실행
				ani.SetBool("isAttack", false);
				ani.SetBool("isWalk", false);
				ani.SetBool("isDie", false);
				ani.SetBool("isDamage", false);
				break;
			//랜덤과 목표 이동할 때 애니메이션 같.
			case SkullState.Move:
			case SkullState.GoTarget:
				//이동 애니메이션 실행
				ani.SetBool("isWalk", true);
				ani.SetBool("isDie", false);
				ani.SetBool("isDamage", false);
				ani.SetBool("isAttack", false);
				break;
			//공격할 때
			case SkullState.Atk:
				//공격 애니메이션 실행
				ani.SetBool("isWalk", true);
				ani.SetBool("isDie", false);
				ani.SetBool("isDamage", false);
				ani.SetBool("isAttack", true);
				break;
			//죽었을 때
			case SkullState.Die:
				//죽을 때도 애니메이션 실행
				ani.SetBool("isDie", true);
				ani.SetBool("isDamage", false);
				ani.SetBool("isAttack", false);
				ani.SetBool("isWalk", false);
				break;
			default:
				break;

		}
	}
	void OnAtkAnmationFinished()
	{
		ani.SetBool("isWalk", false);
		ani.SetBool("isDie", false);
		ani.SetBool("isDamage", false);
		ani.SetBool("isAttack", false);
		theAtk();
		skullState = SkullState.Idle;
	}

	void OnDmgAnmationFinished()
	{
		skullState = SkullState.Idle;
		this.transform.GetChild(this.transform.childCount-1).gameObject.GetComponent<ParticleSystem>().Pause();
		this.transform.GetChild(this.transform.childCount-1).gameObject.GetComponent<ParticleSystem>().Clear();
		ObjectPool.Instance.ReturnObject(PoolObjectType.DAMAGEDEFFECT, this.transform.GetChild(this.transform.childCount-1).gameObject);
		effectDamage.GetComponent<ParticleSystem>().Pause();		effectDie.GetComponent<ParticleSystem>().Pause();
		effectDie.GetComponent<ParticleSystem>().Clear();
	}

	void OnDieAnmationFinished()
	{
		gameObject.SetActive(false);
		ObjectPool.Instance.ReturnObject(PoolObjectType.Soldier, gameObject);
	}
	#endregion

	#region State
	void CkState()
	{
		switch (skullState)
		{
			case SkullState.Idle:
				//이동에 관련된 RayCast값
				setIdle();
				break;
			case SkullState.GoTarget:
			case SkullState.Move:
				setMove();
				break;
			case SkullState.Atk:
				setAtk();
				break;
			default:
				break;
		}
	}
	void setIdle()
	{
		if (targetCharactor == null)
		{
			posTarget = new Vector3(skullTransform.position.x + Random.Range(-10f, 10f),
									skullTransform.position.y + 1000f,
									skullTransform.position.z + Random.Range(-10f, 10f)
				);
			Ray ray = new Ray(posTarget, Vector3.down);
			RaycastHit infoRayCast = new RaycastHit();
			if (Physics.Raycast(ray, out infoRayCast, Mathf.Infinity) == true)
			{
				posTarget.y = infoRayCast.point.y;
			}
			skullState = SkullState.Move;
		}
		else
		{
			skullState = SkullState.GoTarget;
		}
	}

	void setMove()
	{
		//출발점 도착점 두 벡터의 차이 
		Vector3 distance = Vector3.zero;
		//어느 방향을 바라보고 가고 있느냐 
		Vector3 posLookAt = Vector3.zero;

		//해골 상태
		switch (skullState)
		{
			//해골이 돌아다니는 경우
			case SkullState.Move:
				//만약 랜덤 위치 값이 제로가 아니면
				if (posTarget != Vector3.zero)
				{
					//목표 위치에서 해골 있는 위치 차를 구하고
					distance = posTarget - skullTransform.position;

					//만약에 움직이는 동안 해골이 목표로 한 지점 보다 작으 
					if (distance.magnitude < AtkRange)
					{
						//대기 동작 함수를 호출
						StartCoroutine(setWait());
						//여기서 끝냄
						return;
					}

					//어느 방향을 바라 볼 것인. 랜덤 지역
					posLookAt = new Vector3(posTarget.x,
											//타겟이 높이 있을 경우가 있으니 y값 체크
											skullTransform.position.y,
											posTarget.z);
				}
				break;
			//캐릭터를 향해서 가는 돌아다니는  경우
			case SkullState.GoTarget:
				//목표 캐릭터가 있을 땟
				if (targetCharactor != null)
				{
					//목표 위치에서 해골 있는 위치 차를 구하고
					distance = targetCharactor.transform.position - skullTransform.position;
					//만약에 움직이는 동안 해골이 목표로 한 지점 보다 작으 
					if (distance.magnitude < AtkRange)
					{
						//공격상태로 변경합니.
						skullState = SkullState.Atk;
						//여기서 끝냄
						return;
					}
					//어느 방향을 바라 볼 것인. 랜덤 지역
					posLookAt = new Vector3(targetCharactor.transform.position.x,
											//타겟이 높이 있을 경우가 있으니 y값 체크
											skullTransform.position.y,
											targetCharactor.transform.position.z);
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
		skullTransform.Translate(amount, Space.World);
		//캐릭터 방향 정하기
		skullTransform.LookAt(posLookAt);
	}

	IEnumerator setWait()
	{
		//해골 상태를 대기 상태로 바꿈
		skullState = SkullState.Wait;
		//대기하는 시간이 오래되지 않게 설정
		float timeWait = Random.Range(1f, 3f);
		//대기 시간을 넣어 준.
		yield return new WaitForSeconds(timeWait);
		//대기 후 다시 준비 상태로 변경
		skullState = SkullState.Idle;
	}

	void setAtk()
	{
		//해골과 캐릭터간의 위치 거리 
		float distance = Vector3.Distance(targetTransform.position, skullTransform.position); //무겁다

		//공격 거리보다 둘 간의 거리가 멀어 졌다면 
		if (distance > AtkRange + 0.5f)
		{
			//타겟과의 거리가 멀어졌다면 타겟으로 이동 
			skullState = SkullState.GoTarget;
		}
	}
	#endregion
	void OnCkTarget(GameObject target)
	{
		//목표 캐릭터에 파라메터로 검출된 오브젝트를 넣고 
		targetCharactor = target;
		//목표 위치에 목표 캐릭터의 위치 값을 넣습니다. 
		targetTransform = targetCharactor.transform;

		//목표물을 향해 해골이 이동하는 상태로 변경
		skullState = SkullState.GoTarget;

	}

	void theAtk()
	{
		for (int i = 0; i < attackparticle.Length; i++)
		{
			attackparticle[i].gameObject.SetActive(true);
		}
		for (int i = 0; i < attackparticle.Length; i++)
		{
			attackparticle[i].Clear();
			attackparticle[i].Play();
		}
		Collider[] a = Physics.OverlapSphere(this.transform.position, radius, layerMask);

		if (a.Length > 0)
		{
			for(int i = 0; i<a.Length; i++)
			{
				a[i].GetComponentInParent<OnHIt>().OnHit(Atk);
			}
		}
	}
	#region Hit
	private void OnTriggerEnter(Collider other)
	{
		//만약에 해골이 캐릭터 공격에 맞았다면
		if (other.gameObject.CompareTag("PlayerAtk") == true && count < eventParam.eventint)
		{
			ani.SetBool("isDie", false);
			ani.SetBool("isDamage", true);
			ani.SetBool("isAttack", false);
			ani.SetBool("isWalk", false);
			OnHit(other.GetComponentInParent<OnHIt>().Atk);
		}
	}
	IEnumerator Wait(GameObject obj)
	{
		yield return new WaitForSeconds(1f);
		obj.transform.position = this.transform.position;
	}
	void effectDamageTween()
	{
		Color colorTo = Color.red;

		skinnedMeshRenderer.material.DOColor(colorTo, 0f).OnComplete(OnDamageTweenFinished);
	}
	void OnDamageTweenFinished()
	{
		//트윈이 끝나면 하얀색으로 확실히 색상을 돌려준다
		skinnedMeshRenderer.material.DOColor(Color.white, 2f);
	}

	void IsAttacked(EventParam events)
	{
		eventParam = events;
		if (eventParam.eventint == 0)
		{
			count = 0;
		}
	}

	public void OnHit(int atk)
	{
		count++;
		//해골 체력을 10 빼고 
		hp -= atk;
		hpBar.gameObject.SetActive(true);
		UpdateSlider();
		if (hp > 0)
		{
			//피격 이펙트 
			skullState = SkullState.Damage;
			GameObject obj = ObjectPool.Instance.GetObject(PoolObjectType.DAMAGEDEFFECT);
			obj.transform.parent = this.transform;
			obj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);


			effectDamageTween();
			//체력이 0 이상이면 피격 애니메이션을 연출 하고 
		}
		else
		{
			//0 보다 작으면 해골이 죽음 상태로 바꾸어라  
			skullState = SkullState.Die;
			effectDie.GetComponent<ParticleSystem>().Play();
			for(int i = 0; i<attackparticle.Length; i++)
			{
				attackparticle[i].gameObject.SetActive(false);
			}
			hpBar.gameObject.SetActive(false);
			GameManager.Instance.PlayerData.Money += 5;
			GameObject obj = ObjectPool.Instance.GetObject(PoolObjectType.HP);
			StartCoroutine(Wait(obj));
		}
	}

	public void UpdateSlider()
	{
		hpBar.maxValue = maxHp;
		hpBar.value = hp;
	}

	#endregion

	private void OnDestroy()
	{
		EventManager.StopListening("Attaking", IsAttacked);
	}
}