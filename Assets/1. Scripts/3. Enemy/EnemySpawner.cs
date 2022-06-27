using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public PoolObjectType[] enemyObjects { get => GameManager.Instance.stageSO[GameManager.Instance.StageCount].enemyObjects; set => enemyObjects = GameManager.Instance.stageSO[GameManager.Instance.StageCount].enemyObjects;}
	public int[] prefabCount { get => GameManager.Instance.stageSO[GameManager.Instance.StageCount].prefabCount; }
	public float waitforCreateSecound { get => GameManager.Instance.stageSO[GameManager.Instance.StageCount].waitForCreateSecound; }

	public int round;

	private int j;
	private int i;

	private void Start()
	{
		for(i = 0; i<enemyObjects.Length; i++)
		{
			StartCoroutine(StageStart(i));
		}
	}

	private void Update()
	{
		EndStage();
	}

	private IEnumerator StageStart(int ii)
	{
		GameObject obj = ObjectPool.Instance.GetObject(enemyObjects[ii]);
		Vector3 a = Random.onUnitSphere;
		obj.transform.parent = this.transform;
		obj.transform.position = new Vector3(a.x, 0, a.z) * Random.Range(0f, round) + this.transform.position;
		yield return new WaitForSeconds(waitforCreateSecound);
		if (j != prefabCount[ii])
			StartCoroutine(StageStart(ii));
		else
			j = 0;
	}

	private void EndStage()
	{
		if(this.gameObject.transform.childCount <= 0)
		{
			GameManager.Instance.StageCount++;
			this.transform.position = GameManager.Instance.stageSO[GameManager.Instance.StageCount].stagePos;
			enemyObjects = GameManager.Instance.stageSO[GameManager.Instance.StageCount].enemyObjects;
			gameObject.SetActive(false);
		}
	}
}