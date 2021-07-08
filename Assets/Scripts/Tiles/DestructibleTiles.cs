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

    private List<UnityEngine.Vector3Int> destructibleTiles = new List<UnityEngine.Vector3Int>();

    // Start is called before the first frame update
    void Start()
    {
        destructibleTilemap = GetComponent<Tilemap>();
        timeEvents.GoBackInTimeEvent += GoBack;
        timeEvents.PreviewBackInTimeEvent += PreviewPosition;

        foreach (UnityEngine.Vector3Int position in destructibleTilemap.cellBounds.allPositionsWithin){
            TileBase tile = destructibleTilemap.GetTile(position);
                if (tile != null) {
                    destructibleTiles.Add(position);
            }
        }
    }
    private void OnDestroy() {
        timeEvents.GoBackInTimeEvent -= GoBack;
        timeEvents.PreviewBackInTimeEvent -= PreviewPosition;
    }
    private void OnTriggerEnter2D(Collider2D other) {

        if( layermask == (layermask | (1 << other.gameObject.layer))) {
            
            UnityEngine.Vector3 hitPosition = UnityEngine.Vector3.zero;
            UnityEngine.Vector3 otherVelocity = other.gameObject.GetComponent<Rigidbody2D>().velocity;

            hitPosition.x = other.transform.position.x + 0.025f * otherVelocity.x;// add bias to enter the grid cell where the tile is
            hitPosition.y = other.transform.position.y + 0.025f * otherVelocity.y;

            UnityEngine.Vector3Int cell = destructibleTilemap.WorldToCell(hitPosition + new UnityEngine.Vector3(0.07f, 0.07f, 0f));

            UnityEngine.Vector3Int cell2 = destructibleTilemap.WorldToCell(hitPosition + new UnityEngine.Vector3(-0.07f, -0.07f, 0f));

            UnityEngine.Vector3Int cell3 = destructibleTilemap.WorldToCell(hitPosition + new UnityEngine.Vector3(-0.07f, 0.07f, 0f));

            UnityEngine.Vector3Int cell4 = destructibleTilemap.WorldToCell(hitPosition + new UnityEngine.Vector3(0.07f, -0.07f, 0f));

            foreach(UnityEngine.Vector3Int position in destructibleTiles) {
                    if(cell == position && destructibleTilemap.GetTile(cell) != null) {
                        destructibleTilemap.SetTile(cell, null);
                        other.GetComponent<Health>().Damage(1);
                    }
                    if(cell2 == position && destructibleTilemap.GetTile(cell2) != null) {
                        destructibleTilemap.SetTile(cell2, null);
                        other.GetComponent<Health>().Damage(1);
                    }
                    if(cell3 == position && destructibleTilemap.GetTile(cell3) != null) {
                        destructibleTilemap.SetTile(cell3, null);
                        other.GetComponent<Health>().Damage(1);
                    }
                    if(cell4 == position && destructibleTilemap.GetTile(cell4) != null) {
                        destructibleTilemap.SetTile(cell4, null);
                        other.GetComponent<Health>().Damage(1);
                    }
            }

            cells.Add((cell, Time.time));
        }
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

                foreach(UnityEngine.Vector3Int position in destructibleTiles) {
                    if(cells[i].Item1 == position && destructibleTilemap.GetTile(cells[i].Item1) == null)
                        destructibleTilemap.SetTile(cells[i].Item1, tileBase);
                }              
            }
            else
            {
                if(destructibleTilemap.GetTile(cells[i].Item1) != null)
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
