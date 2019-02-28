using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    public ResourceManager rm;

    public void OnTriggerEnter(Collider other) {
        if(other.tag == "Enemy"){
            GameObject.Destroy(other.gameObject);
            rm.PlayerHealth -= 10;
            rm.UpdateResourceText();
            if (rm.PlayerHealth <= 0) {
                SceneManager.LoadScene(0);
            }
        }
    }
}
