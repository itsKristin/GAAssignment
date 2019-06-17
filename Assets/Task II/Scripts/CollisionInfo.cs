using UnityEngine;

[System.Serializable]
public struct SpheresCollisionInfo {
    public bool Collided;
    public Vector3 CollisionCentreA;
    public Vector3 CollisionCentreB;
    public Vector3 NormalCollisionPlane;
    public Vector3 PointInCollisionPlane;
    public float Time;
    public Vector3 VelocityAfterCollisionA;
    public Vector3 VelocityAfterCollisionB;
}