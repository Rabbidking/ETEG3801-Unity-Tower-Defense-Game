using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingBullet
{
    public int numPierce = 2;

    /*void OnTriggerEnter(Collider co)
    {
        if(co.tag == "Enemy")
        {
            numPierce--;
            print(co);
			co.GetComponent<Monster>().loseHP();
			int health = co.GetComponent<Monster>().HP;
			if (health<= 0)
            {
                ResourceManager.instance.PlayerGold += co.gameObject.GetComponent<Monster>().MoneyReturn;
				Destroy(co.gameObject);
            }
			if (numPierce <= 0)
			{
				numPierce = 0;
				Destroy(gameObject);
			}
		}
    }*/
}
