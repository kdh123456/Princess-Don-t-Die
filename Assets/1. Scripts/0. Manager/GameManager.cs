using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
	[SerializeField]
	private PlayerDataSO playerDataSO;
	[SerializeField]
	private PrincessDataSO princessDataSO;

	public StageSO[] stageSO;

	public PlayerDataSO PlayerData { get => playerDataSO; set => playerDataSO = value;}
	public PrincessDataSO PrincessData => princessDataSO;

	public EnemySpawner enemySpawner;

	public int StageCount = 0;

	public GameObject PointedObjet;

	private void Start()
	{
		playerDataSO.hp = playerDataSO.maxHp;
		princessDataSO.hp = princessDataSO.maxHp;
	}

	private void Update()
	{
		//Pointed();
	}

	private void Pointed()
	{
		if (!enemySpawner.isActiveAndEnabled)
		{

		}
	}
}
