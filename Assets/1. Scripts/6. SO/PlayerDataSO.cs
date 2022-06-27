using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
	public float maxHp;
	public float maxMp;
	public float maxSt;

	private float Hp;
	private float st;

	public float Money;
	public float hp { get => Hp; set => Hp =Mathf.Clamp(value, 0, maxHp); }
	public float Mp;
	public float St { get => st;  set => st = Mathf.Clamp(value, 0, maxSt); }

	public float damage;
}
