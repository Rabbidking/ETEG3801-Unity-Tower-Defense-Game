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

	public void Start()
	{
		Enemy = GetComponent<NavMeshAgent>();
		HP = MaxHP;
		Enemy.speed = speed;
	}
	public void loseHP()
	{
		HP -= 5;
	}
}