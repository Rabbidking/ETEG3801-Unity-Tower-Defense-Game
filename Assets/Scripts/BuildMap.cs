using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TILE_TYPE{TAKEN, NO_BUILD, BUILDABLE, PATH };

public struct TileObject
{
    public GameObject mGameObject;
    public uint mTileX, mTileY;
}

public class BuildMap : MonoBehaviour
{
    private int DummyTowerCost = 100;
    public int upgradeCost = 100;

    public TILE_TYPE[,] Tiles;
    public List<TileObject> TileObjects = new List<TileObject>();

    public TowerPrefabList TowerPrefabs;
    public CameraController CamController;
    public Camera MapCamera;
    public int MapLayer;

    public GameObject MapCursor, MapCursorSelect, TowerParent;
    private MeshRenderer MapCursorRenderer;

    private int MapSize, MapGridSize;
    private float MapHeight;

    private bool TileIsSelected = false;
    private uint[] CursorTile   = new uint[2];  // [x, z]
    public uint[] SelectedTile = new uint[2];  // [x, z]
    private float[] BottomLeft  = new float[2]; // [x, z]
    public float TileSize;

    public MapSection current, currentSelect;

    public void SetMapValues(int mapSize, int mapGridSize, float mapHeight)
    {
        MapSize     = mapSize;
        MapGridSize = mapGridSize;
        MapHeight   = mapHeight;
    }

    public void FinishSetup()
    {
        TileSize = ((float)MapSize / (float)MapGridSize);

        // In case we have rectangular maps later //
        BottomLeft[0] = (float)MapSize / -2.0f;
        BottomLeft[1] = (float)MapSize / -2.0f;

        Vector3 lp = MapCursor.transform.localPosition;
        MapCursor.transform.localPosition = new Vector3(lp.x, MapHeight + 0.0125f, lp.z);
        MapCursor.transform.localScale    = new Vector3(TileSize * 0.95f, 0.025f, TileSize * 0.95f);
        MapCursorRenderer = MapCursor.GetComponent<MeshRenderer>();
        MapCursor.SetActive(false);

        MapCursorSelect.transform.localPosition = new Vector3(lp.x, MapHeight + 0.005f, lp.z);
        MapCursorSelect.transform.localScale = new Vector3(TileSize * 0.93f, 0.01f, TileSize * 0.93f);
        MapCursorSelect.SetActive(false);

        CamController.transform.position = new Vector3(0, MapSize, 0);
        CamController.transform.LookAt(new Vector3(0, -1.0f, 0));
        CamController.SetResetValuesWithCurrentValues();
    }

    void Update()
    {
        if(RaycastToTile())
        {
            HighlightCursorTile();

            if(Input.GetMouseButtonDown(0))
                SelectCursorTile();
        }
        else
            MapCursor.SetActive(false);

        if(TileIsSelected)
        {
            if(Input.GetMouseButtonDown(1))
                DeselectTile();
            else
            {
                //build tower
                if (Input.GetKeyDown("b"))
                {
                    SpawnTower();
                }
                //upgrade tower
                else if (Input.GetKeyDown("1"))
                {
                    if (current.tiles[SelectedTile[0], SelectedTile[1]] == TILE_TYPE.TAKEN && ResourceManager.instance.PlayerGold >= upgradeCost)
                    {
                        foreach (TileObject TO in TileObjects)
                        {
                            if (TO.mTileX == SelectedTile[0] && TO.mTileY == SelectedTile[1])
                            {
                                TO.mGameObject.GetComponent<Tower>().damageUpgrade();
                                ResourceManager.instance.PlayerGold -= upgradeCost;
                                break;
                            }
                        }
                    }
                }

                else if (Input.GetKeyDown("2"))
                {
                    if (current.tiles[SelectedTile[0], SelectedTile[1]] == TILE_TYPE.TAKEN && ResourceManager.instance.PlayerGold >= upgradeCost)
                    {
                        foreach (TileObject TO in TileObjects)
                        {
                            if (TO.mTileX == SelectedTile[0] && TO.mTileY == SelectedTile[1])
                            {
                                TO.mGameObject.GetComponent<Tower>().chargeRateUpgrade();
                                ResourceManager.instance.PlayerGold -= upgradeCost;
                                break;
                            }
                        }
                    }
                }


                else if (Input.GetKeyDown("3"))
                {
                    if (current.tiles[SelectedTile[0], SelectedTile[1]] == TILE_TYPE.TAKEN && ResourceManager.instance.PlayerGold >= upgradeCost)
                    {
                        foreach (TileObject TO in TileObjects)
                        {
                            if (TO.mTileX == SelectedTile[0] && TO.mTileY == SelectedTile[1])
                            {
                                TO.mGameObject.GetComponent<Tower>().converterUpgrade();
                                ResourceManager.instance.PlayerGold -= upgradeCost;
                                break;
                            }
                        }
                    }
                }

                else if (Input.GetKeyDown("4"))
                {
                    if (current.tiles[SelectedTile[0], SelectedTile[1]] == TILE_TYPE.TAKEN && ResourceManager.instance.PlayerGold >= upgradeCost)
                    {
                        foreach (TileObject TO in TileObjects)
                        {
                            if (TO.mTileX == SelectedTile[0] && TO.mTileY == SelectedTile[1])
                            {
                                TO.mGameObject.GetComponent<Tower>().maxCapacityUpgrade();
                                ResourceManager.instance.PlayerGold -= upgradeCost;
                                break;
                            }
                        }
                    }
                }
				else if (Input.GetKeyDown("m"))
				{
                    ResourceManager.instance.spawnWave();
				}
                else if (Input.GetKeyDown("g"))
                {
                    MapGenerator.instance.Generate();
                }
                //destroy tower
                else if (Input.GetKeyDown("v"))
                    DestroyTower();
            }
        }
    }

