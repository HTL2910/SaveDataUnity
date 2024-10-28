using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isRunning = false;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetVelocity = Vector3.zero;
    private float acceleration = 0.1f; 
    private Animator _animator;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            isRunning = true;
            Debug.Log("Có");
        }

        //if (isRunning)
        //{
        //    targetVelocity = Vector3.right * 5f;
        //}
        //else
        //{
        //    targetVelocity = Vector3.zero;
        //}
    }

    private void FixedUpdate()
    {
        
        //velocity = Vector3.Lerp(velocity, targetVelocity, acceleration);
        //gameObject.transform.position += velocity * Time.fixedDeltaTime;

        _animator.SetBool("Run", isRunning);
        if (isRunning)
        {
            _animator.SetFloat("Movement Multiplier", 3f);

        }
        else
        {
            _animator.SetFloat("Movement Multiplier", 0f);

        }
    }
}
