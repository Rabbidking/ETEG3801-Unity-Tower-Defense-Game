using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TILE_TYPE {BUILD, NO_BUILD, PATH};

public class BuildMap : MonoBehaviour
{
    public TILE_TYPE[,] Tiles;

    public TowerPrefabList TowerPrefabs;
    public Camera MapCamera;
    public int MapLayer;

    public GameObject MapCursor, MapTerrain;
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

    void Update()
    {
        if(RaycastToTile())
        {
            HighlightCurrentTile();
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

        if (Tiles[CurrentTile[0], CurrentTile[1]] == TILE_TYPE.BUILD)
            MapCursorRenderer.material.color = Color.blue;
        else
            MapCursorRenderer.material.color = Color.red;
    }

    private void BuildTower()
    {

    }

    private void DestroyTower()
    {

    }
}
