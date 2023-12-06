using UnityEngine;

public class ToyDisplay : MonoBehaviour, IObserver<Resource>, ICarriedResourceDisplay
{
    [SerializeField] private GameObject leg1;
    [SerializeField] private GameObject leg2;
    [SerializeField] private GameObject arm1;
    [SerializeField] private GameObject arm2;
    [SerializeField] private GameObject torso;
    [SerializeField] private GameObject head;

    [SerializeField] private Material[] materials;
    
    public void OnNotify(Resource argument)
    {
        switch (argument)
        {
            case not null when argument.GetType() == typeof(Toy):
                OnToyCollected(argument as Toy);
                break;
        }
    }

    private void OnToyCollected(Toy toy)
    {
        int armCount = 0;
        int legCount = 0;
        
        foreach (var part in toy.ToyParts)
        {
            var toyPart = GetToyPartType(part, ref legCount, ref armCount);
            var meshRenderer = toyPart.GetComponentInChildren<MeshRenderer>();
            meshRenderer.material = materials[ICarriedResourceDisplay.GetColorIndex(part)];
        }
    }

    private GameObject GetToyPartType(ToyPart toyPart, ref int legCount, ref int armCount)
    {
        var toyPartGameObject = gameObject;
        switch (toyPart.Part)
        {
            case ToyPart.ToySection.Leg when legCount == 0:
                toyPartGameObject = leg1;
                legCount++;
                break;
            case ToyPart.ToySection.Leg:
                toyPartGameObject = leg2;
                break;
            case ToyPart.ToySection.Arm when armCount == 0:
                toyPartGameObject = arm1;
                armCount++;
                break;
            case ToyPart.ToySection.Arm:
                toyPartGameObject = arm2;
                break;
            case ToyPart.ToySection.Torso:
                toyPartGameObject = torso;
                break;
            case ToyPart.ToySection.Head:
                toyPartGameObject = head;
                break;
        }
        return toyPartGameObject;
    }
}