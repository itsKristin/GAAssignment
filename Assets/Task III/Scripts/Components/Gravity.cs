using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct Gravity : IComponentData
{
    public float3 Value;
}
