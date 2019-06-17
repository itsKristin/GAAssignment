using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct CustomRigidbody : IComponentData
{
    public float MassValue;
    public float3 CenterOfMass;
    public float3 MomentOfInertia;
    public float3 Momentum;
    public float3 AngularMomentum;



}
