using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerLevel
{
    public int cost;
    public float fireRate;
    public GameObject visualization;
}

public class Tower : MonoBehaviour
{
    public List<TowerLevel> levels;
    private TowerLevel currentLevel;
    public List<GameObject> enemiesInRange;
    private float lastShotTime;
    private Tower towerData;

    // The Bullet
    public GameObject bulletPrefab;

    // Rotation Speed
    public float rotationSpeed = 35;

    //ADDED FOR 2/29/DEMO by RyanTollefson
    public MapGenerator mg;

    void Start()
    {
        //store all enemies that are in range
        enemiesInRange = new List<GameObject>();

        lastShotTime = Time.time;
        towerData = gameObject.GetComponentInChildren<Tower>();
        //ADDED FOR 2/29/DEMO by RyanTollefson
        mg = GameObject.Find("TileMapGroup").GetComponent<MapGenerator>();
    }

    public TowerLevel GetNextLevel()
    {
        int currentLevelIndex = levels.IndexOf(currentLevel);
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

    public void IncreaseLevel()
    {
        int currentLevelIndex = levels.IndexOf(currentLevel);
        if (currentLevelIndex < levels.Count - 1)
        {
            CurrentLevel = levels[currentLevelIndex + 1];
        }
    }


    public TowerLevel CurrentLevel
    {
        get
        {
            return currentLevel;
        }
        set
        {
            currentLevel = value;
            int currentLevelIndex = levels.IndexOf(currentLevel);

            GameObject levelVisualization = levels[currentLevelIndex].visualization;
            for (int i = 0; i < levels.Count; i++)
            {
                if (levelVisualization != null)
                {
                    if (i == currentLevelIndex)
                    {
                        levels[i].visualization.SetActive(true);
                    }
                    else
                    {
                        levels[i].visualization.SetActive(false);
                    }
                }
            }
        }
    }

    void OnEnable()
    {
        CurrentLevel = levels[0];
    }

    void Update()
    {
        //rotate tower
        transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);

        GameObject target = null;
        // 1
        float minimalEnemyDistance = float.MaxValue;
        foreach (GameObject enemy in enemiesInRange)
        {
            if (!enemy) continue;
            //REMOVED FOR 2/29/DEMO by RyanTollefson
            //float distanceToGoal = Vector3.Distance(enemy.transform.position, GameObject.Find("Castle").transform.position);

            //ADDED FOR 2/29/DEMO by RyanTollefson
            float distanceToGoal = Vector3.Distance(enemy.transform.position, mg.End);
            
            if (distanceToGoal < minimalEnemyDistance)
            {
                target = enemy;
                minimalEnemyDistance = distanceToGoal;
            }
        }
        // 2
        if (target != null)
        {
            if (Time.time - lastShotTime > towerData.CurrentLevel.fireRate)
            {
                Shoot(target);
                lastShotTime = Time.time;
            }
        }
    }

    void Shoot(GameObject target)
    {
        Vector3 startPosition = gameObject.transform.position;
        Vector3 targetPosition = target.transform.position;
        //startPosition = bulletPrefab.transform.position;
        //targetPosition = bulletPrefab.transform.position;

        GameObject newBullet = (GameObject)Instantiate(bulletPrefab);
        newBullet.transform.position = startPosition;
        Bullet bulletComp = newBullet.GetComponent<Bullet>();
        bulletComp.target = target.gameObject;
        bulletComp.startPosition = startPosition;
        bulletComp.targetPosition = targetPosition;

        //Animator animator = towerData.CurrentLevel.visualization.GetComponent<Animator>();
        //animator.SetTrigger("fireShot");
        //AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        //audioSource.PlayOneShot(audioSource.clip);
    }

    void OnEnemyDestroy(GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
    }

    void OnTriggerEnter(Collider other)
    {
        // 2
        if (other.gameObject.tag.Equals("Enemy"))
        {
            //GameObject g = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            //g.GetComponent<Bullet>().target = other.transform;

            enemiesInRange.Add(other.gameObject);
            //EnemyDestructionDelegate del = other.gameObject.GetComponent<EnemyDestructionDelegate>();
            //del.enemyDelegate += OnEnemyDestroy;
        }
    }
    // 3
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
           // EnemyDestructionDelegate del = other.gameObject.GetComponent<EnemyDestructionDelegate>();
          //  del.enemyDelegate -= OnEnemyDestroy;
        }
    }
}
