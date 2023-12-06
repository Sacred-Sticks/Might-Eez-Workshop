using UnityEngine;

public class MaterialDisplay : MonoBehaviour, IObserver<Resource>, ICarriedResourceDisplay
{
    [SerializeField] private Material[] materials;
    
    public void OnNotify(Resource argument)
    {
        switch (argument.GetType())
        {
            case not null when argument.GetType() == typeof(BaseMaterial):
            case not null when argument.GetType() == typeof(ProcessedMaterial):
                OnMaterialCollected(argument);
                break;
        }
    }

    private void OnMaterialCollected(Resource baseMaterial)
    {
        var meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.material = materials[ICarriedResourceDisplay.GetColorIndex(baseMaterial)];
    }
}
