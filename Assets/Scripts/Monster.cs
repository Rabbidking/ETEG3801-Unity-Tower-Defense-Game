using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
	// Start is called before the first frame update
	public int HP;
	public int speed;
	private NavMeshAgent Enemy;

	public void Start()
	{
		Enemy = GetComponent<NavMeshAgent>();
		Enemy.speed = speed;
	}
}