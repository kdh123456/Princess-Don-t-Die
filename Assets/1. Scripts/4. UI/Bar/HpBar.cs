using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField]
    private Slider PlayerhpSlider;
    [SerializeField]
    private RectTransform PlayerhpSliderRect;
    [SerializeField]
    private Slider PrincesshpSlider;
    [SerializeField]
    private RectTransform PrincesshpSliderRect;

    private void Start()
	{
        EventManager.StartListening("PUPDATESLIDER", PlayerUpdateHpSlider);
        EventManager.StartListening("PRUPDATESLIDER", PrincessUpdateHpSlider);
		PlayerhpSliderRect.sizeDelta = new Vector2(GameManager.Instance.PlayerData.maxHp + 300, 50);
		PrincesshpSliderRect.sizeDelta = new Vector2(GameManager.Instance.PrincessData.maxHp+300, 50);
	}
	void Update()
    {
    }

    private void PlayerUpdateHpSlider(EventParam eventParam)
	{
        PlayerhpSlider.maxValue = GameManager.Instance.PlayerData.maxHp;
        PlayerhpSlider.value = GameManager.Instance.PlayerData.hp;
    }

    private void PrincessUpdateHpSlider(EventParam eventParam)
	{
        PrincesshpSlider.maxValue = GameManager.Instance.PrincessData.maxHp;
        PrincesshpSlider.value = GameManager.Instance.PrincessData.hp;
	}

	private void OnDestroy()
	{
        EventManager.StopListening("PUPDATESLIDER", PlayerUpdateHpSlider);
        EventManager.StopListening("PRUPDATESLIDER", PrincessUpdateHpSlider);
    }
}
