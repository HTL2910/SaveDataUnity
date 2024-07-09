using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform target;

    private void Update()
    {
        transform.position = target.position;
    }
}
