using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Vines_Seed : Seed
{
    public Tile vineTile;
    public int maxHeight = 15;
    Tilemap vinesMap;
    Tilemap environmentMap;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();

        vinesMap = GameObject.FindGameObjectWithTag("Vines").GetComponent<Tilemap>();
        environmentMap = GameObject.FindGameObjectWithTag("Ground").GetComponent<Tilemap>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override IEnumerator Grow()
    {
        Vector3Int cell = vinesMap.WorldToCell(plantedPos);
        cell.y += 1;
        int i = 0;
        while (environmentMap.GetTile(cell) == null && i < maxHeight)
        {
            vinesMap.SetTile(cell, vineTile);
            cell.y += 1;
            i++;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
