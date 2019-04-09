using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    private int _health, _gold, _wave;

    public int PlayerHealth
    {
        get { return _health; }
        set
        {
            _health = value;
            PlayerHealthText.text = "[HEALTH]: " + _health;
        }
    }

    public int PlayerGold
    {
        get { return _gold; }
        set
        {
            _gold = value;
            PlayerGoldText.text = "[GOLD]: " + _gold;
        }
    }

    public int CurWave
    {
        get { return _wave; }
        set
        {
            _wave = value;
            CurWaveText.text = "[GOLD]: " + _wave;
        }
    }

    public int startingHealth, startingGold;

    public Text PlayerHealthText, PlayerGoldText, CurWaveText;

    public static ResourceManager instance;

    private void Start()
    {
        PlayerHealth = startingHealth;
        PlayerGold = startingGold;
		CurWave = 1;
        instance = this;
    }
}
