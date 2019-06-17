using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;

public class Bootstrap : MonoBehaviour
{
    public Material spaceshipMaterial;

    private float3 spaceshipDimensions = new float3(1f,1f,1f);
    private float3 thrustVector;
    private float3 thrusterPosition;
    private float spaceshipMass = 1f;
    private bool gravity;
    private bool thrusterplaced;

    public float3 SpaceshipDimensions   { get { return spaceshipDimensions; } set { spaceshipDimensions = value; } }
    public float3 ThrustVector          { get { return thrustVector; } set { thrustVector = value; } }
    public float3 ThrusterPosition      { get { return thrusterPosition; } set { thrusterPosition = value; } }
    public float SpaceshipMass          { get { return spaceshipMass; } set { spaceshipMass = value; } }
    public GameObject SpaceshipPrefab   { get { return spaceshipPrefab; } set { spaceshipPrefab = value; } }
    public bool Gravity                 { get { return gravity; } set { gravity = value; } }
    public bool Trusterplaced           { get { return thrusterplaced; } set { thrusterplaced = value; } }

    private EntityManager entityManager;
    private GameObject spaceshipPrefab;
    private Entity spaceshipEntity;

    //In start we are first instantiating our cube as a regular game object. It will remain a game object for the entire setup period and
    //will be transformed into an entity once we press "Run Simulation"
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
        //Converting gameobject into an entity
        spaceshipEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(spaceshipPrefab, World.Active);

        //We dont destroy the prefab because we are going to reuse it if press Reset
        spaceshipPrefab.SetActive(false);

        //We are adding our custom rigidbody component that holds mass,center of mass, moment of interatia, momentum and angular momentum
        entityManager.AddComponentData(spaceshipEntity, new CustomRigidbody 
        { 
            MassValue = spaceshipMass, 
            CenterOfMass = spaceshipPrefab.transform.position, 
            MomentOfInertia = CalculateInertia(), 
            Momentum = float3.zero, 
            AngularMomentum = float3.zero 
        });

        //we add a velocity component for angular movement, angular velocity for rotation and gravity in case we want to simulate gravity.
        entityManager.AddComponentData(spaceshipEntity, new Velocity { Value = float3.zero });
        entityManager.AddComponentData(spaceshipEntity, new AngularVelocity { Value = float3.zero });
        entityManager.AddComponentData(spaceshipEntity, new Gravity { Value = gravity ? new float3(0f, -9.8f, 0f) : float3.zero });
       
        //Only if a thruster is places we add the thruster component to our entity.
        if(thrusterplaced)
        {
            entityManager.AddComponentData(spaceshipEntity, new ThrusterComponent
            {
                ThrustVector = thrustVector,
                ThrusterPosition = spaceshipPrefab.transform.GetChild(0).InverseTransformPoint(thrusterPosition)
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
