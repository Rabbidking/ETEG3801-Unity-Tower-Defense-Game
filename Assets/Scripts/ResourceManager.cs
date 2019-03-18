using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public int PlayerHealth, PlayerGold, CurWave;
    public Text PlayerHealthText, PlayerGoldText, CurWaveText;

    private void Start()
    {
		CurWave = 1;
        UpdateResourceText();
    }

    public void UpdateResourceText()
    {
        PlayerHealthText.text = "[HEALTH]: "   + PlayerHealth.ToString();
        PlayerGoldText.text   = "[GOLD]:     " + PlayerGold.ToString();
		CurWaveText.text = "[Wave]:       " + CurWave.ToString();
    }

}
