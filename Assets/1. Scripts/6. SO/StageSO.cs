using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Stage/StageData")]
public class StageSO : ScriptableObject
{
	public Vector3 stagePos;
	public PoolObjectType[] enemyObjects;
	public int[] prefabCount;
	public int waitForCreateSecound;
}
