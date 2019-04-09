using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public Vector2Int gridPoint; //the x y on the local grid
    public Vector2Int terrian; //the x y of the terrian this endpoint is on;

    public int MapDirection;

    public void OnTriggerEnter(Collider other) {
        if(other.tag == "Enemy"){
            Monster m = other.GetComponent<Monster>();
            ResourceManager.instance.PlayerHealth -= m.Damage;
            GameObject.Destroy(other.gameObject);
        }
    }
}
