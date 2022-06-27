using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Princess/PrincessData")]
public class PrincessDataSO : ScriptableObject
{
	public int maxHp;
	public int maxAtk;

	public int hp;
	public int atk;
}
