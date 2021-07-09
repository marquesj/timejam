using System.Numerics;
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
    private List<(TileBase, UnityEngine.Vector3)> shootingTiles = new List<(TileBase, UnityEngine.Vector3)>();
    public float speed = 1.0f;
    void Start()
    {
        tilemap = GetComponent<Tilemap>();

        foreach (UnityEngine.Vector3Int position in tilemap.cellBounds.allPositionsWithin){
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
        GameObject obj = new GameObject();
        for(int i = 0; i < shootingTiles.Count; i++) {
            UnityEngine.Vector3 worldPosition = shootingTiles[i].Item2;
            Sprite sprite = tilemap.GetSprite(tilemap.WorldToCell(worldPosition));
            if(sprite == leftSprite)
                obj = Instantiate(projectilePrefab, worldPosition + new UnityEngine.Vector3(-0.16f, 0.16f, 0f), UnityEngine.Quaternion.Euler(0f,180f,0f));//tilemap.GetTransformMatrix(tilemap.WorldToCell(worldPosition)).rotation);
            else if(sprite == rightSprite)
                obj = Instantiate(projectilePrefab, worldPosition + new UnityEngine.Vector3(0.40f, 0.16f, 0f), UnityEngine.Quaternion.Euler(0f,0f,0f));
            else if(sprite == upSprite)
                obj = Instantiate(projectilePrefab, worldPosition + new UnityEngine.Vector3(0.16f, 0.40f,0f), UnityEngine.Quaternion.Euler(0f,0f,90f));
            else if(sprite == downSprite)
                obj = Instantiate(projectilePrefab, worldPosition + new UnityEngine.Vector3(0.16f, -0.16f,0f), UnityEngine.Quaternion.Euler(0f,0f,-90f));
            TimedElement timedElement = obj.GetComponent<TimedElement>();
        }
    }

}