    private bool RaycastToTile()
    {
        RaycastHit rayHit;
        Ray cameraRay = MapCamera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(cameraRay, out rayHit, (float)MapCamera.transform.position.y * 1.2f, 1 << MapLayer))
        {
            current = rayHit.collider.gameObject.GetComponent<MapSection>();
            CursorTile[1] = ((uint)((rayHit.point.x + -BottomLeft[1]) / TileSize))%(uint)MapGenerator.instance.gridSize;
            CursorTile[0] = ((uint)((rayHit.point.z + -BottomLeft[0]) / TileSize))% (uint)MapGenerator.instance.gridSize;
            return true;
        }

        return false;
    }

    public void GetTileCenter(uint tileX, uint tileZ, out float centerX, out float centerZ)
    {
        centerX = BottomLeft[1] + (TileSize * (tileX + 0.5f));
        centerZ = BottomLeft[0] + (TileSize * (tileZ + 0.5f));
    }

    private void DeselectTile()
    {
        MapCursorSelect.SetActive(false);
        TileIsSelected = false;
    }

    private void SelectCursorTile()
    {
        if(current.tiles[CursorTile[0], CursorTile[1]] != TILE_TYPE.PATH)
        {
            currentSelect = current;

            SelectedTile[0] = CursorTile[0];
            SelectedTile[1] = CursorTile[1];

            float centerX, centerZ;
            GetTileCenter(SelectedTile[1], SelectedTile[0], out centerX, out centerZ);

            MapCursorSelect.transform.localPosition = new Vector3(centerX + currentSelect.Section.y * MapSize, MapHeight + 0.005f, centerZ + currentSelect.Section.x  * MapSize);
            MapCursorSelect.SetActive(true);

            TileIsSelected = true;
        }
    }

    private void HighlightCursorTile()
    {
        float newCursorX, newCursorZ;
        GetTileCenter(CursorTile[1], CursorTile[0], out newCursorX, out newCursorZ);

        MapCursor.SetActive(true);
        MapCursor.transform.position = new Vector3(newCursorX + current.Section.y * MapSize, MapCursor.transform.position.y, newCursorZ + current.Section.x * MapSize);

        if(current.tiles[CursorTile[0], CursorTile[1]] != TILE_TYPE.PATH)
            MapCursorRenderer.material.color = Color.blue;
        else
            MapCursorRenderer.material.color = Color.red;
    }

    private void SpawnTower(int curLevel = 0)
    {
        if(current.tiles[SelectedTile[0], SelectedTile[1]] == TILE_TYPE.BUILDABLE && ResourceManager.instance.PlayerGold >= DummyTowerCost)
        {
            TileObject newTower;
            newTower.mTileX = SelectedTile[0];
            newTower.mTileY = SelectedTile[1];

            float newX, newZ;
            GetTileCenter(SelectedTile[0], SelectedTile[1], out newX, out newZ);
            newX += currentSelect.Section.x * MapSize;
            newZ += currentSelect.Section.y * MapSize;
            //newTower.mGameObject = Instantiate(TowerPrefabs.DummyTower.GetComponent<Tower>().levels[curLevel].visualization);
            newTower.mGameObject = Instantiate(TowerPrefabs.DummyTower);
            newTower.mGameObject.transform.SetParent(TowerParent.transform, false);
            newTower.mGameObject.transform.localPosition = new Vector3(newZ, MapHeight + 0.5f, newX);
            //newTower.mGameObject.transform.localScale = new Vector3(TileSize * 0.9f, 1, TileSize * 0.9f);
            TileObjects.Add(newTower);

            current.tiles[SelectedTile[0], SelectedTile[1]] = TILE_TYPE.TAKEN;

            DummyTowerCost = TowerPrefabs.DummyTower.GetComponent<Tower>().levels[0].cost;
            ResourceManager.instance.PlayerGold -= DummyTowerCost;
        }
    }

    private void DestroyTower()
    {
        if(current.tiles[SelectedTile[0], SelectedTile[1]] == TILE_TYPE.TAKEN)
        {
            foreach(TileObject TO in TileObjects)
            {
                if(TO.mTileX == SelectedTile[0] && TO.mTileY == SelectedTile[1])
                {
                    Destroy(TO.mGameObject);
                    TileObjects.Remove(TO);
                    break;
                }
            }

            current.tiles[SelectedTile[0], SelectedTile[1]] = TILE_TYPE.BUILDABLE;

            ResourceManager.instance.PlayerGold += (DummyTowerCost / 2);
        }
    }
}
