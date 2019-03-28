using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
	// Start is called before the first frame update
	public int HP;
	public int speed;
	public int MoneyReturn;
	private NavMeshAgent Enemy;
    public GameObject rm;

	public void Start()
	{
        rm = GameObject.Find("ResourceManager");
        Enemy = GetComponent<NavMeshAgent>();
		Enemy.speed = speed;
	}
	public void loseHP()
	{
		HP -= 1;
	}

    public void Update()
    {
        if (HP <= 0)
        {
            rm.gameObject.GetComponent<ResourceManager>().PlayerGold += MoneyReturn;
            rm.gameObject.GetComponent<ResourceManager>().UpdateGoldText();
            Destroy(gameObject);
        }

    }
}