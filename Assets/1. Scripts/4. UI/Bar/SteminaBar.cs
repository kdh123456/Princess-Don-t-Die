using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteminaBar : MonoBehaviour
{
    private Slider playerStSlider;
    private void Start()
    {
        EventManager.StartListening("PUPDATESLIDER", PlayerUpdateStaminaSlider);
        playerStSlider = GetComponent<Slider>();
        playerStSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(GameManager.Instance.PlayerData.maxSt + 300, 50);
    }
    void Update()
    {
    }

    private void PlayerUpdateStaminaSlider(EventParam eventParam)
    {
        playerStSlider.maxValue = GameManager.Instance.PlayerData.maxSt;
        playerStSlider.value = GameManager.Instance.PlayerData.St;
    }

	private void OnDestroy()
	{
        EventManager.StopListening("PUPDATESLIDER", PlayerUpdateStaminaSlider);
    }
}
