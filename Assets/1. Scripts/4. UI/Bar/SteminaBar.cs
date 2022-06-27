using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteminaBar : MonoBehaviour
{
    private Slider playerStSlider;
    private void Start()
    {
        EventManager.StartListening("STMINA", UpdateHpSlider);
        playerStSlider = GetComponent<Slider>();
        playerStSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(GameManager.Instance.PlayerData.maxSt + 300, 50);
    }
    void Update()
    {
    }

    private void UpdateHpSlider(EventParam eventParam)
    {
        playerStSlider.maxValue = GameManager.Instance.PlayerData.maxSt;
        playerStSlider.value = GameManager.Instance.PlayerData.St;
    }
}
