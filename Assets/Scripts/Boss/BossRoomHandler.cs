using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BossRoomHandler : MonoBehaviour
{
//TEST
    public static BossRoomHandler Instance;
    [SerializeField] private Tilemap wallTilemap;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void OpenDoor()
    {

        wallTilemap.SetTile(new Vector3Int(6, 13), null);
        wallTilemap.SetTile(new Vector3Int(6, 14), null);
    }

    private void OnDisable()
    {
        Instance = null;
    }
}
