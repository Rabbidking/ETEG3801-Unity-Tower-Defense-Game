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
			co.GetComponent<Monster>().loseHP();
			int health = co.GetComponent<Monster>().HP;
			if (health<= 0)
            {
				rm.gameObject.GetComponent<ResourceManager>().PlayerGold += co.gameObject.GetComponent<Monster>().MoneyReturn;
				rm.gameObject.GetComponent<ResourceManager>().UpdateGoldText();
				Destroy(co.gameObject);
            }
			if (numPierce <= 0)
			{
				numPierce = 0;
				Destroy(gameObject);
			}
		}
    }
}
