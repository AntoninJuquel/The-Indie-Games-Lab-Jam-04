using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    CurrencyManager currencyManager;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one BuildManager in scene");
            return;
        }

        instance = this;
        currencyManager = GetComponent<CurrencyManager>();
        UIManager = GetComponent<UIManager>();

    }

    UIManager UIManager;

    public GameObject buildEffect;
    public StructuresBlueprint[] structures;

    private StructuresBlueprint structureToBuild;

    public GameObject selectedTurret;

    public List<List<Transform>> myLists;
    public List<Transform> turretsOnGround;
    public List<Transform> treesOnGround;
    public List<Transform> snipersOnGround;
    public List<Transform> rocketsOnGround;
    public List<Transform> lasersOnGround;
    public List<Transform> mortarsOnGround;
    public static int TurretBuilt;

    private void Start()
    {
        myLists = new List<List<Transform>>
        {
            turretsOnGround,
            treesOnGround,
            snipersOnGround,
            rocketsOnGround,
            lasersOnGround,
            mortarsOnGround
        };
    }

    private void Update()
    {
        if (selectedTurret)
        {
            UIManager.UpdateHealth(selectedTurret.GetComponent<Health>().health);
        }

        if(selectedTurret == null)
            UIManager.HideTurretUI();

        if (Input.GetKeyDown(InputManager.instance.deselect) )
        {
            UIManager.HideTurretUI();
            structureToBuild = null;
        }
    }

    public void AddInLists(Transform turret)
    {
        turretsOnGround.Add(turret);

        TurretStatus.Category category = turret.GetComponent<TurretStatus>().category;

        switch (category)
        {
            case TurretStatus.Category.laser:
                lasersOnGround.Add(turret);
                break;
            case TurretStatus.Category.mortar:
                mortarsOnGround.Add(turret);
                break;
            case TurretStatus.Category.rocket:
                rocketsOnGround.Add(turret);
                break;
            case TurretStatus.Category.tree:
                treesOnGround.Add(turret);
                break;
            case TurretStatus.Category.sniper:
                snipersOnGround.Add(turret);
                break;
            default:
                
                break;
        }
    }

    public void RemoveFromLists(Transform turret)
    {
        foreach (List<Transform> list in myLists)
            if (list.Contains(turret))
                list.Remove(turret);
    }

    public StructuresBlueprint GetTurretToBuild()
    {
        if (structureToBuild == null)
            return null;

        if (!currencyManager.EnoughMoney(structureToBuild.cost, structureToBuild.currency))
            return null;

        return structureToBuild;
    }

    public void SetStructureToBuild(int turretIndex)
    {
        if (turretIndex >= structures.Length)
        {
            structureToBuild = null;
            return;
        }


        if (!currencyManager.EnoughMoney(structures[turretIndex].cost, structures[turretIndex].currency))
        {
            structureToBuild = null;
            return;
        }
        structureToBuild = structures[turretIndex];
        return;
    }

    public GameObject BuildStructureOn(Ground ground)
    {
        if (structureToBuild == null)
            return null;
        if (!currencyManager.EnoughMoney(structureToBuild.cost, structureToBuild.currency))
        {
            return null;
        }

        GameObject structure = (GameObject)Instantiate(structureToBuild.prefab[0], ground.GetBuildPosition(), Quaternion.identity);
        structure.GetComponent<TurretStatus>().sittingOn = ground;
        structure.GetComponent<TurretStatus>().myInfo = structureToBuild;

        structure.GetComponent<TurretStatus>().SelectThisTurret();

        Instantiate(buildEffect, ground.GetBuildPosition(), Quaternion.identity);
        currencyManager.SetMoney(-structureToBuild.cost, structureToBuild.currency);

        AddInLists(structure.transform);
        TurretBuilt++;
        return structure;
    }

    public void SelectTurret(GameObject _turret, StructuresBlueprint structure, int _level, float _health, float _damage, float _fireRate, float _range)
    {
        selectedTurret = _turret;
        UIManager.SetTurretSelected(structure.image[_level - 1], _level, structure.maxLevel, _health, structure.maxHealth, _damage, structure.maxDamage, _fireRate, structure.maxFirerate, _range, structure.maxRange,structure.upgradeCost[_level-1], structure.refund);
    }

    public void UpgradeTurret()
    {
        if (selectedTurret.GetComponent<Palm>())
            return;
        if (selectedTurret.GetComponent<TurretStatus>().myInfo.upgradeCost[selectedTurret.GetComponent<TurretStatus>().level - 1] > CurrencyManager.Seed)
            return;

        if (selectedTurret.GetComponent<TurretStatus>().level < selectedTurret.GetComponent<TurretStatus>().myInfo.maxLevel)
        {
            selectedTurret.GetComponent<TurretStatus>().level++;
            int level = selectedTurret.GetComponent<TurretStatus>().level;
            Ground ground = selectedTurret.GetComponent<TurretStatus>().sittingOn;

            StructuresBlueprint blueprint = selectedTurret.GetComponent<TurretStatus>().myInfo;

            RemoveFromLists(selectedTurret.transform);

            selectedTurret.GetComponent<TurretStatus>().sittingOn.StructureDestroyed();
            
            Destroy(selectedTurret);

            GameObject structure = (GameObject)Instantiate(blueprint.prefab[level - 1], ground.GetBuildPosition(), Quaternion.identity);

            AddInLists(structure.transform);

            structure.GetComponent<TurretStatus>().sittingOn = ground;
            ground.structureOnMe = structure;
            structure.GetComponent<TurretStatus>().myInfo = blueprint;
            structure.GetComponent<TurretStatus>().level = level;

            Instantiate(buildEffect, ground.GetBuildPosition(), Quaternion.identity);
            currencyManager.SetMoney(-structure.GetComponent<TurretStatus>().myInfo.upgradeCost[level - 2], CurrencyManager.Currency.seed);

            structure.GetComponent<TurretStatus>().SelectThisTurret();
            selectedTurret = structure;
        }
    }

    public void SellTurret()
    {
        if (selectedTurret.GetComponent<Palm>())
            return;

        RemoveFromLists(selectedTurret.transform);
        selectedTurret.GetComponent<TurretStatus>().sittingOn.StructureDestroyed();
        currencyManager.SetMoney(selectedTurret.GetComponent<TurretStatus>().myInfo.refund, CurrencyManager.Currency.seed);

        Instantiate(buildEffect, selectedTurret.transform.position, Quaternion.identity);

        Destroy(selectedTurret);
    }
}
