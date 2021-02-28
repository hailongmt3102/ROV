using UnityEngine;
using UnityEngine.Tilemaps;

public class GenarateTileMap : MonoBehaviour
{
    // map group path and map name path store in the resource.
    public string MapGroup  = "Map 1";
    public string MapName   = "lv1";

    // Some color to mapping in the map picture.
    public Color colorToRealGround          = Color.black;
    public Color colorToVirtualGround       = Color.white;

    // define some kind to mapping color with start point, save point, endpoint
    public ColorToPrefabs ColorToStart      = new ColorToPrefabs(Color.red);
    public ColorToPrefabs ColorToSave       = new ColorToPrefabs(Color.blue);
    public ColorToPrefabs ColorToEnd        = new ColorToPrefabs(Color.green);

    // COin prefabs
    public GameObject Coin;

    // number of coin in this level.
    public int CoinNumber;

    // store all tile from source
    private Tile[] tilemaps;

    // virtual tilemap and real tilemap
    // In the virtual tilemap, the player can't stand on
    // Otherwise, the player can move In the real tilemap
    private Tilemap RealTileMap;
    private Tilemap VirtualMap;

    //get position of the player in this level.
    private Vector3 playerPosition = Vector3.zero;

    private string[] SpriteToDescription = new string[8] { "left", "center", "right", "single", "belowup", "up", "belowdown", "down" };

