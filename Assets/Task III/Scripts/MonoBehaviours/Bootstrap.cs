using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Rendering;

public class Bootstrap : MonoBehaviour
{
    private float3 spaceshipDimensions = new float3(1f,1f,1f);
    private float3 thrustVector;
    private float3 thrusterPosition;
    private float spaceshipMass = 1f;
    private bool gravity;
    private bool thrusterplaced;
    private bool useWorldSpace;

    public Material spaceshipMaterial;

    public float3 SpaceshipDimensions { get { return spaceshipDimensions; } set { spaceshipDimensions = value; } }
    public float3 ThrustVector { get { return thrustVector; } set { thrustVector = value; } }
    public float3 ThrusterPosition { get { return thrusterPosition; } set { thrusterPosition = value; } }
    public float SpaceshipMass { get { return spaceshipMass; } set { spaceshipMass = value; } }
    public GameObject SpaceshipPrefab { get { return spaceshipPrefab; } set { spaceshipPrefab = value; } }
    public bool Gravity { get { return gravity; } set { gravity = value; } }
    public bool Trusterplaced { get { return thrusterplaced; } set { thrusterplaced = value; } }
    public bool UseWorldSpace { get { return useWorldSpace; } set { useWorldSpace = value; } }

    private EntityManager entityManager;
    private GameObject spaceshipPrefab;
    private Entity spaceshipEntity;


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
        spaceshipEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(spaceshipPrefab, World.Active);
        spaceshipPrefab.SetActive(false);

        entityManager.AddComponentData(spaceshipEntity, new CustomRigidbody 
        { 
            MassValue = spaceshipMass, 
            CenterOfMass = spaceshipPrefab.transform.position, 
            MomentOfInertia = CalculateInertia(), 
            Momentum = float3.zero, 
            AngularMomentum = float3.zero 
        });

        entityManager.AddComponentData(spaceshipEntity, new Velocity { Value = float3.zero });
        entityManager.AddComponentData(spaceshipEntity, new AngularVelocity { Value = float3.zero });
        entityManager.AddComponentData(spaceshipEntity, new Gravity { Value = gravity ? new float3(0f, -9.8f, 0f) : float3.zero });
       
        if(thrusterplaced)
        {
            entityManager.AddComponentData(spaceshipEntity, new ThrusterComponent
            {
                ThrustVector = thrustVector,
                ThrusterPosition = thrusterPosition    
            });
        }
    }

    public void Refresh()
    {
        spaceshipPrefab.transform.localScale = spaceshipDimensions;
    }

    public void Reset()
    {
        Entity temp = spaceshipEntity;
        entityManager.DestroyEntity(temp);

        spaceshipPrefab.SetActive(true);
        spaceshipPrefab.transform.position = Vector3.zero;
        spaceshipPrefab.transform.localScale = Vector3.one;
        spaceshipPrefab.transform.rotation = quaternion.identity;
        spaceshipDimensions = spaceshipPrefab.transform.localScale;
    }

    // https://en.wikipedia.org/wiki/List_of_moments_of_inertia#List_of_3D_inertia_tensors solid cuboid
    private float3 CalculateInertia()
    {
        return spaceshipMass / 12f * new float3
        (
            spaceshipDimensions.x * spaceshipDimensions.x + spaceshipDimensions.z * spaceshipDimensions.z,
            spaceshipDimensions.z * spaceshipDimensions.z * spaceshipDimensions.y * spaceshipDimensions.y,
            spaceshipDimensions.x * spaceshipDimensions.x + spaceshipDimensions.y * spaceshipDimensions.y
        );
    }
    

}
