using UnityEngine;

public class MovingInformation
{
    private Vector3 savePosition;
    private int coin;

    public MovingInformation() {
        savePosition = Vector3.zero;
        coin = 0;
    }
    public MovingInformation(Vector3 SavePosition) {
        savePosition = SavePosition;
        coin = 0;
    }

    public Vector3 SavePosition {
        get { return savePosition; }
        set { savePosition = value; }
    }

    public int Coin {
        get { return coin; }
        set { coin = value; }
    } 

    public void addCoin() {
        coin += 1;
    }
}
