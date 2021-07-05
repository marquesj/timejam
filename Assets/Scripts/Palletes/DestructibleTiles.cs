using System;
using System.Threading;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleTiles : MonoBehaviour
{
    public TimeEvents timeEvents;
    public TileBase tileBase;
    public Tilemap destructibleTilemap;
    public LayerMask layermask;
    private List<(UnityEngine.Vector3Int, float)> cells = new List<(UnityEngine.Vector3Int, float)>();
    // Start is called before the first frame update
    void Start()
    {
        destructibleTilemap = GetComponent<Tilemap>();
        timeEvents.GoBackInTimeEvent += GoBack;
        timeEvents.PreviewBackInTimeEvent += PreviewPosition;
    }
    private void OnTriggerEnter2D(Collider2D other) {

        if( layermask == (layermask | (1 << other.gameObject.layer))) {
            
            UnityEngine.Vector3 hitPosition = UnityEngine.Vector3.zero;
            UnityEngine.Vector3 otherVelocity = other.gameObject.GetComponent<Rigidbody2D>().velocity;

            hitPosition.x = other.transform.position.x + 0.1f * otherVelocity.x; // add bias to enter the grid cell where the tile is
            hitPosition.y = other.transform.position.y + 0.1f * otherVelocity.y;

            UnityEngine.Vector3Int cell = destructibleTilemap.WorldToCell(hitPosition);

            destructibleTilemap.SetTile(cell, null);

            cells.Add((cell, Time.time));
        }
    }

    private void RestoreTile(UnityEngine.Vector3Int  cell) {
        destructibleTilemap.SetTile(cell, tileBase);
    }

    private void GoBack(float time) {
        float offset = (Time.time - time);
        SetPastState(time);
        AdjustTimeline(offset);
    }

    private void PreviewPosition(float time) {
        SetPastState(time);
    }

    private void SetPastState(float time)
    {
        for(int i = 0; i < cells.Count; i++) {
            if(time < cells[i].Item2)
            {
                destructibleTilemap.SetTile(cells[i].Item1, tileBase);
                
            }
            else
            {
                destructibleTilemap.SetTile(cells[i].Item1, null);
            }
            
        }
    }

    private void AdjustTimeline(float offset)
    {
        float createdTime = 0;
        for(int i = 0; i < cells.Count; i++) {
            createdTime = cells[i].Item2;
            createdTime += offset;
            cells[i] = (cells[i].Item1, createdTime);
        }
    }
}
