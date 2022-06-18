using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
	public float maxHp;
	public float maxMp;
	public float maxSt;

	public float hp;
	public float Mp;
	public float St;

	public float damage;
}
