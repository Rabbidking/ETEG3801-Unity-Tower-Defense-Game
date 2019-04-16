using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    //Actual values
    private int _health, _gold, _wave;
    private ManagerGame gameManager;

    AudioSource audioSource;

    bool loadInitiated = false;

    //Handle Text Resource read out here;
    public int PlayerHealth {
        get { return _health;}
        set {
            _health = value;
            if (_health <= 0)
            {
                MapGenerator.instance.CleanUp();
                //Here is to active Endgame Menu
                //gameManager.myCanvases[3].enabled(true);
                SceneManager.LoadScene(0);
                audioSource = gameObject.GetComponent<AudioSource>();
                GameObject.Find("TileMapGroup").GetComponent<MapGenerator>().CleanUp();
                if(!loadInitiated)
                {
                    StartCoroutine(DelayLoad());
                    loadInitiated = true;
                }
            }
            PlayerHealthText.text = "[HEALTH]: " + _health; }
    }

    IEnumerator DelayLoad()
    {
        audioSource.PlayOneShot(audioSource.clip);
        yield return new WaitForSeconds(audioSource.clip.length);

        //Load scene here
        SceneManager.LoadScene(0);
    }

    public int PlayerGold {
        get { return _gold; }
        set { _gold = value; PlayerGoldText.text = "[GOLD]: " + _gold; }
    }
    public int CurWave {
        get { return _wave; }
        private set { _wave = value; CurWaveText.text = "[WAVE]: " + (_wave + 1); }
    }
    //What needs to be set in editor
    public Spawner spawner;
    public Text PlayerHealthText, PlayerGoldText, CurWaveText, NextWaveText;
    public int StartHealth, StartGold, WaveTime;
    [HideInInspector]
	private bool isWave = false;
    private float stime;
    public static ResourceManager instance;

	private void Start()
    {
		spawner = GameObject.Find("EnemySpawner").GetComponent<Spawner>();
		CurWave = 0;
        PlayerHealth = StartHealth;
        PlayerGold = StartGold;
        stime = 0;
        instance = this;
		isWave = false;
    }
    

	public void Update()
	{

        if (!isWave)
        {
            NextWaveText.text = string.Format("[WAVE TIMER]: {0:N}", WaveTime - stime);
            stime += Time.deltaTime;
            if (stime >= WaveTime)
            {
                spawnWave();
            }
        }
        else {
            NextWaveText.text = "[WAVE TIMER]: IN PROGRESS";
        }
	}

	public void spawnWave()
	{
		if (!isWave)
		{
			isWave = true;
			stime = 0;
			StartCoroutine(spawner.SpawnWave());
		}
	}

    public void EndWave() {
        isWave = false;
        CurWave++;
        if (CurWave % 5 == 0) {
            MapGenerator.instance.Generate();
        }
    }
}
