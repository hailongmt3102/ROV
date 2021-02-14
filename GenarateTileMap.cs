using UnityEngine;
using UnityEngine.Tilemaps;

public class GenarateTileMap : MonoBehaviour
{
    // Start is called before the first frame update

    public string MapName;

    private Tile[] tiles;

    private Tilemap RealTileMap;
    private Tilemap VirtualMap;

    private string[] SpriteToDescription = new string[8] {"left", "center", "right", "single", "otherup", "up", "otherdown", "down" };
    void Start()
    {
        RealTileMap =  transform.GetChild(0).GetComponent<Tilemap>();
        VirtualMap = transform.GetChild(1).GetComponent<Tilemap>();

        ReadTiles();
        GenerateMap(RealTileMap, Color.black);
    }

    private void GenerateMap(Tilemap map, Color color) {
        // readmap from resource
        Texture2D level = Resources.Load<Texture2D>("Maps/Map 1/Levels/" + MapName);
        if (level == null) {
            Debug.LogError("Can't load map image from path \"Maps/Map 1/Levels/\"" + MapName);
        }

        // create array 2D of directionController type.
        directionController[,] ground = new directionController[level.width + 2, level.height + 2];
        for (int x = 0; x < level.width + 2; x++)
        {
            for (int y = 0; y < level.height + 2; y++)
            {
                ground[x, y] = new directionController();
            }
        }
        // set 1 to value of directionController when capture dark pixel in level picture.
        for (int x = 0; x < level.width; x++) {
            for (int y = 0; y < level.height; y++) {
                if (level.GetPixel(x, y) == color)
                {
                    ground[x + 1, y + 1].value = 1;
                }
            }
        }
        CaculateLocationSprites(ground, level.width, level.height);
        // generate map by array 2 dimension map above.
        GenerateTiles(ground, level.width, level.height, tiles, map);
    }

    private void CaculateLocationSprites(directionController[,] ground, int width, int height) {
        for (int x = 1; x < width - 1; x++) {
            for (int y = 1; y < height - 1; y++) {
                if (ground[x, y].value == 0) continue;
                if (ground[x + 1, y - 1].value == 1) {
                    ground[x, y].rightbottom = true;
                }
                if (ground[x, y - 1].value == 1) {
                    ground[x, y].bottom = true;
                }
                if (ground[x - 1, y - 1].value == 1) {
                    ground[x, y].leftbottom = true;
                }
                if (ground[x - 1, y].value == 1) {
                    ground[x, y].left = true;
                }
                if (ground[x + 1, y].value == 1) {
                    ground[x, y].right = true;
                }
                if (ground[x - 1, y + 1].value == 1) {
                    ground[x, y].lefttop = true;  
                }
                if (ground[x, y + 1].value == 1) {
                    ground[x, y].top = true;
                }
                if (ground[x + 1, y + 1].value == 1) {
                    ground[x, y].righttop = true;
                }
            } 
        }
    }
    private void GenerateTiles(directionController[,] ground, int width, int height, Tile[] tiles, Tilemap map)
    {
        int FindSpriteIndex;
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                if (ground[x, y].value == 1)
                {
                    ground[x, y].caculateLocation();
                    FindSpriteIndex = Des2Index(ground[x, y].description, SpriteToDescription);
                    if (FindSpriteIndex == -1) {
                        Debug.LogError("something wrong when match description to sprite");
                        continue;
                    }
                    map.SetTile(new Vector3Int(x - 1, y - 1, 0), tiles[FindSpriteIndex]);
                }
            }
        }
    }
    private void ReadTiles() {
        tiles = Resources.LoadAll<Tile>("Maps/Map 1/Tile Palates/Tiles");
        if (tiles.Length == 0) {
            Debug.LogError("Can't load tiles from path \"Maps/Map1/Tile Palates/Tiles\"");
        }
    }
    class directionController
    {
        public bool left;
        public bool right;
        public bool top;
        public bool bottom;
        public bool lefttop;
        public bool leftbottom;
        public bool righttop;
        public bool rightbottom;
        public int value;
        public string description;

        public directionController()
        {
            left = right = bottom = top = leftbottom = lefttop = rightbottom = righttop = false;
            value = 0;
            description = "";
        }
        //value from 1 to 8 for 8 sprite map prefabs
        public void caculateLocation()
        {
            if (left == false && right == false && bottom == false && top == false && leftbottom == false && lefttop == false && rightbottom == false && righttop == false)
            {
                description = "single";
            }
            else if (left == true && right == true)
            {
                description = "center";
            }
            else if (right == true && left == false)
            {
                description = "left";
            }
            else if (left == true && right == false)
            {
                description = "right";
            }
            else if (leftbottom == true)
            {
                description = "up";
            }
            else if (lefttop == true)
            {
                description = "down";
            }
        }
        public void clear()
        {
            left = right = bottom = top = leftbottom = lefttop = rightbottom = righttop = false;
            value = 0;
        }
    }

    private int Des2Index(string des, string[] Sprite2Des) {
        for (int i = 0; i < Sprite2Des.Length; i++) {
            if (Sprite2Des[i] == des) return i;
        }
        return -1;
    }
}
