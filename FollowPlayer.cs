using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform Player;
    public float SmoothSpeed = 0.125f;
    public Vector3 Offset;

    private Vector3 DesiredPos;
    private Vector3 SmoothedPos;
    void FixedUpdate()
    {
        DesiredPos = Player.position + Offset;
        SmoothedPos = Vector3.Lerp(transform.position, DesiredPos, SmoothSpeed);
        transform.position = SmoothedPos;

        transform.LookAt(Player);
    }
}
