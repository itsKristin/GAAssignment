using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;

public class ThrusterSystem : JobComponentSystem
{
    // This declares a new kind of job, which is a unit of work to do.
    // The job is declared as an IJobForEach<Translation, Rotation>,
    // meaning it will process all entities in the world that have both
    // Translation and Rotation components. Change it to process the component
    // types you want.
    //
    // The job is also tagged with the BurstCompile attribute, which means
    // that the Burst compiler will optimize it for the best performance.
    [BurstCompile]
    struct ThrusterSystemJob : IJobForEach<ThrusterComponent,Velocity, CustomRigidbody, AngularVelocity>
    {
        // Add fields here that your job needs to do its work.
        // For example,
        public float deltaTime;
        public bool keyDown;



        public void Execute(ref ThrusterComponent _thrusterComponent, ref Velocity _velocity, ref CustomRigidbody _customRigidbody, ref AngularVelocity _angularVelocity)
        {

            if(keyDown)
            {
                _customRigidbody.Momentum += _thrusterComponent.ThrustVector * deltaTime;
                _velocity.Value += _customRigidbody.Momentum / _customRigidbody.MassValue;

                float3 offset = _customRigidbody.CenterOfMass - _thrusterComponent.ThrusterPosition;
                float3 torque = math.cross(_thrusterComponent.ThrustVector, offset);
                _customRigidbody.AngularMomentum += torque * deltaTime;
            }   
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new ThrusterSystemJob();
        
        job.deltaTime = UnityEngine.Time.deltaTime;
        job.keyDown = Input.GetKey(KeyCode.Space);

        return job.Schedule(this, inputDependencies);
    }
}