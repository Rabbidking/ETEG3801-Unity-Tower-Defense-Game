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
        UpdateHPText();
        UpdateGoldText();
        UpdateWaveText();
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

}
