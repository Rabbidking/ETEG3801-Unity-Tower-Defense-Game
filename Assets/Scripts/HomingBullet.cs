using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : Bullet
{
    //rotation speed of the bullet
    private float rotation = 1000;

    //max distance bullet follows a target before disengaging
    private float focus = 15;

    private Transform target;
    private bool isFollowing = true;
    private bool facing = true;

    private Vector3 tmp;

    void Start()
    {
		rm = GameObject.FindWithTag("ResourceManager");
		target = GameObject.FindGameObjectWithTag("Enemy").transform;
        transform.forward = target.position - transform.position;
    }

    void Update()
    {
        Vector3 targetDir = target.position - transform.position;
        float rotationDiff = Vector3.Dot(transform.forward, targetDir);

        if (rotationDiff > focus)
            isFollowing = false;
        else
            isFollowing = true;

        if (facing)
        {
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, rotation * Time.deltaTime, 0.0f);
            MoveForward(Time.deltaTime);

            if (isFollowing)
            {
                transform.rotation = Quaternion.LookRotation(newDir);
            }
        }

        else
        {
            if (isFollowing)
            {
                tmp = targetDir.normalized;
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(tmp * Time.deltaTime * speed, Space.World);
            }
        }
    }

    private void MoveForward(float rate)
    {
        transform.Translate(Vector3.forward * rate * speed, Space.Self);
    }
}