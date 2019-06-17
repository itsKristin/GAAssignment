using UnityEngine;

public static class Collisions {

    public static SpheresCollisionInfo SphereCollision (Vector3 _positionA, Vector3 _positionB, Vector3 _velocityA, Vector3 _velocityB, float _radiusA, float _radiusB, float _elastisity) {
        var info = new SpheresCollisionInfo ();

        float sqrRadiiSum = (_radiusA + _radiusB) * (_radiusA + _radiusB);
        Vector3 offsetToB = _positionB - _positionA;

        // Already colliding
        if (offsetToB.sqrMagnitude <= sqrRadiiSum) {
            info.Collided = true;
            float resolutionDst = (_radiusA + _radiusB) - offsetToB.magnitude;
            info.CollisionCentreA = _positionA - offsetToB.normalized * resolutionDst / 2;
            info.CollisionCentreB = _positionB + offsetToB.normalized * resolutionDst / 2;
            return info;
        }

        // Get velocity of A in the reference frame of B, such that B can be considered a stationary object
        Vector3 _velocityAInReferenceFrameOfB = _velocityA - _velocityB;

        // Can't collide if A moving away from B
        if (Vector3.Dot (_velocityAInReferenceFrameOfB, offsetToB) < 0) {
            return info;
        }

        // Calculate the closest point of A passing B
        Vector3 dirAInReferenceFrameOfB = _velocityAInReferenceFrameOfB.normalized;
        Vector3 closestPassingPointToB = _positionA + dirAInReferenceFrameOfB * Vector3.Dot (dirAInReferenceFrameOfB, offsetToB);
        float closestSqrDstToBInPassing = (closestPassingPointToB - _positionB).sqrMagnitude;

        // Check if too far away to collide
        float resolveIntersectionSqrDst = sqrRadiiSum - closestSqrDstToBInPassing;
        if (resolveIntersectionSqrDst < 0) {
            return info;
        }

        // Calculate backtrack distance to avoid intersection
        float resolveIntersectionDst = Mathf.Sqrt (resolveIntersectionSqrDst);
        Vector3 closestConstrainedPoint = closestPassingPointToB - resolveIntersectionDst * dirAInReferenceFrameOfB;
        float collisionTime = (closestConstrainedPoint - _positionA).magnitude / _velocityAInReferenceFrameOfB.magnitude;

        Vector3 distanceCenters = info.CollisionCentreA - info.CollisionCentreB;
        
        //Calculating the x direction velocity vector perpendicular to Y
        float distanceA = Vector3.Dot(distanceCenters.normalized, _velocityA);
        Vector3 velocityAX = distanceCenters.normalized * distanceA;
        Vector3 velocityAY = _velocityA - velocityAX;

        distanceCenters *= -1f;
        float distanceB = Vector3.Dot(distanceCenters.normalized, _velocityB);
        Vector3 velocityBX = distanceCenters.normalized * distanceB;
        Vector3 velocityBY = _velocityB - velocityBX;

        //if the collision is elastic the velocity after impact is the same as before so all we need to do is add our scalar to this function.
        //https://en.wikipedia.org/wiki/Elastic_collision
        //if we wanted to incorporate mass we need to change the velocity calculation to this.
        //info.VelocityAfterCollisionA = (velocityAX * (mass1 - mass2) / (mass1 + mass2) + velocityBX * (2 * mass2) / (mass1 + mass2) + velocityAY) * _elastisity;
        //info.VelocityAfterCollisionB = (velocityAX * (2 * mass1) / (mass1 + mass2) + velocityBX * (mass2 - ,ass1) / (mass1 + mass2) + velocityBY) * _elastisity;

        // Set collision info
        info.Collided = true;
        info.CollisionCentreA = _positionA + _velocityA * collisionTime;
        info.CollisionCentreB = _positionB + _velocityB * collisionTime;
        info.NormalCollisionPlane = (info.CollisionCentreA - info.CollisionCentreB).normalized;
        info.PointInCollisionPlane = (info.CollisionCentreB * _radiusB - info.CollisionCentreA * _radiusA);
        info.Time = collisionTime;

        //Simplified version because we are ignoring mass.
        info.VelocityAfterCollisionA = (velocityBX + velocityAY) * _elastisity;
        info.VelocityAfterCollisionB = (velocityAX + velocityBY) * _elastisity;

        return info;
    }
}