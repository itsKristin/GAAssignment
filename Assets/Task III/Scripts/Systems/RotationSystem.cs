using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class RotationSystem : JobComponentSystem
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
    struct RotationSystemJob : IJobForEach<Rotation, AngularVelocity, CustomRigidbody>
    {
        // Add fields here that your job needs to do its work.
        // For example,
        public float deltaTime;
        
        
        
        public void Execute(ref Rotation _rotation,[ReadOnly] ref AngularVelocity _angularVelocity, [ReadOnly] ref CustomRigidbody _customRigidbody)
        {
            // Implement the work to perform for each entity here.
            // You should only access data that is local or that is a
            // field on this job. Note that the 'rotation' parameter is
            // marked as [ReadOnly], which means it cannot be modified,
            // but allows this job to run in parallel with other jobs
            // that want to read Rotation component data.
            // For example,
            //Quaternion quaternionFromAnglularVelo = new Quaternion(_angularVelocity.Value.x, _angularVelocity.Value.y, _angularVelocity.Value.z,0f);
            //Quaternion newQuaternion = Quaternion.Multiply(Quaternion.Multiply((Quaternion.Multiply(quaternionFromAnglularVelo, new Quaternion(_rotation.Value.value.x,_rotation.Value.value.y,_rotation.Value.value.z,_rotation.Value.value.w))) , deltaTime / 2f),2f);
            // _rotation.Value = new quaternion(newQuaternion.X, newQuaternion.Y, newQuaternion.Z, newQuaternion.W);
            //_rotation.Value = math.mul(math.normalize(_rotation.Value), quaternion.AxisAngle(math.up(),  _angularVelocity.Value.x * deltaTime));
            

        }   
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new RotationSystemJob();
        
        // Assign values to the fields on your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        job.deltaTime = UnityEngine.Time.deltaTime;
        
        
        
        // Now that the job is set up, schedule it to be run. 
        return job.Schedule(this, inputDependencies);
    }
}