using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class GravitySystem : JobComponentSystem
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
    struct GravitySystemJob : IJobForEach<CustomRigidbody,Velocity,Gravity>
    {
        public float deltaTime;
        
        
        
        public void Execute([ReadOnly] ref CustomRigidbody _customRigidbody, ref Velocity _velocity, [ReadOnly] ref Gravity _gravity)
        {

            _velocity.Value += _gravity.Value * deltaTime;


        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new GravitySystemJob();
        
        job.deltaTime = UnityEngine.Time.deltaTime;
        
        
        return job.Schedule(this, inputDependencies);
    }
}