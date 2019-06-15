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
    private float spaceshipMass;
    private bool gravity;
    private bool thrusterplaced;

    public Material spaceshipMaterial;

    public float3 SpaceshipDimensions { get { return spaceshipDimensions; } set { spaceshipDimensions = value; } }
    public float3 ThrustVector { get { return thrustVector; } set { thrustVector = value; } }
    public float3 ThrusterPosition { get { return thrusterPosition; } set { thrusterPosition = value; } }
    public float SpaceshipMass { get { return spaceshipMass; } set { spaceshipMass = value; } }
    public GameObject SpaceshipPrefab { get { return spaceshipPrefab; } set { spaceshipPrefab = value; } }
    public bool Gravity { get { return gravity; } set { gravity = value; } }
    public bool Trusterplaced { get { return thrusterplaced; } set { thrusterplaced = value; } }

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
        entityManager.AddComponentData(spaceshipEntity, new CustomRigidbody { MassValue = spaceshipMass, CenterOfMass = float3.zero, MomentOfInertia = float3x3.identity });
        entityManager.AddComponentData(spaceshipEntity, new Velocity { Value = float3.zero });
        entityManager.AddComponentData(spaceshipEntity, new AngularVelocity { Value = float3.zero });
        entityManager.AddComponentData(spaceshipEntity, new Gravity { Value = gravity ? new float3(0f, -9.8f, 0f) : float3.zero });
       
        if(thrusterplaced)
        {
            entityManager.AddComponentData(spaceshipEntity, new ThrusterComponent { ThrustVector = thrustVector, ThrusterPosition = thrusterPosition });
        }
    }

    public void Refresh()
    {
        spaceshipPrefab.transform.localScale = spaceshipDimensions;
    }
    

}
