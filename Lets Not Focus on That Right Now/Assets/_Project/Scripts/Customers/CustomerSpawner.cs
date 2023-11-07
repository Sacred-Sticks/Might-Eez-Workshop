using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerSpwaner : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Range<float, float> spawnRate;
    [Space]
    [SerializeField] private Range<float, float> customerPatienceRange;
    [SerializeField] private Range<float, float> customerPriceRange;

    private IEnumerator Start()
    {
        for (;;)
        {
            var customer = Instantiate(customerPrefab, transform.position, transform.rotation);
            new Customer.Builder()
                .WithPatience(Random.Range(customerPatienceRange.Minimum, customerPatienceRange.Maximum))
                .WithPrice(Random.Range(customerPriceRange.Minimum, customerPriceRange.Maximum))
                .WithToy(CreateToyTemplate())
                .Build(customer);
            yield return new WaitForSeconds(1 / Random.Range(spawnRate.Minimum, spawnRate.Maximum));
        }
    }

    private Toy CreateToyTemplate()
    {
        var leftArm = GenerateToyPart(ToyPart.ToySection.Arm);
        var rightArm = GenerateToyPart(ToyPart.ToySection.Arm);
        var leftLeg = GenerateToyPart(ToyPart.ToySection.Leg);
        var rightLeg = GenerateToyPart(ToyPart.ToySection.Leg);
        var torso = GenerateToyPart(ToyPart.ToySection.Torso);
        var head = GenerateToyPart(ToyPart.ToySection.Head);
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

    private ToyPart GenerateToyPart(ToyPart.ToySection toySection)
    {
        var materialType = GetRandomEnumValue<Resource.MaterialType>();
        // get random color here too
        var baseMaterial = ToyFactory.CreateMaterial(materialType);
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
