using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingBullet : Bullet
{
    public int numPierce = 2;

    void OnTriggerEnter(Collider co)
    {
        if(co.tag == "Enemy")
        {
            numPierce--;
            print(co);
            Health health = co.GetComponentInChildren<Health>();
            if (health)
            {
                health.decrease();
                if (numPierce <= 0)
                {
                    numPierce = 0;
                    Destroy(gameObject);
                }
            }
        }
    }
}
