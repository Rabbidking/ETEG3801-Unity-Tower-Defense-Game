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
    //public char[,] Tiles;
    public TILE_TYPE[,] Tiles;
    private List<TileObject> TileObjects = new List<TileObject>();

    public TowerPrefabList TowerPrefabs;
    public Camera MapCamera;
    public int MapLayer;

    public GameObject MapCursor, TowerParent, MapTerrain;
    private MeshRenderer MapCursorRenderer;

    private int MapSize, MapGridSize;
    private float MapHeight;

    private uint[] CurrentTile = new uint[2];  // [x, z]
    private float[] BottomLeft = new float[2]; // [x, z]
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
        MapCursor.transform.localPosition = new Vector3(lp.z, MapHeight + 0.0125f, lp.z);
        MapCursor.transform.localScale = new Vector3(TileSize * 0.95f, 0.025f, TileSize * 0.95f);
        MapCursorRenderer = MapCursor.GetComponent<MeshRenderer>();

        MapCamera.transform.position = new Vector3(0, MapSize, 0);
        MapCamera.transform.rotation.SetLookRotation(new Vector3(90.0f, 0, 0));
    }

    // (Input.GetMouseButtonDown(0) //

    void Update()
    {
        if(RaycastToTile())
        {
            HighlightCurrentTile();
            if(Input.GetKeyDown("b"))
                SpawnTower();
            else if(Input.GetKeyDown("d"))
                DestroyTower();
        }
        else
            MapCursor.SetActive(false);
    }

    private bool RaycastToTile()
    {
        RaycastHit rayHit;
        Ray cameraRay = MapCamera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(cameraRay, out rayHit, (float)MapCamera.transform.position.y * 1.2f, 1 << MapLayer))
        {
            CurrentTile[1] = (uint)((rayHit.point.x + -BottomLeft[1]) / TileSize);
            CurrentTile[0] = (uint)((rayHit.point.z + -BottomLeft[0]) / TileSize);
            return true;
        }

        return false;
    }

    private void GetCurrentTileCenter(out float centerX, out float centerZ)
    {
        centerX = BottomLeft[1] + (TileSize * (CurrentTile[1] + 0.5f));
        centerZ = BottomLeft[0] + (TileSize * (CurrentTile[0] + 0.5f));
    }

    private void HighlightCurrentTile()
    {
        float newCursorX, newCursorZ;
        GetCurrentTileCenter(out newCursorX, out newCursorZ);

        MapCursor.SetActive(true);
        MapCursor.transform.position = new Vector3(newCursorX, MapCursor.transform.position.y, newCursorZ);

        if(Tiles[CurrentTile[0], CurrentTile[1]] == 0)
            MapCursorRenderer.material.color = Color.blue;
        else
            MapCursorRenderer.material.color = Color.red;
    }

    private void SpawnTower()
    {
        if(Tiles[CurrentTile[0], CurrentTile[1]] == TILE_TYPE.BUILDABLE)
        {
            TileObject newTower;
            newTower.mTileX = CurrentTile[0];
            newTower.mTileY = CurrentTile[1];

            float newX, newZ;
            GetCurrentTileCenter(out newX, out newZ);

            newTower.mGameObject = Instantiate(TowerPrefabs.DummyTower);
            newTower.mGameObject.transform.SetParent(TowerParent.transform, false);
            newTower.mGameObject.transform.localPosition = new Vector3(newX, 0.5f, newZ);
            newTower.mGameObject.transform.localScale = new Vector3(TileSize * 0.9f, MapHeight + 0.5f, TileSize * 0.9f);
            TileObjects.Add(newTower);

            Tiles[CurrentTile[0], CurrentTile[1]] = TILE_TYPE.TAKEN;
        }
    }

    private void DestroyTower()
    {
        if(Tiles[CurrentTile[0], CurrentTile[1]] == TILE_TYPE.TAKEN)
        {
            foreach(TileObject TO in TileObjects)
            {
                if(TO.mTileX == CurrentTile[0] && TO.mTileY == CurrentTile[1])
                {
                    Destroy(TO.mGameObject);
                    TileObjects.Remove(TO);
                    break;
                }
            }

            Tiles[CurrentTile[0], CurrentTile[1]] = TILE_TYPE.BUILDABLE;
        }
    }
}
