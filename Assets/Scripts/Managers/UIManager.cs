using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Countdown")]
    public TextMeshProUGUI waveCountdownText;
    public GameObject waveCountdown;
    public Image fillWaveCountdown;

    public Color four;
    public Color three;
    public Color two;
    public Color one;
    public Color zero;

    [Header("Player")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI healthResult;
    public Image fillHealth;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI killResult;
    public TextMeshProUGUI waveNbText;
    public TextMeshProUGUI waveResult;

    [Header("Timer")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI msText;
    public TextMeshProUGUI timerResult;
    public TextMeshProUGUI turretResult;

    [Header("Currency")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI seedText;

    [Header("TurretUI")]
    public GameObject selectedUI;
    [Header("Turret Selected")]
    public Image turretSelectedImage;
    [Header("Turret Level")]
    public TextMeshProUGUI turretLevelText;
    public Slider turretLevelSlider;
    [Header("Turret Health")]
    public TextMeshProUGUI turretHealthText;
    public Slider turretHealthSlider;
    [Header("Turret Damage")]
    public TextMeshProUGUI turretDamageText;
    public Slider turretDamageSlider;
    [Header("Turret Firerate")]
    public TextMeshProUGUI turretFirerateText;
    public Slider turretFirerateSlider;
    [Header("Turret Range")]
    public TextMeshProUGUI turretRangeText;
    public Slider turretRangeSlider;
    [Header("Turret Actions field")]
    public TextMeshProUGUI upgradeCostText;
    public TextMeshProUGUI sellAmountText;

    [Header("End screens")]
    public GameObject inGameUI;
    public GameObject endScreen;
    public GameObject win;
    public GameObject lose;

    public GameObject pauseMenu;
    private void Start()
    {
        HideTurretUI();
    }
    public void HideTurretUI()
    {
        selectedUI.SetActive(false);
    }

    public void UpdateHealth(float _health)
    {
        turretHealthText.text = "Health : " + _health;
        turretHealthSlider.value = _health;
    }
    public void SetTurretSelected(Sprite _image, int _level,int _maxLevel, float _health,float _maxHealth, float _damage,float _maxDamage, float _firerate,float _maxFirerate, float _range, float _maxRange,float _upgradeCost, int _refund)
    {
        selectedUI.SetActive(true);
        // Image
        turretSelectedImage.sprite = _image;

        // Level
        turretLevelText.text = "Level : " + _level;
        turretLevelSlider.maxValue = _maxLevel;
        turretLevelSlider.value = _level;

        // Health
        turretHealthText.text = "Health : " + _health;
        turretHealthSlider.maxValue = _maxHealth;
        turretHealthSlider.value = _health;

        // Damage
        turretDamageText.text = "Damage : " + _damage;
        turretDamageSlider.maxValue = _maxDamage;
        turretDamageSlider.value = _damage;

        // Fire rate
        turretFirerateText.text = "Fire rate : " + _firerate;
        turretFirerateSlider.maxValue = _maxFirerate;
        turretFirerateSlider.value = _firerate;

        // Range
        turretRangeText.text = "Range : " + _range;
        turretRangeSlider.maxValue = _maxRange;
        turretRangeSlider.value = _range;

        // Upgrade
        upgradeCostText.text = _upgradeCost.ToString();

        // Refund
        sellAmountText.text = "+" + _refund;
    }
    public void SetTimerText(string ms, string timer)
    {
        msText.text = ms;
        timerText.text = timer;
        timerResult.text = "Total time : " + timer;
        turretResult.text = "Turrets built : " + BuildManager.TurretBuilt;
    }

    public void SetWaveNumber(string value)
    {
        waveNbText.text = "Wave : " + value;
        waveResult.text = "Waves survived : " + value;
    }

    public void SetCountDownText(float value)
    {
        if (value <= 5 && value > 0)
        {
            waveCountdown.SetActive(true);
            waveCountdown.transform.localScale =2 * Vector3.one * (value - Mathf.Floor(value));
            waveCountdownText.text = string.Format("{0:00}", Mathf.Ceil(value));
            fillWaveCountdown.fillAmount = (value - Mathf.Floor(value));

            if (value > 4)
                fillWaveCountdown.color = four;
            else if (value > 3)
                fillWaveCountdown.color = three;
            else if (value > 2)
                fillWaveCountdown.color = two;
            else if (value > 1)
                fillWaveCountdown.color = one;
            else
                fillWaveCountdown.color = zero;
        }
        else
        {
            waveCountdown.SetActive(false);
        }
    }

    public void SetMoneyText(string value)
    {
        moneyText.text = value;
    }

    public void SetSeedText(string value)
    {
        seedText.text = value;
    }

    public void SetHealthText(float value)
    {
        int treeNumber = FindObjectsOfType<Palm>().Length;
        healthText.text =string.Format("{0:00}" ,value) ;
        fillHealth.fillAmount = value/(treeNumber * 100);
        healthResult.text ="Total health : " + healthText.text;
    }

    public void SetKillsText(string value)
    {
        killsText.text = value;
        killResult.text = "Total kills : " + value;
    }

    public void EndScreen(string status)
    {
        inGameUI.SetActive(false);
        endScreen.SetActive(true);
        switch (status)
        {
            case "Win":
                SetWinScreen();
                break;
            case "Lose":
                SetLoseScreen();
                break;
            default:
                break;
        }
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        inGameUI.SetActive(!inGameUI.activeSelf);
    }

    public void SetWinScreen()
    {
        win.SetActive(true);
        lose.SetActive(false);
    }

    public void SetLoseScreen()
    {
        lose.SetActive(true);
        win.SetActive(false);
    }
}
