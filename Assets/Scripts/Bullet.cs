using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Speed
    public float speed = 10;

    // Target (set by Tower)
    public List<GameObject> target;

    public Vector3 startPosition;
    public Vector3 targetPosition;
    private float distance;
    private float startTime;
	public GameObject rm;
    
    void Start()
    {
		rm = GameObject.FindWithTag("ResourceManager");
        startTime = Time.time;
        distance = Vector3.Distance(startPosition, targetPosition);
    }

    void Update()
    {
        transform.position += (targetPosition - startPosition).normalized * speed * Time.deltaTime;
        /*
        if (health.current() <= 0)
        {
            Destroy(target);
            AudioSource audioSource = target.GetComponent<AudioSource>();
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);

            //gameManager.Gold += 50;
        }
        }*/
    }

    void OnTriggerEnter(Collider co)
    {
        print(co);
        
		int health = co.GetComponent<Monster>().HP;
		if (health<=0)
        {
			rm.gameObject.GetComponent<ResourceManager>().PlayerGold += co.gameObject.GetComponent<Monster>().MoneyReturn;
			rm.gameObject.GetComponent<ResourceManager>().UpdateGoldText();
			Destroy(co.gameObject);
		}
		Destroy(gameObject);
        
    }
}
