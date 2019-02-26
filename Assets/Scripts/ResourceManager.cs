using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public int PlayerHealth, PlayerGold;
    public Text PlayerHealthText, PlayerGoldText;

    private void Start()
    {
        UpdateResourceText();
    }

    public void UpdateResourceText()
    {
        PlayerHealthText.text = "[HEALTH]: "   + PlayerHealth.ToString();
        PlayerGoldText.text   = "[GOLD]:     " + PlayerGold.ToString();
    }

}
