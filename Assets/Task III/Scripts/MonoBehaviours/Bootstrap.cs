using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;

public class Bootstrap : MonoBehaviour
{
    private float3 spaceshipDimensions = new float3(1f,1f,1f);
    private float spaceshipMass;

    public Material spaceshipMaterial;
    public bool Gravity;

    public float3 SpaceshipDimensions { get { return spaceshipDimensions; } set { spaceshipDimensions = value; } }
    public float SpaceshipMass { get { return spaceshipMass; } set { spaceshipMass = value; } }
    public GameObject SpaceshipPrefab { get { return spaceshipPrefab; } set { spaceshipPrefab = value; } }

    private EntityManager entityManager;
    private GameObject spaceshipPrefab;

    private void Start()
    {
        entityManager = World.Active.EntityManager;

        spaceshipPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        spaceshipPrefab.GetComponent<MeshRenderer>().material = spaceshipMaterial;
        spaceshipPrefab.transform.position = Vector3.zero;
        spaceshipPrefab.transform.localScale = spaceshipDimensions;
        spaceshipPrefab.tag = "Cube";
    }

    public void RunSimulation()
    {
        Entity spaceshipEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(spaceshipPrefab, World.Active);
        Destroy(spaceshipPrefab);

        entityManager.AddComponentData(spaceshipEntity, new SpaceshipbodyComponent { Dimensions = spaceshipDimensions, Mass = spaceshipMass });
        entityManager.AddComponentData(spaceshipEntity, new CustomRigidbody { });
        entityManager.AddComponentData(spaceshipEntity, new Velocity { Value = new float3(0f, 0f, 0f) });
        entityManager.AddComponentData(spaceshipEntity, new Gravity { Value = Gravity ? new float3(0f, -9.8f, 0f) : new float3(0f, 0f, 0f) });
    }

    public void Refresh()
    {
        spaceshipPrefab.transform.localScale = spaceshipDimensions;
    }
    

}
