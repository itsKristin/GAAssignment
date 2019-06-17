using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct AngularVelocity : IComponentData
{
    public float3 Value;
}
