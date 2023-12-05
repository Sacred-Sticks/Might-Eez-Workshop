using System;
using Kickstarter.Events;
using UnityEngine;
using IServiceProvider = Kickstarter.Events.IServiceProvider;

public class ResourceDisplay : MonoBehaviour, IServiceProvider
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
        int displayIndex = resource.GetType() switch
        {
            not null when resource.GetType() == typeof(BaseMaterial) => 0,
            not null when resource.GetType() == typeof(ProcessedMaterial) => 1,
            not null when resource.GetType() == typeof(ToyPart) => 2,
            not null when resource.GetType() == typeof(Toy) => 3,
            _ => -1,
        };

        if (currentActiveIndex != -1)
            resourceDisplays[currentActiveIndex].SetActive(false);
        currentActiveIndex = displayIndex;
        if (currentActiveIndex != -1)
            resourceDisplays[currentActiveIndex].SetActive(true);
    }
}
