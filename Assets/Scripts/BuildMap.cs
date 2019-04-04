using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TILE_TYPE{BUILDABLE, TAKEN, NO_BUILD, PATH};

struct TileObject
{
    public GameObject mGameObject;
    public uint mTileX, mTileY;
}

public class BuildMap : MonoBehaviour
{
    public ResourceManager GoldMaster;
    private int DummyTowerCost = 100;

    //public char[,] Tiles;
    public TILE_TYPE[,] Tiles;
    private List<TileObject> TileObjects = new List<TileObject>();

    public TowerPrefabList TowerPrefabs;
    public CameraController CamController;
    public Camera MapCamera;
    public int MapLayer;

    public GameObject MapCursor, MapCursorSelect, TowerParent, MapTerrain;
    private MeshRenderer MapCursorRenderer;

    private int MapSize, MapGridSize;
    private float MapHeight;

    private bool TileIsSelected = false;
    private uint[] CursorTile   = new uint[2];  // [x, z]
    private uint[] SelectedTile = new uint[2];  // [x, z]
    private float[] BottomLeft  = new float[2]; // [x, z]
    private float TileSize;

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
                    SpawnTower();

                //upgrade tower
                else if (Input.GetKeyDown("1"))
                {
                    if (Tiles[SelectedTile[0], SelectedTile[1]] == TILE_TYPE.TAKEN)
                    {
                        foreach (TileObject TO in TileObjects)
                        {
                            if (TO.mTileX == SelectedTile[0] && TO.mTileY == SelectedTile[1])
                            {
                                TO.mGameObject.GetComponent<Tower>().damageUpgrade();
                                GoldMaster.PlayerGold -= 100;
                                GoldMaster.UpdateGoldText();
                                break;
                            }
                        }
                    }
                }

                else if (Input.GetKeyDown("2"))
                {
                    if (Tiles[SelectedTile[0], SelectedTile[1]] == TILE_TYPE.TAKEN)
                    {
                        foreach (TileObject TO in TileObjects)
                        {
                            if (TO.mTileX == SelectedTile[0] && TO.mTileY == SelectedTile[1])
                            {
                                TO.mGameObject.GetComponent<Tower>().chargeRateUpgrade();
                                GoldMaster.PlayerGold -= 100;
                                GoldMaster.UpdateGoldText();
                                break;
                            }
                        }
                    }
                }


                else if (Input.GetKeyDown("3"))
                {
                    if (Tiles[SelectedTile[0], SelectedTile[1]] == TILE_TYPE.TAKEN)
                    {
                        foreach (TileObject TO in TileObjects)
                        {
                            if (TO.mTileX == SelectedTile[0] && TO.mTileY == SelectedTile[1])
                            {
                                TO.mGameObject.GetComponent<Tower>().converterUpgrade();
                                GoldMaster.PlayerGold -= 100;
                                GoldMaster.UpdateGoldText();
                                break;
                            }
                        }
                    }
                }

                else if (Input.GetKeyDown("4"))
                {
                    if (Tiles[SelectedTile[0], SelectedTile[1]] == TILE_TYPE.TAKEN)
                    {
                        foreach (TileObject TO in TileObjects)
                        {
                            if (TO.mTileX == SelectedTile[0] && TO.mTileY == SelectedTile[1])
                            {
                                TO.mGameObject.GetComponent<Tower>().maxCapacityUpgrade();
                                GoldMaster.PlayerGold -= 100;
                                GoldMaster.UpdateGoldText();
                                break;
                            }
                        }
                    }
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
            CursorTile[1] = (uint)((rayHit.point.x + -BottomLeft[1]) / TileSize);
            CursorTile[0] = (uint)((rayHit.point.z + -BottomLeft[0]) / TileSize);
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
        if(Tiles[CursorTile[0], CursorTile[1]] != TILE_TYPE.PATH)
        {
            SelectedTile[0] = CursorTile[0];
            SelectedTile[1] = CursorTile[1];

            float centerX, centerZ;
            GetTileCenter(SelectedTile[1], SelectedTile[0], out centerX, out centerZ);

            MapCursorSelect.transform.localPosition = new Vector3(centerX, MapHeight + 0.005f, centerZ);
            MapCursorSelect.SetActive(true);

            TileIsSelected = true;
        }
    }

    private void HighlightCursorTile()
    {
        float newCursorX, newCursorZ;
        GetTileCenter(CursorTile[1], CursorTile[0], out newCursorX, out newCursorZ);

        MapCursor.SetActive(true);
        MapCursor.transform.position = new Vector3(newCursorX, MapCursor.transform.position.y, newCursorZ);

        if(Tiles[CursorTile[0], CursorTile[1]] != TILE_TYPE.PATH)
            MapCursorRenderer.material.color = Color.blue;
        else
            MapCursorRenderer.material.color = Color.red;
    }

    private void SpawnTower(int curLevel = 0)
    {
        if(Tiles[SelectedTile[0], SelectedTile[1]] == TILE_TYPE.BUILDABLE && GoldMaster.PlayerGold >= DummyTowerCost)
        {
            TileObject newTower;
            newTower.mTileX = SelectedTile[0];
            newTower.mTileY = SelectedTile[1];

            float newX, newZ;
            GetTileCenter(SelectedTile[0], SelectedTile[1], out newX, out newZ);

            //newTower.mGameObject = Instantiate(TowerPrefabs.DummyTower.GetComponent<Tower>().levels[curLevel].visualization);
            newTower.mGameObject = Instantiate(TowerPrefabs.DummyTower);
            newTower.mGameObject.transform.SetParent(TowerParent.transform, false);
            newTower.mGameObject.transform.localPosition = new Vector3(newZ, MapHeight + 0.5f, newX);
            //newTower.mGameObject.transform.localScale = new Vector3(TileSize * 0.9f, 1, TileSize * 0.9f);
            TileObjects.Add(newTower);

            Tiles[SelectedTile[0], SelectedTile[1]] = TILE_TYPE.TAKEN;

            DummyTowerCost = TowerPrefabs.DummyTower.GetComponent<Tower>().levels[0].cost;
            GoldMaster.PlayerGold -= DummyTowerCost;
            GoldMaster.UpdateGoldText();
        }
    }

    private void DestroyTower()
    {
        if(Tiles[SelectedTile[0], SelectedTile[1]] == TILE_TYPE.TAKEN)
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

            Tiles[SelectedTile[0], SelectedTile[1]] = TILE_TYPE.BUILDABLE;

            GoldMaster.PlayerGold += (DummyTowerCost / 2);
            GoldMaster.UpdateGoldText();
        }
    }
}
