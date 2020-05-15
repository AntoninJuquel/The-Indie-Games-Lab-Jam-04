using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class StructuresBlueprint
{
    public string StructureName;

    public GameObject[] prefab;
    public Sprite[] image;
    public CurrencyManager.Currency currency;
    public int cost;
    public int[] upgradeCost;
    public int refund;

    [Header("Level Max")]
    public int maxLevel;
    public float maxHealth;
    public float maxDamage;
    public float maxFirerate;
    public float maxRange;
}
