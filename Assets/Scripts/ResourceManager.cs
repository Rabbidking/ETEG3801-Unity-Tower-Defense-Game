using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
	public int PlayerHealth, PlayerGold, CurWave;
	public float nextWaveTime;
    public Text PlayerHealthText, PlayerGoldText, CurWaveText, NextWaveText;
	public GameObject spawner;
	public bool isWave = false;

	float stime = 0f, etime = 30.0f;

	private void Start()
    {
		spawner = GameObject.Find("EnemySpawner");
		CurWave = 0;
        UpdateHPText();
        UpdateGoldText();
        UpdateWaveText();
		UpdateNextWaveText();
		isWave = false;
    }
	public void UpdateHPText()
    {
        PlayerHealthText.text = "[HEALTH]: "   + PlayerHealth.ToString();
    }
	public void UpdateGoldText()
    {
        PlayerGoldText.text   = "[GOLD]:     " + PlayerGold.ToString();
    }
	public void UpdateWaveText()
    {
		CurWaveText.text = "[Wave]:       " + CurWave.ToString();
    }
	public void UpdateNextWaveText()
	{
		NextWaveText.text = "[WaveTime]:   " + ((int)nextWaveTime).ToString();
	}
	public void Update()
	{
		nextWaveTime = etime - stime;
		UpdateNextWaveText();
		if (!isWave)
		{
			stime += Time.deltaTime;
			
			if (stime >= etime)
			{
				isWave = true;
				stime = 0;
				etime = 30;
				StartCoroutine(spawner.gameObject.GetComponent<Spawner>().SpawnWave());
			}
		}
	}
	public void spawnWave()
	{
		if (!isWave)
		{
			isWave = true;
			stime = 0;
			etime = 30;
			StartCoroutine(spawner.gameObject.GetComponent<Spawner>().SpawnWave());
		}
	}
}
