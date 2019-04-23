using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerGame : MonoBehaviour
{
    public Canvas[] myCanvases;
    public BuildMap buildMap;
    public UIManager UI;

    public Text DamageText, CapacitorText, FireRateText, ConverterText;
    public Text WavesSurviveTxt;

    private int upgradeCost;

    // Start is called before the first frame update
    void Start()
    {
        upgradeCost = buildMap.upgradeCost;
        myCanvases = UI.GetComponentsInChildren<Canvas>();
        myCanvases[0].enabled = false;
        myCanvases[2].enabled = false;
        myCanvases[3].enabled = false;
        updateCapacitor();
        updateDamage();
        updateConverter();
        updateFireRate();
    }
    void Update()
    {
        if (buildMap.current && buildMap.current.tiles[buildMap.SelectedTile[0], buildMap.SelectedTile[1]] == TILE_TYPE.TAKEN)
        {
            myCanvases[2].enabled = true;
            //Time.timeScale = 0f;
        }
        else
            myCanvases[2].enabled = false;
        if(ResourceManager.instance.PlayerHealth == 0)
        {
            EnableEndMenu();
        }
    }
    public void TogglePauseMenu()
    {
        if (myCanvases[0].enabled == true)
        {
            myCanvases[0].enabled = false;
            Time.timeScale = 1.0f;
        }
        else
        {
            myCanvases[0].enabled = true;
            Time.timeScale = 0f;
        }
    }
    private void EnableEndMenu()
    {
        if(myCanvases[3].enabled == false)
        {
            myCanvases[3].enabled = true;
            Time.timeScale = 0f;
        }
    }
    public void upgradeConverter()
    {
        if (buildMap.current.tiles[buildMap.SelectedTile[0], buildMap.SelectedTile[1]] == TILE_TYPE.TAKEN && ResourceManager.instance.PlayerGold >= buildMap.upgradeCost)
        {
            foreach (TileObject TO in buildMap.TileObjects)
            {
                if (TO.mTileX == buildMap.SelectedTile[0] && TO.mTileY == buildMap.SelectedTile[1])
                {
                    TO.mGameObject.GetComponent<Tower>().converterUpgrade();
                    ResourceManager.instance.PlayerGold -= buildMap.upgradeCost;
                    Time.timeScale = 1.0f;
                    break;
                }
            }
        }
    }
    public void upgradeFireRate()
    {
        if ((buildMap.current.tiles[buildMap.SelectedTile[0], buildMap.SelectedTile[1]] == TILE_TYPE.TAKEN && ResourceManager.instance.PlayerGold >= buildMap.upgradeCost))
        {
            foreach (TileObject TO in buildMap.TileObjects)
            {
                if (TO.mTileX == buildMap.SelectedTile[0] && TO.mTileY == buildMap.SelectedTile[1])
                {
                    TO.mGameObject.GetComponent<Tower>().chargeRateUpgrade();
                    ResourceManager.instance.PlayerGold -= buildMap.upgradeCost;
                    Time.timeScale = 1.0f;
                    break;
                }
            }
        }
    }
    public void updateDamage()
    {
        DamageText.text = "Upgrade Damage: "+ upgradeCost.ToString() + " Gold";
    }
    public void updateFireRate()
    {
        FireRateText.text = "Upgrade Fire Rate:"+ upgradeCost.ToString()+ " Gold";
    }
    public void upgradeDamage()
    {
        if (buildMap.current.tiles[buildMap.SelectedTile[0], buildMap.SelectedTile[1]] == TILE_TYPE.TAKEN && ResourceManager.instance.PlayerGold >= buildMap.upgradeCost)
        {
            foreach (TileObject TO in buildMap.TileObjects)
            {
                if (TO.mTileX == buildMap.SelectedTile[0] && TO.mTileY == buildMap.SelectedTile[1])
                {
                    TO.mGameObject.GetComponent<Tower>().damageUpgrade();
                    ResourceManager.instance.PlayerGold -= buildMap.upgradeCost;
                    Time.timeScale = 1.0f;
                    break;
                }
            }
        }
    }
    public void updateConverter()
    {
        ConverterText.text = "Upgrade Converter: "+ upgradeCost.ToString() + " Gold";
    }
    public void updateCapacitor()
    {
        CapacitorText.text = "Upgrade Capacitor: "+ upgradeCost.ToString() + " Gold";
    }
    public void upgradeCapacitor()
    {
        if (buildMap.current.tiles[buildMap.SelectedTile[0], buildMap.SelectedTile[1]] == TILE_TYPE.TAKEN && ResourceManager.instance.PlayerGold >= buildMap.upgradeCost)
        {
            foreach (TileObject TO in buildMap.TileObjects)
            {
                if (TO.mTileX == buildMap.SelectedTile[0] && TO.mTileY == buildMap.SelectedTile[1])
                {
                    TO.mGameObject.GetComponent<Tower>().maxCapacityUpgrade();
                    ResourceManager.instance.PlayerGold -= buildMap.upgradeCost;
                    Time.timeScale = 1.0f;
                    break;
                }
            }
        }
    }
    public void updateFinalWave()
    {
        WavesSurviveTxt.text = "Waves Survived: " + ResourceManager.instance.CurWaveText;
    }
}
