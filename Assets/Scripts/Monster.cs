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
        rm = GameObject.Find("ResourceManager");
        Enemy = GetComponent<NavMeshAgent>();
		HP = MaxHP;
		Enemy.speed = speed;
	}
	public void loseHP(int dmg)
	{
		HP -= dmg;
	}

    public void Update()
    {
        if (HP <= 0)
        {
            ResourceManager.instance.PlayerGold += MoneyReturn;
            Destroy(gameObject);
        }

    }
}