using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerSpwaner : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Range<float, float> spawnDelay;
    [Space]
    [SerializeField] private Range<float, float> customerPatienceRange;
    [SerializeField] private Range<float, float> customerPriceRange;

    private IEnumerator Start()
    {
        for (;;)
        {
            var customerGO = Instantiate(customerPrefab, transform.position, transform.rotation);
            var customer = new Customer.Builder()
                .WithPatience(Random.Range(customerPatienceRange.Minimum, customerPatienceRange.Maximum))
                .WithPrice(Random.Range(customerPriceRange.Minimum, customerPriceRange.Maximum))
                .WithToy(CreateToyTemplate())
                .Build(customerGO);
            OrderManager.AddCustomer(customer);
            yield return new WaitForSeconds(Random.Range(spawnDelay.Minimum, spawnDelay.Maximum));
        }
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

    private static T GetRandomEnumValue<T>() where T : Enum
    {
        var random = new System.Random();
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(random.Next(values.Length));
    }

    [Serializable]
    private class Range<TType1, TType2>
    {
        [SerializeField] private TType1 minimum;
        [SerializeField] private TType2 maximum;

        public TType1 Minimum => minimum;
        public TType2 Maximum => maximum;
    }
}
