using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : Bar
{
    private Slider hpSlider;
	private void Start()
	{
        EventManager.StartListening("HP", UpdateHpSlider);
        hpSlider = GetComponent<Slider>();
        hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(playerDataSO.maxHp + 300,50);
	}
	void Update()
    {
    }

    private void UpdateHpSlider(EventParam eventParam)
	{
        hpSlider.maxValue = playerDataSO.maxHp;
        hpSlider.value = playerDataSO.hp;
    }
}
