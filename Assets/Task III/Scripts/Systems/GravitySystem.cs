using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

public class GravitySystem : JobComponentSystem
{
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