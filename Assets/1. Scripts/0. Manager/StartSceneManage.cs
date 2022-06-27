using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManage : MonoBehaviour
{
	public void Quit()
	{
		Application.Quit();
	}

	public void GameStart()
	{
		SceneManager.LoadScene(1);
	}
}
