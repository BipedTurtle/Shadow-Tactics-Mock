using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private float _viewRadius;
    public float ViewRadius { get => this._viewRadius; }
    [SerializeField] private float _viewAngle;
    public float ViewAngle { get => this._viewAngle; }

    public Vector3 GetVectorFromAngle(float angleInDeg, bool isGlobalAngle)
    {
        if (!isGlobalAngle)
            angleInDeg += transform.rotation.eulerAngles.y;

        var angleInRad = angleInDeg * Mathf.Deg2Rad;
        var x = Mathf.Sin(angleInRad);
        var z = Mathf.Cos(angleInRad);

        return new Vector3(x, 0, z);
    }
}
