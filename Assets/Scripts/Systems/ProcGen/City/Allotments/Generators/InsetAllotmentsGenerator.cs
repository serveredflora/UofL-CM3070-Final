using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProcGen/Allotments/Inset", fileName = "InsetAllotmentsGenerator")]
public class InsetAllotmentsGenerator : AAllotmentsGenerator
{
    [Header("Settings")]
    public float AllotmentThickness = 2.0f;
    [Range(0.0f, 1.0f)]
    public float BuildingTypeRatio = 0.9f;

    [Header("References")]
    public GameObject AllotmentPrefab;

    public override List<Allotment> GenerateAllotments(ProcGenGeneratorUtility generatorUtility, RoadNetwork roadNetwork)
    {
        var categoryGameObj = generatorUtility.RequestCategoryGameObject("Allotment");

        var allotments = new List<Allotment>();

        float insetAmount = roadNetwork.Thickness;
        foreach (var face in roadNetwork.Faces)
        {
            Allotment allotment = new Allotment();
            allotments.Add(allotment);

            // TODO: ensure at least one building exists regardless of the ratio set
            allotment.Type = Random.Range(0.0f, 1.0f) < BuildingTypeRatio ? AllotmentType.Building : AllotmentType.Park;
            allotment.Bounds = face.Bounds;
            allotment.Bounds.ExpandXZ(-insetAmount);
            allotment.Bounds.center += Vector3.up * AllotmentThickness;

            Vector3 position = allotment.Bounds.center;
            Quaternion rotation = Quaternion.identity;

            var allotmentGameObj = GameObject.Instantiate(AllotmentPrefab, position, rotation, categoryGameObj.transform);
            allotment.GameObjectInstance = allotmentGameObj;

            allotmentGameObj.transform.localScale = allotment.Bounds.size.WithY(AllotmentThickness);
        }

        return allotments;
    }
}
