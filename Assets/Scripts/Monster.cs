using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
	// Start is called before the first frame update
	public int MaxHP;
	public int HP;
	public int speed;
	public int Damage;
	public int MoneyReturn;
	private NavMeshAgent Enemy;
    public GameObject rm;

	public void Start()
	{
<<<<<<< HEAD
        rm = GameObject.Find("ResourceManager");
        Enemy = GetComponent<NavMeshAgent>();
=======
		Enemy = GetComponent<NavMeshAgent>();
		HP = MaxHP;
>>>>>>> 5ba2447507af4bfa691f43c07c2f1eadb24e0924
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