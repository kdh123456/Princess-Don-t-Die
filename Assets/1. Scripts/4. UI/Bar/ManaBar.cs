using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    private Slider mpSlider;
    private void Start()
    {
        EventManager.StartListening("MANA", UpdateHpSlider);
        mpSlider = GetComponent<Slider>();
        mpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(GameManager.Instance.PlayerData.maxMp + 300, 50);
    }
    void Update()
    {
    }

    private void UpdateHpSlider(EventParam eventParam)
    {
        mpSlider.maxValue = GameManager.Instance.PlayerData.maxMp;
        mpSlider.value = GameManager.Instance.PlayerData.Mp;
    }
}
