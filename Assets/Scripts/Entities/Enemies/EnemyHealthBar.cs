using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthBar : MonoBehaviour
{
    public GameObject ui;
    public Slider healthBar;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI nameText;

    private void Start()
    {
        DisableUi();
    }

    public void SetUi(float _value, int _level,string _name)
    {
        healthBar.maxValue = _value;
        healthBar.value = _value;
        levelText.text = "Level : " + _level;
        nameText.text = _name;
    }

    public void UpdateHealthBar(float _value)
    {
        healthBar.value = _value;
    }

    public void EnableUi()
    {
        ui.SetActive(true);
    }

    public void DisableUi()
    {
        ui.SetActive(false);
    }

    private void OnMouseOver()
    {
        EnableUi();
    }

    private void OnMouseExit()
    {
        DisableUi();
    }
}
