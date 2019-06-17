using UnityEngine;

[ExecuteInEditMode]
public class CollisionTester : MonoBehaviour {
    public Transform SphereA;
    public Transform SphereB;
    public Vector3 VelocityA;
    public Vector3 VelocityB;
    [Range(0f, 1f)] 
    public float Elastisity;

    public float GhostTime = 1;
    public bool ShowGhostPositions = true;
    public bool TestedCollision;
    public SpheresCollisionInfo CollisionInfo;

    private Vector3 initalPosA;
    private Vector3 initalPosB;

    public void TestCollision () {
        CollisionInfo = Collisions.SphereCollision (PositionA, PositionB, VelocityA, VelocityB, RadiusA, RadiusB, Elastisity);

        if (CollisionInfo.Collided) {
            ShowGhostPositions = true;
            TestedCollision = true;
            GhostTime = CollisionInfo.Time;
            initalPosA = SphereA.position;
            initalPosB = SphereB.position;
            SphereA.position = CollisionInfo.CollisionCentreA;
            SphereB.position = CollisionInfo.CollisionCentreB;
        } else {
            Debug.Log ("No Collision");
        }

    }

    public void Reset()
    {
        SphereA.position = initalPosA;
        SphereB.position = initalPosB;
        GhostTime = 0;
        TestedCollision = false;
    }

    void Update () {
        UpdateGhosts ();
        
    }

    void UpdateGhosts () {
        if (ShowGhostPositions) {
            SphereA.GetChild (0).gameObject.SetActive (true);
            SphereB.GetChild (0).gameObject.SetActive (true);

            SphereA.GetChild (0).position = TestedCollision ? SampleACol(GhostTime) : SampleA (GhostTime);
            SphereB.GetChild (0).position = TestedCollision ? SampleBCol (GhostTime) : SampleB (GhostTime);
        } else {
            SphereA.GetChild (0).gameObject.SetActive (false);
            SphereB.GetChild (0).gameObject.SetActive (false);
        }
    }

    Vector3 SampleA (float t) {
        return PositionA + VelocityA * t;
    }

    Vector3 SampleB (float t) {
        return PositionB + VelocityB * t;
    }

    Vector3 SampleACol(float t)
    {
        return PositionA + CollisionInfo.VelocityAfterCollisionA * t;
    }

    Vector3 SampleBCol(float t)
    {
        return PositionB + CollisionInfo.VelocityAfterCollisionB * t;
    }

    float RadiusA {
        get {
            return SphereA.transform.localScale.x / 2;
        }
    }

    float RadiusB {
        get {
            return SphereB.transform.localScale.x / 2;
        }
    }

    Vector3 PositionA {
        get {
            return SphereA.transform.position;
        }
    }

    Vector3 PositionB {
        get {
            return SphereB.transform.position;
        }
    }
}