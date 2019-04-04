using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerLevel
{
    public int cost;
    public float fireRate;
    //public GameObject bulletPrefab;
}

public class Tower : MonoBehaviour
{
    public List<TowerLevel> levels;
    private int currentLevel;
    public List<GameObject> enemiesInRange;
    private float lastShotTime;
    private Tower towerData;
    public Transform rotate;
    public Transform firePosition;
    //public bool useLaser = false;
    public LineRenderer lineRenderer;
    public GameObject rm;

    public enum lens { NORMAL, PIERCE, SCATTER };
    //capacitor is battery (fire rate). Once a charge value hits 100, fire.
    public int lensType;
    //capacitor = energy storage; converter = how much energy we can covert; targeting = multi-targeting; lens = type of laser
    public float capacitor, converter, chargeRate;
    public float maxCapacity = 100.00f;
    public int damage;

    //lineRenderer width
    public float width = 2.0f;

    // Rotation Speed
    public float rotationSpeed = 35;

    public int numPierce = 2;

    public MapGenerator mg;

    //SFX
    //public AudioClip fire1, fire2;
    //public new AudioSource audio;

    void Start()
    {
        //store all enemies that are in range
        enemiesInRange = new List<GameObject>();

        currentLevel = 0;
        damage = 1;

        lastShotTime = Time.time;
        towerData = gameObject.GetComponentInChildren<Tower>();
        mg = GameObject.Find("TileMapGroup").GetComponent<MapGenerator>();
        rm = GameObject.Find("ResourceManager");

        //audio = towerData.GetComponent<AudioSource>();
    }

    public TowerLevel GetNextLevel()
    {
        int currentLevelIndex = levels.IndexOf(levels[currentLevel]);
        int maxLevelIndex = levels.Count - 1;
        if (currentLevelIndex < maxLevelIndex)
        {
            return levels[currentLevelIndex + 1];
        }
        else
        {
            return null;
        }
    }

    //On upgrade, lower converter cost, increase damage / capacity. Two different functions each for converter and capacity (4 total)


    public void IncreaseLevel()
    {
        int currentLevelIndex = levels.IndexOf((levels[currentLevel]));
        if (currentLevelIndex < levels.Count - 1)
        {
            CurrentLevel = currentLevel + 1;
        }
    }


    public int CurrentLevel
    {
        get
        {
            return currentLevel;
        }
        set
        {
            currentLevel = value;
            int currentLevelIndex = levels.IndexOf((levels[currentLevel]));
        }
    }

    void OnEnable()
    {
        CurrentLevel = 0;
        lensType = 0;
        damage = 1;
    }

    public void damageUpgrade()
    {
        //increase damage and converter cost (optional increase cost?)
        damage += 5;
        if(damage >= 25)
        {
            lineRenderer.widthMultiplier = width;
            //audio.clip = fire2;
        }
        /*else
        {
            audio.clip = fire1;
        }*/
    }

    public void converterUpgrade()
    {
        //lower converter cost. Optional?
        converter -= 5;
    }

    public void maxCapacityUpgrade()
    {
        //increase maxCapacity
        maxCapacity += 25;
    }

    public void chargeRateUpgrade()
    {
        //increase chargeRate speed
        chargeRate += 5;
    }

    void Update()
    {
        GameObject target = null;

        if (capacitor < maxCapacity)
            capacitor += chargeRate * Time.deltaTime;

        if(capacitor > maxCapacity)
            capacitor = maxCapacity;

        if (converter < 0)
            converter = 0;

        float minimalEnemyDistance = float.MaxValue;
        foreach (GameObject enemy in enemiesInRange)
        {
            if (!enemy) continue;

            float distanceToGoal = Vector3.Distance(enemy.transform.position, mg.EndPos);
            
            if (distanceToGoal < minimalEnemyDistance)
            {
                target = enemy;
                minimalEnemyDistance = distanceToGoal;
            }
        }

        if (target != null)
        {
            //lock on to an enemy target, rotating to face it
            Vector3 dir = target.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(rotate.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
            rotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            //update the laser firing to track enemies every frame
            lineRenderer.SetPosition(0, firePosition.transform.position);
            lineRenderer.SetPosition(1, target.transform.position);

            if (Time.time - lastShotTime > towerData.levels[currentLevel].fireRate && capacitor >= converter)
            {
                capacitor -= converter;
                Shoot(target);
                StartCoroutine(LineHandler());
                target.GetComponent<Monster>().loseHP(damage);
                lastShotTime = Time.time;
            }
            else
                lineRenderer.enabled = false;
        }
        else
        {
            if (lineRenderer.enabled)
                lineRenderer.enabled = false;
            return;
        }
    }

    IEnumerator LineHandler()
    {
        int timeToDraw = 1;
        if(Time.time - timeToDraw <= 0)
        {
            yield return new WaitForSeconds(1);
            lineRenderer.enabled = false;
        }
    }

    void Shoot(GameObject target)
    {
        Vector3 startPosition = gameObject.transform.position;
        Vector3 targetPosition = target.transform.position;

        //animator.SetTrigger("fireShot");
        //AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        //audioSource.PlayOneShot(audioSource.clip);*/

        StartCoroutine(LineHandler());

        if (!lineRenderer.enabled)
            lineRenderer.enabled = true;  
    }

    void OnEnemyDestroy(GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag.Equals("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }
}
