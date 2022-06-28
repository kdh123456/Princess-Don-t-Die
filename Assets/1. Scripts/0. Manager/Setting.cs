using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
	[SerializeField]
	private GameObject obj;
	[SerializeField]
	private GameObject obj2;

	private void Start()
	{
		EventManager.StartListening("ONST", OnStore);
	}

	private void OnStore(EventParam eventParam)
	{
		obj.SetActive(eventParam.input.isSetting);
	}

	public void OnStore()
	{
		obj.SetActive(false);
	}

	public void OnStores()
	{
		obj.SetActive(true);
	}

	public void OnSetting()
	{
		obj2.SetActive(true);
	}

	public void UnSetting()
	{
		obj2.SetActive(false);
	}
	private void OnDestroy()
	{
		EventManager.StopListening("ONSTORE", OnStore);
	}
}
