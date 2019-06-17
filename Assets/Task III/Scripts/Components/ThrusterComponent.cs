using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct ThrusterComponent : IComponentData
{
    public float3 ThrustVector;
    public float3 ThrusterPosition;
}
