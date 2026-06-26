using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePart : MonoBehaviour
{
    public SnakeHealth Owner { get; set; }
    //public Vector3 TargetPosition { get; set; }
    //public Vector3 TargetLookPosition { get; set; }

    //public float MoveSpeed { get; set; }

    //private void FixedUpdate()
    //{
    //    transform.position = Vector3.Lerp(transform.position, TargetPosition, MoveSpeed * Time.fixedDeltaTime);
    //    if (Vector3.Distance(transform.position, TargetPosition) < 0.001f)
    //    {
    //        transform.position = TargetPosition;
    //    }
    //    Vector3 dir = TargetLookPosition - transform.position;

    //    if (dir.sqrMagnitude > 0.0001f)
    //    {
    //        transform.rotation = Quaternion.LookRotation(dir);
    //    }
    //} pick this logic back to the Snakebody script.
}
