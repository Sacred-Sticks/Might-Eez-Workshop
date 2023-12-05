using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class WorkstationStatusDisplay : MonoBehaviour, IObserver<Workstation.Status>
{
    [SerializeField] private Material idleMaterial;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private Material completedMaterial;
    
    private MeshRenderer renderer;

    #region UnityEvents
    private void Awake()
    {
        transform.parent.GetComponent<Workstation>().AddObserver(this);
        renderer = GetComponent<MeshRenderer>();
    }
    #endregion
    
    public void OnNotify(Workstation.Status argument)
    {
        var material = argument switch
        {
            Workstation.Status.Idle => idleMaterial,
            Workstation.Status.Active => activeMaterial,
            Workstation.Status.Completed => completedMaterial,
            _ => throw new Exception(),
        };
        SetMaterial(material);
    }

    private void SetMaterial(Material mat)
    {
        renderer.material = mat;
    }
}
