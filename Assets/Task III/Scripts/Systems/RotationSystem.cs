using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class RotationSystem : JobComponentSystem
{

    [BurstCompile]
    struct RotationSystemJob : IJobForEach<Rotation, AngularVelocity, CustomRigidbody>
    {
        public float deltaTime;

        public void Execute(ref Rotation _rotation,[ReadOnly] ref AngularVelocity _angularVelocity, [ReadOnly] ref CustomRigidbody _customRigidbody)
        {
            //Inversing the moment of interatia value
            float3 inverseInertia = new float3(1f / _customRigidbody.MomentOfInertia.x, 1f / _customRigidbody.MomentOfInertia.y, 1f / _customRigidbody.MomentOfInertia.z);
            //Adjusting the angular velocity by multiplying the angular momentum to the inverseIneratio component wise
            _angularVelocity.Value = Vector3.Scale(_customRigidbody.AngularMomentum, inverseInertia);
            //multiplying the current rotation times the euler of our angular velocity times deltatime
            _rotation.Value = math.mul(_rotation.Value,quaternion.Euler(_angularVelocity.Value * deltaTime));

        }   
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new RotationSystemJob();
        
        job.deltaTime = UnityEngine.Time.deltaTime;

        return job.Schedule(this, inputDependencies);
    }
}