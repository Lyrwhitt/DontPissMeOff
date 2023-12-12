using UnityEngine;

public class BezierCurve
{
    public Vector3 Linear(float t, Vector3 p0, Vector3 p1)
    {
        return ((1 - t) * p0) + (t * p1);
    }

    public Vector3 Quadratic(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return ((1 - t) * (1 - t) * p0) + (2 * t * (1 - t) * p1) + (t * t * p2);
    }
}
