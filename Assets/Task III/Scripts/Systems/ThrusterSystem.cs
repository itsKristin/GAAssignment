using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
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
        
        
        
        public void Execute(ref ThrusterComponent _thrusterComponent, ref Velocity _velocity, [ReadOnly] ref CustomRigidbody _customRigidbody, ref AngularVelocity _angularVelocity)
        {
            //Angular Force
            float3 acceleration = _thrusterComponent.ThrustVector / _customRigidbody.MassValue;
            _velocity.Value += acceleration * deltaTime;

            float3 torque = math.dot(_thrusterComponent.ThrusterPosition - new float3(0f, 0f, 0f), _thrusterComponent.ThrustVector);
            _angularVelocity.Value += torque;

        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new ThrusterSystemJob();
        
        job.deltaTime = UnityEngine.Time.deltaTime;
         
        return job.Schedule(this, inputDependencies);
    }
}