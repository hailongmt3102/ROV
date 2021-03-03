using UnityEngine;

public class MapSelection : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject[] maps;
    void Start()
    {
        Transform[] child = gameObject.GetComponentsInChildren<Transform>();
        // the child will start from 1 st
    }

}
