using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildplace : MonoBehaviour
{
    // The Tower that should be built
    public GameObject towerPrefab;

    void OnMouseUpAsButton()
    {
        // Build Tower above Buildplace
        GameObject g = (GameObject)Instantiate(towerPrefab);
        g.transform.position = transform.position + Vector3.up;
    }

    [ContextMenu("Make cubes")]
    void makeCubes()
    {

        GameObject parent = new GameObject();
        parent.name = "LevelBlocks";

        for (int x = -14; x < 15; x++)
        {
            for (int y = -14; y < 15; y++)
            {
                GameObject g = (GameObject)Instantiate(this.gameObject);
                g.transform.position = new Vector3((float)x, 0.5f, (float)y);
                g.name = "Cube (" + x + "," + y + ")";
                g.transform.parent = parent.transform;
            }
        }
    }
}
