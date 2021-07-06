using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class ShootingTiles : MonoBehaviour
{
    // Start is called before the first frame update

    
    public float shootingPeriod = 1.0f;
    public GameObject projectilePrefab;
    private Tilemap tilemap;
    public Sprite leftSprite;
    public Sprite upSprite;
    public Sprite rightSprite;
    public Sprite downSprite;
    private List<(TileBase, Vector3)> shootingTiles = new List<(TileBase, Vector3)>();
    public float speed = 1.0f;
    void Start()
    {
        tilemap = GetComponent<Tilemap>();

        foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin){
            TileBase tile = tilemap.GetTile(position);
                if (tile != null) {
                    shootingTiles.Add((tile, tilemap.CellToWorld(position)));
            }
        }
        StartCoroutine(DoShoot());
    }

    private IEnumerator DoShoot() 
    {
        while(true)
        {
            Shoot();
            yield return new WaitForSeconds(shootingPeriod);
        }
    }

    private void Shoot() {
        GameObject obj;
        for(int i = 0; i < shootingTiles.Count; i++) {
            Vector3 worldPosition = shootingTiles[i].Item2;
            if(tilemap.GetSprite(tilemap.WorldToCell(worldPosition)) == leftSprite)
                obj = Instantiate(projectilePrefab, worldPosition + new Vector3(0.25f, 0.25f,0f), Quaternion.Euler(0f,180f,0f));//tilemap.GetTransformMatrix(tilemap.WorldToCell(worldPosition)).rotation);
            else if(tilemap.GetSprite(tilemap.WorldToCell(worldPosition)) == rightSprite)
                obj = Instantiate(projectilePrefab, worldPosition + new Vector3(0.25f, 0.25f,0f), Quaternion.Euler(0f,0f,0f));
            else if(tilemap.GetSprite(tilemap.WorldToCell(worldPosition)) == upSprite)
                obj = Instantiate(projectilePrefab, worldPosition + new Vector3(-1f, 0f,0f), Quaternion.Euler(0f,90f,0f));
            else
                obj = Instantiate(projectilePrefab, worldPosition + new Vector3(-1f, 0f,0f), Quaternion.Euler(0f,-90f,0f));
            TimedElement timedElement = obj.GetComponent<TimedElement>();
        }
    }

}
