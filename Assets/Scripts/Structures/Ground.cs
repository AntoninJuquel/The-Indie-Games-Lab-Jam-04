using UnityEngine;
using UnityEngine.EventSystems;


public class Ground : MonoBehaviour
{
    Material startColor;
    public Material hoverColor;
    public Material cantBuildColor;

    public GameObject structureOnMe;

    public Vector3 buildOffset;

    Renderer rend;
    BuildManager buildManager;
    private void Awake()
    {
        rend = GetComponent<Renderer>();

        startColor = rend.material;
    }

    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (structureOnMe != null || buildManager.GetTurretToBuild() == null)
        {
            rend.material = cantBuildColor;
            return;
        }

        rend.material = hoverColor;
    }

    private void OnMouseExit()
    {
        rend.material = startColor;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (structureOnMe != null)
        {
            Debug.Log("can't build here - TODO display on screen");
            return;
        }

        structureOnMe = buildManager.BuildStructureOn(this);
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + buildOffset;
    }

    public void StructureDestroyed()
    {
        structureOnMe = null;
    }
}
