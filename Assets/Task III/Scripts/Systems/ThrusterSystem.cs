using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class ThrusterSystem : JobComponentSystem
{
    [BurstCompile]
    struct ThrusterSystemJob : IJobForEach<ThrusterComponent,Velocity, CustomRigidbody, AngularVelocity>
    {
        public float deltaTime;
        public bool keyDown;

        public void Execute(ref ThrusterComponent _thrusterComponent, ref Velocity _velocity, ref CustomRigidbody _customRigidbody, ref AngularVelocity _angularVelocity)
        {
            //If space bar is pressed
            if(keyDown)
            {
                //adding thrust vector times deltatime to the momentum
                _customRigidbody.Momentum += _thrusterComponent.ThrustVector * deltaTime;
                //adding momentum divided by the mass to our linear velocity
                _velocity.Value += _customRigidbody.Momentum / _customRigidbody.MassValue;

                //calculating offset between center of mass and thurster position to calculate torque
                float3 offset = _customRigidbody.CenterOfMass - _thrusterComponent.ThrusterPosition;
                float3 torque = math.cross(_thrusterComponent.ThrustVector, offset);
                //adding torque times deltatime to our angular momentum
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