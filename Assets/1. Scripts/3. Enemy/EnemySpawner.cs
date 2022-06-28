using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public PoolObjectType[] enemyObjects;
	public int[] prefabCount;
	public float waitforCreateSecound;

	public int round;

	public GameObject objs;

	private void Start()
	{
		enemyObjects = GameManager.Instance.stageSO[GameManager.Instance.StageCount].enemyObjects;
		prefabCount = GameManager.Instance.stageSO[GameManager.Instance.StageCount].prefabCount;
		waitforCreateSecound = GameManager.Instance.stageSO[GameManager.Instance.StageCount].waitForCreateSecound;

		this.transform.position = GameManager.Instance.stageSO[GameManager.Instance.StageCount].stagePos;
	}

	private IEnumerator StageStart(int ii)
	{
		GameObject obj = ObjectPool.Instance.GetObject(enemyObjects[ii]);
		Vector3 a = Random.onUnitSphere;
		obj.transform.parent = objs.transform;
		obj.transform.position = new Vector3(a.x, 0, a.z) * Random.Range(0f, round) + objs.transform.position;
		yield return new WaitForSeconds(waitforCreateSecound);
		if (this.transform.childCount < prefabCount[ii])
			StartCoroutine(StageStart(ii));
		else
		{
			if (GameManager.Instance.StageCount < GameManager.Instance.stageSO.Length)
			{
				enemyObjects = GameManager.Instance.stageSO[GameManager.Instance.StageCount].enemyObjects;
				waitforCreateSecound = GameManager.Instance.stageSO[GameManager.Instance.StageCount].waitForCreateSecound;
			}
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			for (int i = 0; i < enemyObjects.Length; i++)
			{
				if (GameManager.Instance.StageCount < GameManager.Instance.stageSO.Length)
				{
					objs.transform.position = GameManager.Instance.stageSO[GameManager.Instance.StageCount].stagePos;
					GameManager.Instance.StageCount++;
					this.transform.position = GameManager.Instance.stageSO[GameManager.Instance.StageCount].stagePos;
				}
				StartCoroutine(StageStart(i));
			}
		}
	}


}