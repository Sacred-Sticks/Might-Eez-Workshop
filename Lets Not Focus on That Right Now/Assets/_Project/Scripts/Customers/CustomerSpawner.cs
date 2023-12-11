using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Range<float, float> spawnDelay;
    [SerializeField] private float spawnDelayDepreciation;
    [Space]
    [SerializeField] private Vector3 orderPosition;
    [SerializeField] private Vector3 waitingPosition;
    [SerializeField] private Range<float, float> customerPatienceRange;
    [SerializeField] private Range<float, float> customerPriceRange;

    [SerializeField] private float gameTime;

    public static CustomerSpawner instance;
    public Range<float, float> SpawnDelay => spawnDelay;

    private void Awake()
    {
        InitializeSingleton();
    }

    private IEnumerator Start()
    {
        StartCoroutine(GameTimer());
        for (;;)
        {
            yield return CreateCustomer(spawnDelay);
        }
    }

    public object CreateCustomer(Range<float, float> spawnDelay)
    {
        var customerGO = Instantiate(customerPrefab, transform.position + Vector3.up, transform.rotation, transform);
        var customer = new Customer.Builder()
            .WithPatience(Random.Range(customerPatienceRange.Minimum, customerPatienceRange.Maximum))
            .WithPrice(Random.Range(customerPriceRange.Minimum, customerPriceRange.Maximum))
            .WithOrderPosition(orderPosition)
            .WithWaitingPosition(waitingPosition)
            .WithToy(CreateToyTemplate())
            .Build(customerGO);
        OrderManager.AddCustomer(customer);
        spawnDelay.Minimum -= spawnDelayDepreciation;
        spawnDelay.Maximum -= spawnDelayDepreciation;
        return new WaitForSeconds(Random.Range(spawnDelay.Minimum, spawnDelay.Maximum));
    }

    private void InitializeSingleton()
    {
        if (instance == null)
            instance = this;
    }
    
    private Toy CreateToyTemplate()
    {
        var materialType = GetRandomEnumValue<Resource.MaterialType>();

        var leftArm = GenerateToyPart(ToyPart.ToySection.Arm, materialType);
        var rightArm = GenerateToyPart(ToyPart.ToySection.Arm, materialType);
        var leftLeg = GenerateToyPart(ToyPart.ToySection.Leg, materialType);
        var rightLeg = GenerateToyPart(ToyPart.ToySection.Leg, materialType);
        var torso = GenerateToyPart(ToyPart.ToySection.Torso, materialType);
        var head = GenerateToyPart(ToyPart.ToySection.Head, materialType);
        var parts = new[]
        {
            leftArm,
            rightArm,
            leftLeg,
            rightLeg,
            torso,
            head,
        };
        return ToyFactory.AssembleToy(parts);
    }

    private ToyPart GenerateToyPart(ToyPart.ToySection toySection, Resource.MaterialType materialType)
    {
        var colorType = GetRandomEnumValue<Resource.MaterialColor>();
        var baseMaterial = ToyFactory.CreateMaterial(materialType, colorType);
        var processedMaterial = ToyFactory.ProcessMaterial(baseMaterial);
        return ToyFactory.ConstructToyPart(processedMaterial, toySection);
    }

    private IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(gameTime);
        GameManager.instance.EndGame(GameManager.EndGameStatus.Win);
    }

    private static T GetRandomEnumValue<T>() where T : Enum
    {
        var random = new System.Random();
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(random.Next(values.Length));
    }

    [Serializable]
    public class Range<TType1, TType2>
    {
        [field: SerializeField] public TType1 Minimum { get; set; }
        [field: SerializeField] public TType2 Maximum { get; set; }
    }
}