using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;
    void Start()
    {
        buildManager = BuildManager.instance;
    }
    public void ChooseStructure(int structureIndex)
    {
        buildManager.SetStructureToBuild(structureIndex);
    }
}
