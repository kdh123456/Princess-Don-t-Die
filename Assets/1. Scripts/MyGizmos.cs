using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public Color gizmoColor = Color.green;
    [Range(0f, 1f)] public float radius = 0.1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
