using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : Bar
{
    private Slider mpSlider;
    private void Start()
    {
        EventManager.StartListening("MANA", UpdateHpSlider);
        mpSlider = GetComponent<Slider>();
        mpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(playerDataSO.maxMp + 300, 50);
    }
    void Update()
    {
    }

    private void UpdateHpSlider(EventParam eventParam)
    {
        mpSlider.maxValue = playerDataSO.maxMp;
        mpSlider.value = playerDataSO.Mp;
    }
}