    void Awake()
    {
        RealTileMap = transform.GetChild(0).GetComponent<Tilemap>();
        VirtualMap = transform.GetChild(1).GetComponent<Tilemap>();
        ReadTiles();
        GenerateMap(RealTileMap, colorToRealGround, VirtualMap, colorToVirtualGround);
        // to change the color of each cell when the player moves, we must set a flag of the cell on a virtual ground
        SetTileFlagToAllVirtualMap();
        // after generate the map, set player position.
        playerPosition.x += 0.5f;
        playerPosition.y += 2;
        GameObject.Find("Player").gameObject.transform.position = playerPosition;
    }
    // This function will read all tiles map from the source.
    private void ReadTiles()
    {
        tilemaps = Resources.LoadAll<Tile>("Maps/"+ MapGroup +"/Tile Palates/Tiles");
        if (tilemaps.Length == 0)
        {
            Debug.LogError("Can't load tiles from path \"Maps/Map1/Tile Palates/Tiles\"");
        }
    }
    private void GenerateMap(Tilemap RealMap, Color RealGroundColor, Tilemap VirtualMap, Color VirtualGroundColor) {
        // read the map from the source
        Texture2D level = Resources.Load<Texture2D>("Maps/" + MapGroup + "/Levels/" + MapName);
        if (level == null) {
            Debug.LogError("Can't load map image from path \"Maps/" + MapGroup + "/Levels/\"" + MapName);
            return;
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
        // set 1 to the value of directionController when capturing black and white pixel in the level picture.
        for (int x = 0; x < level.width; x++) {
            for (int y = 0; y < level.height; y++) {
                if (level.GetPixel(x, y) == RealGroundColor)
                {
                    ground[x + 1, y + 1].value = 1;
                }
                else if (level.GetPixel(x, y) == VirtualGroundColor) {
                    ground[x + 1, y + 1].value = -1;
                }
            }
        }
        CaculateLocationSprites(ground, level.width + 2, level.height + 2);
        // generate map by array 2 dimension map above.
        GenerateTiles(ground, level.width + 2, level.height + 2, tilemaps, RealMap, VirtualMap);
        // create start point, save point, an endpoint in the tilemap.
        GeneratePoint(level, ColorToStart, ColorToSave, ColorToEnd);
        // generate coin in game
        CoinNumber = GenerateCoin(level, Coin);
    }
    private void CaculateLocationSprites(directionController[,] ground, int width, int height) {
        for (int x = 1; x < width - 1; x++) {
            for (int y = 1; y < height - 1; y++) {
                if (ground[x, y].value == 0) continue;
                if (Mathf.Abs(ground[x + 1, y - 1].value) == 1) {
                    ground[x, y].rightbottom = true;
                }
                if (Mathf.Abs(ground[x, y - 1].value) == 1) {
                    ground[x, y].bottom = true;
                }
                if (Mathf.Abs(ground[x - 1, y - 1].value) == 1) {
                    ground[x, y].leftbottom = true;
                }
                if (Mathf.Abs(ground[x - 1, y].value) == 1) {
                    ground[x, y].left = true;
                }
                if (Mathf.Abs(ground[x + 1, y].value) == 1) {
                    ground[x, y].right = true;
                }
                if (Mathf.Abs(ground[x - 1, y + 1].value) == 1) {
                    ground[x, y].lefttop = true;
                }
                if (Mathf.Abs(ground[x, y + 1].value) == 1) {
                    ground[x, y].top = true;
                }
                if (Mathf.Abs(ground[x + 1, y + 1].value) == 1) {
                    ground[x, y].righttop = true;
                }
            }
        }
    }
    private void GenerateTiles(directionController[,] ground, int width, int height, Tile[] tiles, Tilemap RealMap, Tilemap VirtualMap)
    {
        int FindSpriteIndex;
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                if (ground[x, y].value != 0)
                {
                    ground[x, y].caculateLocation();
                    FindSpriteIndex = Des2Index(ground[x, y].description, SpriteToDescription);
                    if (FindSpriteIndex == -1) {
                        Debug.LogError("something wrong when match description to sprite");
                        continue;
                    }
                    if (ground[x, y].value == 1)
                    {
                        RealMap.SetTile(new Vector3Int(x - 1, y - 1, 0), tiles[FindSpriteIndex]);
                    }
                    else {
                        VirtualMap.SetTile(new Vector3Int(x - 1, y - 1, 0), tiles[FindSpriteIndex]);
                    }
                    if (ground[x, y].description == "up")
                    {
                        FindSpriteIndex = Des2Index("belowup", SpriteToDescription);
                        if (FindSpriteIndex == -1)
                        {
                            Debug.LogError("something wrong when match description to sprite");
                            continue;
                        }
                        if (ground[x, y].value == 1)
                        {
                            RealMap.SetTile(new Vector3Int(x - 1, y - 2, 0), tiles[FindSpriteIndex]);
                        }
                        else
                        {
                            VirtualMap.SetTile(new Vector3Int(x - 1, y - 2, 0), tiles[FindSpriteIndex]);
                        }
                    }
                    else if (ground[x, y].description == "down") {
                        FindSpriteIndex = Des2Index("belowdown", SpriteToDescription);
                        if (FindSpriteIndex == -1)
                        {
                            Debug.LogError("something wrong when match description to sprite");
                            continue;
                        }
                        if (ground[x, y].value == 1)
                        {
                            RealMap.SetTile(new Vector3Int(x - 1, y - 2, 0), tiles[FindSpriteIndex]);
                        }
                        else
                        {
                            VirtualMap.SetTile(new Vector3Int(x - 1, y - 2, 0), tiles[FindSpriteIndex]);
                        }
                    }
                }
            }
        }
    }
    private void GeneratePoint(Texture2D level, ColorToPrefabs start, ColorToPrefabs save, ColorToPrefabs end) {
        Color current;
        Vector3 position = Vector3.zero;
        for (int x = 0; x < level.width; x++) {
            for (int y = 0; y < level.height; y++) {
                current = level.GetPixel(x, y);
                position.Set(x, y, 0);
                if (current == start.color)
                {
                    Instantiate(start.prefab, position, Quaternion.identity);
                    playerPosition = position;
                }
                else if (current == save.color) {
                    Instantiate(save.prefab, position, Quaternion.identity);
                }
                else if (current == end.color)
                {
                    Instantiate(end.prefab, position, Quaternion.identity);
                }
            }
        }
    }
    private int GenerateCoin(Texture2D level, GameObject Coin)
    {
        int CoinNumber = 0;
        int shape = 0;
        // order mapping shape:
        // squares, triangle, axis, single
        bool[] ShapeChecking = new bool[4];
        // Coin Number
        int[] coins = new int[4];
        coins[0] = 4; coins[1] = 3; coins[2] = 3; coins[3] = 1;
        // Shape description
        string[] shapes = {"squares", "triangle", "axis", "single" };

        int x = 3, y;
        Vector3 position = Vector3.zero;
        while (x < level.width - 1)
        {
            y = 0;
            while (y < level.height - 3)
            {
                if (level.GetPixel(x, y) != Color.clear)
                {
                    // if all element is false , continue
                    // otherwise, 50% will generate.
                    if (checkAreaForShape(level, x, y, ref ShapeChecking) && Random.Range(0,2) == 0) {
                        //else
                        do
                        {
                            //  random from 0 to 3
                            shape = Random.Range(0, 4);
                        }
                        while (!ShapeChecking[shape]);
                        //// generate coin shape
                        position.Set(x, y + 1, 0);
                        CoinShape(position, Coin, shapes[shape]);
                        CoinNumber += coins[shape];
                        if (shape == 4 || shape == 3)
                        {
                            x += 1;
                        }
                        else
                        {
                            x += 2;
                        }
                    }
                }
                y += 1;
            }
            x += 1;
        }
        return CoinNumber;
    }

    private bool checkAreaForShape(Texture2D level, int x, int y, ref bool[] ShapeChecking)
    {
        if (level.GetPixel(x, y + 1) != Color.clear || level.GetPixel(x, y + 2) != Color.clear || level.GetPixel(x + 1, y + 1) != Color.clear || level.GetPixel(x + 1, y + 2) != Color.clear)
        {
            ShapeChecking[0] = false; //  Check Squares.
            ShapeChecking[1] = false; // Check triangle
        }
        else
        {
            ShapeChecking[0] = true;
            ShapeChecking[1] = true;
        }

        // check for axis shape
        if (level.GetPixel(x, y + 1) != Color.clear || level.GetPixel(x, y + 2) != Color.clear || level.GetPixel(x, y + 2) != Color.clear)
        {
            ShapeChecking[2] = false; // check axis
        }
        else
        {
            ShapeChecking[2] = true;
        }

        // check for single
        if (level.GetPixel(x, y + 1) != Color.clear)
        {
            ShapeChecking[3] = false;
        }
        else
        {
            ShapeChecking[3] = true;
        }
        // check if all element is false, return false. Otherwise, return true
        for (int i = 0; i < ShapeChecking.Length; i++)
        {
            if (ShapeChecking[i] == true) return true;
        }
        return false;
    }

    private int CoinShape(Vector3 position, GameObject Coin, string shape)
    {
        // ofset because pivot is center of the gameobject
        position.x += 0.5f;
        position.y += 0.5f;
        if (shape == "squares")
        {
            Instantiate(Coin, position, Quaternion.identity);
            position.x += 1;
            Instantiate(Coin, position, Quaternion.identity);
            position.y += 1;
            Instantiate(Coin, position, Quaternion.identity);
            position.x -= 1;
            Instantiate(Coin, position, Quaternion.identity);
            return 4;
        }
        else if (shape == "triangle")
        {
            Instantiate(Coin, position, Quaternion.identity);
            position.x += 1;
            Instantiate(Coin, position, Quaternion.identity);
            position.x -= 0.5f;
            position.y += 1;
            Instantiate(Coin, position, Quaternion.identity);
        }
        else if (shape == "axis")
        {
            Instantiate(Coin, position, Quaternion.identity);
            position.y += 1;
            Instantiate(Coin, position, Quaternion.identity);
            position.y += 1;
            Instantiate(Coin, position, Quaternion.identity);
        }
        else
        {
            Instantiate(Coin, position, Quaternion.identity);
        }
        return 0;
    }
    private int Des2Index(string des, string[] Sprite2Des)
    {
        for (int i = 0; i < Sprite2Des.Length; i++)
        {
            if (Sprite2Des[i] == des) return i;
        }
        return -1;
    }
    private void SetTileFlagToAllVirtualMap()
    {
        Vector3Int position = Vector3Int.zero;
        for (int x = 0; x < VirtualMap.size.x; x++)
        {
            for (int y = 0; y < VirtualMap.size.y; y++)
            {
                position.Set(x, y, 0);
                if (VirtualMap.HasTile(position))
                {
                    VirtualMap.SetTileFlags(position, TileFlags.None);
                }
            }
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
            else if ((left == true && right == true) || (left == true && righttop == true) || (right == true && lefttop == true)) { 
                description = "center";
            }
            else if (leftbottom == true)
            {
                description = "up";
            }
            else if (rightbottom == true)
            {
                description = "down";
            }
            else if (right == true && left == false)
            {
                description = "left";
            }
            else if (left == true && right == false)
            {
                description = "right";
            }
            else {
                description = "center";
            }
        }
        public void clear()
        {
            left = right = bottom = top = leftbottom = lefttop = rightbottom = righttop = false;
            value = 0;
        }
    }

    // declaration of the class mapping Color and prefab
    [System.Serializable]
    public class ColorToPrefabs {
        public Color color;
        public GameObject prefab;

        public ColorToPrefabs(Color color) {
            this.color = color;
        }
    }
}
