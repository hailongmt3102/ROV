using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangeOpacity : MonoBehaviour
{
    // The color will be changed.
    public Color ColorChanging = new Color(200, 150, 200, 50);

    // get player location.
    public Transform Player;

    // range of tile will be changed. It caculated by cell.
    public int Range = 2;

    // when player is moving near by virtual ground. It will change its color.
    private Tilemap VirtualMap;
    
    // player Position and player cell Position
    private Vector3 PlayerPosition;
    private Vector3Int PlayerCell;
    private Vector3Int NewPlayerCell;

    // current Position, use in loop
    private  Vector3Int CurrentPosition = Vector3Int.zero;
    private void Start()
    {
        VirtualMap = GetComponent<Tilemap>();
        // make sure that the Player name exist in list game object.
        Player = GameObject.Find("Player").transform;
    }

    void FixedUpdate()
    {
        // get player Position
        PlayerPosition = Player.position;
        // get cell Position of player.
        NewPlayerCell = VirtualMap.LocalToCell(PlayerPosition);
        // when new Position is changed, set some tile with default color.
        if (NewPlayerCell != PlayerCell)
        {
            excuteWhenChangePos(PlayerCell, NewPlayerCell, Range);
            PlayerCell = NewPlayerCell;
        }
        else {
            return;
        }
        for (int x = PlayerCell.x - Range; x <= PlayerCell.x + Range; x++)
        {
            for (int y = PlayerCell.y - Range; y <= PlayerCell.y + Range; y++)
            {
                // check xy coordinate is correct.
                if (x >= 0 && x < VirtualMap.size.x && y >= 0 && y < VirtualMap.size.y)
                {
                    CurrentPosition.Set(x, y, 0);
                    // if xy coordinate has tile in virtual ground.
                    if (VirtualMap.HasTile(CurrentPosition))
                    {
                        VirtualMap.SetColor(CurrentPosition, ColorChanging);
                    }
                }
            }
        }
    }

    private void excuteWhenChangePos(Vector3Int oldPos, Vector3Int NewPos, int Range) {
        int fromx   = NewPos.x > oldPos.x ? oldPos.x - Range : NewPos.x + Range;
        int tox     = NewPos.x > oldPos.x ? NewPos.x - Range : oldPos.x + Range;
        int fromy   = NewPos.y > oldPos.y ? oldPos.y - Range : NewPos.y + Range;
        int toy     = NewPos.y > oldPos.y ? NewPos.y - Range : oldPos.y + Range;
        if (fromx == tox) {
            fromx -= 2 * Range;
        }
        if (fromy == toy) {
            fromy -= 2 * Range;
        }
        for (int x = fromx; x <= tox; x++) {
            for (int y = fromy; y <= toy; y++) { 
                CurrentPosition.Set(x, y, 0);
                if (VirtualMap.HasTile(CurrentPosition)) {
                    VirtualMap.SetColor(CurrentPosition, Color.white);
                }
            }
        }
    }
}
