using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteminaBar : Bar
{
    private Slider stSlider;
    private void Start()
    {
        EventManager.StartListening("STMINA", UpdateHpSlider);
        stSlider = GetComponent<Slider>();
        stSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(playerDataSO.maxHp + 300, 50);
    }
    void Update()
    {
    }

    private void UpdateHpSlider(EventParam eventParam)
    {
        stSlider.maxValue = playerDataSO.maxSt;
        stSlider.value = playerDataSO.St;
    }
}
