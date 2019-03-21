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

    /*void FixedUpdate()
    {
        // Still has a Target?
        if (target)
        {
            // Fly towards the target
            Vector3 dir = target.transform.position - transform.position;
            GetComponent<Rigidbody>().velocity = dir.normalized * speed;
        }
        else
        {
            // Otherwise destroy self
            Destroy(gameObject);
        }
    }*/

    void Update()
    {
        transform.position += (targetPosition - startPosition).normalized * speed * Time.deltaTime;
        /*float timeInterval = Time.time - startTime;
        gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, timeInterval * speed / distance);

        if ((gameObject.transform.position - targetPosition).sqrMagnitude < 0.01 )
        {
            if (target != null)
            {
                
                /*Transform healthBarTransform = target.transform.Find("Health");
                Health health = healthBarTransform.gameObject.GetComponent<Health>();
                health.decrease();

                if (health.current() <= 0)
                {
                    Destroy(target);
                    AudioSource audioSource = target.GetComponent<AudioSource>();
                    AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);

                    //gameManager.Gold += 50;
                }

                // Fly towards the target
                Vector3 dir = target.transform.position - transform.position;
                GetComponent<Rigidbody>().velocity = dir.normalized * speed;
            }

            else
            {
                // Otherwise destroy self
                Destroy(gameObject);
            }
        }*/
    }

    void OnTriggerEnter(Collider co)
    {
        print(co);
        
		co.GetComponent<Monster>().loseHP();
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
