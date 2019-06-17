using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class MovementSystem : JobComponentSystem
{
    [BurstCompile]
    struct MovementSystemJob : IJobForEach<Translation, CustomRigidbody, Velocity>
    {
        public float deltaTime;
        
        public void Execute(ref Translation _translation, [ReadOnly] ref CustomRigidbody _customrigidbody, ref Velocity _velocity)
        {
            //In each "update" we add our velocity times delta time to our translation for our linear movement.
            _translation.Value += _velocity.Value * deltaTime;      
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new MovementSystemJob();
        
        job.deltaTime = UnityEngine.Time.deltaTime;

        return job.Schedule(this, inputDependencies);
    }
}