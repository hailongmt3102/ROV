using UnityEngine;

public class MovingInformation
{
    private int score;
    private Vector3 savePosition;
    private int coin;

    public MovingInformation() {
        score = 0;
        savePosition = Vector3.zero;
        coin = 0;
    }
    public MovingInformation(Vector3 SavePosition) {
        score = 0;
        savePosition = SavePosition;
        coin = 0;
    }

    public int Score {
        get { return score; }
        set { score = value; }
    }

    public Vector3 SavePosition {
        get { return savePosition; }
        set { savePosition = value; }
    }

    public int Star {
        get { return coin; }
        set { coin = value; }
    }
}
