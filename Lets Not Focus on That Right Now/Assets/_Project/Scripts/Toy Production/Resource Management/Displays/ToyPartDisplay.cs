using UnityEngine;

public class ToyPartDisplay : MonoBehaviour, IObserver<Resource>, ICarriedResourceDisplay
{
    [Header("Toy Parts")]
    [SerializeField] private GameObject leg;
    [SerializeField] private GameObject arm;
    [SerializeField] private GameObject torso;
    [SerializeField] private GameObject head;

    [Header("Colors")]
    [SerializeField] private Material[] materials;

    private GameObject activePart;

    public void OnNotify(Resource argument)
    {
        switch (argument)
        {
            case not null when argument.GetType() == typeof(ToyPart):
                OnToyPartCollected(argument as ToyPart);
                break;
        }
    }

    private void OnToyPartCollected(ToyPart toyPart)
    {
        activePart?.SetActive(false);
        activePart = toyPart.Part switch
        {
            ToyPart.ToySection.Leg => leg,
            ToyPart.ToySection.Arm => arm,
            ToyPart.ToySection.Head => head,
            ToyPart.ToySection.Torso => torso,
            _ => activePart,
        };
        activePart.SetActive(true);
        activePart.GetComponent<MeshRenderer>().material = materials[ICarriedResourceDisplay.GetColorIndex(toyPart)];
    }


}