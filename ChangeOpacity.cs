using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangeOpacity : MonoBehaviour
{
    // Start is called before the first frame update

    private TilemapRenderer map;

    public Material newMaterial;
    void Start()
    {
        map = GetComponent<TilemapRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        map.GetComponent<Material>();
    }
}
