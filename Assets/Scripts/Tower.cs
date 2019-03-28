using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerLevel
{
    public int cost;
    public float fireRate;
    //public GameObject visualization;
    public GameObject bulletPrefab;
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

    // Rotation Speed
    public float rotationSpeed = 35;

    public MapGenerator mg;

    void Start()
    {
        //store all enemies that are in range
        enemiesInRange = new List<GameObject>();

        currentLevel = 0;

        lastShotTime = Time.time;
        towerData = gameObject.GetComponentInChildren<Tower>();
        mg = GameObject.Find("TileMapGroup").GetComponent<MapGenerator>();
        rm = GameObject.Find("ResourceManager");
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

            /*GameObject levelVisualization = levels[currentLevelIndex].visualization;
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
            }*/
        }
    }

    void OnEnable()
    {
        CurrentLevel = 0;
    }

    void Update()
    {
        //rotate tower
        //transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);

        GameObject target = null;

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

            if (Time.time - lastShotTime > towerData.levels[currentLevel].fireRate)
            {
                Shoot(target);
                target.GetComponent<Monster>().loseHP();
                lastShotTime = Time.time;
            }
        }
        else
        {
            if (lineRenderer.enabled)
                lineRenderer.enabled = false;
            return;
        }
    }

    void Shoot(GameObject target)
    {
        Vector3 startPosition = gameObject.transform.position;
        Vector3 targetPosition = target.transform.position;

        /*GameObject newBullet = (GameObject)Instantiate(levels[currentLevel].bulletPrefab);
        newBullet.transform.position = firePosition.transform.position;
        Bullet bulletComp = newBullet.GetComponent<Bullet>();
        //bulletComp.target = target.gameObject;
        bulletComp.startPosition = firePosition.transform.position;
        bulletComp.targetPosition = targetPosition;
        GameObject.Destroy(newBullet, 3);
        //Animator animator = towerData.CurrentLevel.visualization.GetComponent<Animator>();
        //animator.SetTrigger("fireShot");
        //AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        //audioSource.PlayOneShot(audioSource.clip);*/

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
