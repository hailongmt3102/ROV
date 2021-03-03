using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuStateController : MonoBehaviour
{
    public EventForButtonState[] ButtonForStates;
    public GameObject[] obj4States;
    static private List<string> state = new List<string>();
    private void Start()
    {
        foreach (EventForButtonState b4t in ButtonForStates) {
            b4t.obj.GetComponent<Button>().onClick.AddListener(delegate { nextState(b4t.state); });
        }
        // find key
        if (!PlayerPrefs.HasKey("state")) {
            PlayerPrefs.SetString("state", "Menu");
        }
        if (!PlayerPrefs.HasKey("played")) {
            PlayerPrefs.SetInt("played", 0);
        }
        state.Add("Menu");
        foreach (GameObject obj4State in obj4States)
        {
            if (obj4State.name == "Menu")
            {
                obj4State.SetActive(true);
            }
            else
            {
                obj4State.SetActive(false);
            }
        }
    }

    private void nextState(string NewState) {
        state.Add(NewState);
        foreach (GameObject obj4State in obj4States) {
            if (obj4State.name == NewState || obj4State.name == "UI")
            {
                obj4State.SetActive(true);
            }
            else {
                obj4State.SetActive(false);
            }
        }
    }

    public void previousState() {
        if (state.Count <= 1) {
            Debug.LogError("Something wrong in list state");
        }
        state.RemoveAt(state.Count - 1);
        string currentState = state[state.Count - 1];
        foreach (GameObject obj4State in obj4States)
        {
            if (obj4State.name == currentState || (obj4State.name == "UI" && currentState != "Menu"))
            {
                obj4State.SetActive(true);
            }
            else
            {
                obj4State.SetActive(false);
            }
        }
    }
}

[Serializable]
public class EventForButtonState{
    public GameObject obj;
    public string state;
    public EventForButtonState() {
        state = "";
    }
}
