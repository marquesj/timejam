using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleTiles : MonoBehaviour
{
    public Tilemap destructibleTilemap;
    public LayerMask layermask;
    // Start is called before the first frame update
    void Start()
    {
        destructibleTilemap = GetComponent<Tilemap>();
    }
    private void OnTriggerEnter2D(Collider2D other) {

        if( layermask == (layermask | (1 << other.gameObject.layer))) {
            
            UnityEngine.Vector3 hitPosition = UnityEngine.Vector3.zero;
            UnityEngine.Vector3 otherVelocity = other.gameObject.GetComponent<Rigidbody2D>().velocity;

            hitPosition.x = other.transform.position.x + 0.1f * otherVelocity.x; // add bias to enter the grid cell where the tile is
            hitPosition.y = other.transform.position.y + 0.1f * otherVelocity.y;
            destructibleTilemap.SetTile(destructibleTilemap.WorldToCell(hitPosition), null);
        }
    }
}
