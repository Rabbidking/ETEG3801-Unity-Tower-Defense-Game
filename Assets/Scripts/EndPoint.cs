using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    public int x, y; //the x y on the local grid
    public int terrianX, terrianY; //the x y of the terrian this endpoint is on;

    public void OnTriggerEnter(Collider other) {
        if(other.tag == "Enemy"){
            GameObject.Destroy(other.gameObject);
            ResourceManager.instance.PlayerHealth -= 10;
            if (ResourceManager.instance.PlayerHealth <= 0) {
				GameObject.Find("TileMapGroup").GetComponent<MapGenerator>().CleanUp();
				SceneManager.LoadScene(0);
			}
        }
    }
}
