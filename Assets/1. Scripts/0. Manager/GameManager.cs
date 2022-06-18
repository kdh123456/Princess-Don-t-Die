using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private PlayerDataSO playerDataSO;
    public PlayerDataSO PlayerData => playerDataSO;

	private void Start()
	{
		playerDataSO.hp = PlayerData.maxHp;
	}
}
