using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarshipObject : AgentObject
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    private Rigidbody2D rigidBody;
    void Start()
    {
        Debug.Log("Starting Starship");
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TargetPostition != null)
        {
            //SeekKinematic();
            SeekForward(rigidBody,TargetPostition,movementSpeed,rotationSpeed);
        }
    }

    public static void SeekKinematic(Rigidbody2D rb, Vector2 target, float speed)
    {
        Vector2 desiredVelocity = ((Vector3)target - rb.transform.position).normalized * (speed * Time.fixedDeltaTime);

        Vector2 steeringForce = desiredVelocity - rb.velocity;
        
        rb.AddForce(steeringForce);
    }
    
    public static void SeekForward(Rigidbody2D rb, Vector2 target, float speed, float rotSpeed)
    {
        Vector2 directionToTarget = ((Vector3)target - rb.transform.position).normalized;

        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg + 90f;

        float angleDifference = Mathf.DeltaAngle(targetAngle, rb.transform.eulerAngles.z);

        float rotationStep = rotSpeed* Time.fixedDeltaTime;

        float rotationAmount = Mathf.Clamp(angleDifference, -rotationStep, rotationStep);
        
        rb.transform.Rotate(Vector3.forward, rotationAmount);

        rb.velocity = rb.transform.up * (speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Target")
            GetComponent<AudioSource>().Play();
    }
}
