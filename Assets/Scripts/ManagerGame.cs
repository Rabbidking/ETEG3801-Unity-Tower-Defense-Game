using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerGame : MonoBehaviour
{
    private Canvas[] myCanvases;
    private GameObject mTower;
    public BuildMap buildMap;
    public UIManager UI;

    public Text DamageText, CapacitorText, FireRateText, ConverterText;
    public Text DamageCostText, CapacitorCostText, FireRateCostText, ConverterCostText;

    private int upgradeCost;
    // Start is called before the first frame update
    void Start()
    {
        upgradeCost = buildMap.upgradeCost;
        myCanvases = UI.GetComponentsInChildren<Canvas>();
        myCanvases[0].enabled = false;
        myCanvases[2].enabled = false;
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
        }
        else
            myCanvases[2].enabled = false;
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
    /*public void CreateTower(GameObject tower)
    {
        mTower = tower;
        Vector3 selectedPos = buildMap.MapCursorSelect.transform.position;
        Quaternion selectedRotation = buildMap.MapCursorSelect.transform.rotation;
        Instantiate(mTower, selectedPos, selectedRotation);
    }*/
   /* public void ToggleGameOver()
    {
        if (ResourceManager.instance.PlayerHealth <= 0)
        {
            myCanvases[2].enabled = true;
            myCanvases[1].enabled = false;
            myCanvases[0].enabled = false;
        }
    }*/
    public void updateConverterText()
    {
        ConverterText.text = "Upgrade Converter";
        ConverterCostText.text = upgradeCost.ToString();
    }
    public void upgradeConverter()
    {
        if (buildMap.Tiles[buildMap.SelectedTile[0], buildMap.SelectedTile[1]] == TILE_TYPE.TAKEN && ResourceManager.instance.PlayerGold >= buildMap.upgradeCost)
        {
            foreach (TileObject TO in buildMap.TileObjects)
            {
                if (TO.mTileX == buildMap.SelectedTile[0] && TO.mTileY == buildMap.SelectedTile[1])
                {
                    TO.mGameObject.GetComponent<Tower>().converterUpgrade();
                    ResourceManager.instance.PlayerGold -= buildMap.upgradeCost;
                    break;
                }
            }
        }
    }
    public void upgradeFireRate()
    {
        if ((buildMap.Tiles[buildMap.SelectedTile[0], buildMap.SelectedTile[1]] == TILE_TYPE.TAKEN && ResourceManager.instance.PlayerGold >= buildMap.upgradeCost))
        {
            foreach (TileObject TO in buildMap.TileObjects)
            {
                if (TO.mTileX == buildMap.SelectedTile[0] && TO.mTileY == buildMap.SelectedTile[1])
                {
                    TO.mGameObject.GetComponent<Tower>().chargeRateUpgrade();
                    ResourceManager.instance.PlayerGold -= buildMap.upgradeCost;
                    break;
                }
            }
        }
    }
    public void updateDamage()
    {
        DamageCostText.text = upgradeCost.ToString();
        DamageText.text = "Upgrade Damage";
    }
    public void updateFireRate()
    {
        FireRateText.text = "Upgrade Fire RAte";
        FireRateCostText.text = upgradeCost.ToString();
    }
    public void upgradeDamage()
    {
        if (buildMap.Tiles[buildMap.SelectedTile[0], buildMap.SelectedTile[1]] == TILE_TYPE.TAKEN && ResourceManager.instance.PlayerGold >= buildMap.upgradeCost)
        {
            foreach (TileObject TO in buildMap.TileObjects)
            {
                if (TO.mTileX == buildMap.SelectedTile[0] && TO.mTileY == buildMap.SelectedTile[1])
                {
                    TO.mGameObject.GetComponent<Tower>().damageUpgrade();
                    ResourceManager.instance.PlayerGold -= buildMap.upgradeCost;
                    break;
                }
            }
        }
    }
    public void updateConverter()
    {
        ConverterCostText.text = upgradeCost.ToString();
        ConverterText.text = "Upgrade Converter";
    }
    public void updateCapacitor()
    {
        CapacitorText.text = "Upgrade Capacitor";
        CapacitorCostText.text = upgradeCost.ToString();
    }
    public void upgradeCapacitor()
    {
        if (buildMap.Tiles[buildMap.SelectedTile[0], buildMap.SelectedTile[1]] == TILE_TYPE.TAKEN && ResourceManager.instance.PlayerGold >= buildMap.upgradeCost)
        {
            foreach (TileObject TO in buildMap.TileObjects)
            {
                if (TO.mTileX == buildMap.SelectedTile[0] && TO.mTileY == buildMap.SelectedTile[1])
                {
                    TO.mGameObject.GetComponent<Tower>().maxCapacityUpgrade();
                    ResourceManager.instance.PlayerGold -= buildMap.upgradeCost;
                    break;
                }
            }
        }
    }
}
