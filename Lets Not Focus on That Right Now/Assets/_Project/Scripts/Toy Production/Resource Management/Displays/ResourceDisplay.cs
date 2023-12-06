using System;
using Kickstarter.Events;
using Kickstarter.Observer;
using UnityEngine;
using IServiceProvider = Kickstarter.Events.IServiceProvider;

public class ResourceDisplay : Observable, IServiceProvider
{
    [SerializeField] private Service showCurrentResource;

    [SerializeField] private GameObject[] resourceDisplays = new GameObject[4];
    private int currentActiveIndex = -1;

    #region Unity Events
    private void OnEnable()
    {
        showCurrentResource.Event += ImplementService;
    }

    private void OnDisable()
    {
        showCurrentResource.Event -= ImplementService;
    }

    private void Start()
    {
        AddObservers(GetComponentsInChildren<ICarriedResourceDisplay>());
        foreach (var resourceDisplay in resourceDisplays)
            resourceDisplay.SetActive(false);
    }
    #endregion

    public void ImplementService(EventArgs args)
    {
        switch (args)
        {
            case ResourceCarrier.ShowResource showResource:
                ShowCurrentResource(showResource.Resource);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ShowCurrentResource(Resource resource)
    {
        const int baseMaterial = 0;
        const int processedMaterial = 1;
        const int toyPart = 2;
        const int toy = 3;
        const int invalid = -1;
        
        int displayIndex = resource.GetType() switch
        {
            not null when resource.GetType() == typeof(BaseMaterial) => baseMaterial,
            not null when resource.GetType() == typeof(ProcessedMaterial) => processedMaterial,
            not null when resource.GetType() == typeof(ToyPart) => toyPart,
            not null when resource.GetType() == typeof(Toy) => toy,
            _ => invalid,
        };

        if (currentActiveIndex != invalid)
            resourceDisplays[currentActiveIndex].SetActive(false);
        currentActiveIndex = displayIndex;
        if (currentActiveIndex != invalid)
            resourceDisplays[currentActiveIndex].SetActive(true);
        NotifyObservers(resource);
    }
}
