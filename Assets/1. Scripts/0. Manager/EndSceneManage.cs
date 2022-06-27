using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneManage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("SceneStart", ReturnScene);
        EventManager.StartListening("Clear", ClearScene);
        EventManager.StartListening("Dead", DeadScene);
    }

    public void ReturnScene(EventParam eventParam)
	{
        SceneManager.LoadScene(1);
	}
    public void ReturnScene()
	{
        SceneManager.LoadScene(1);
    }
    public void TitleScene()
	{
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ClearScene(EventParam eventParam)
	{
        SceneManager.LoadScene(3);
	}

    public void DeadScene(EventParam eventParam)
	{
        SceneManager.LoadScene(2);
	}

	private void OnDestroy()
	{
        EventManager.StopListening("SceneStart", ReturnScene);
        EventManager.StopListening("Clear", ClearScene);
        EventManager.StopListening("Dead", DeadScene);
    }
}
