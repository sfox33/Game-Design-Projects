using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIEditor : MonoBehaviour {

    public Text redBot, blueBot;
    private static string redString, blueString;

    // Use this for initialization
    void Start() {
        redBot.text = blueBot.text = "";
    }

    void Update(){
        if (!redString.Equals("") || !blueString.Equals(""))
        {
            this.startLevel(blueString, redString);
        } else {
            this.endLevel();
        }
    }

    public static void setText(string blue, string red) {
        redString = red;
        blueString = blue;
    }

    public void startLevel(string blue, string red) {
        redBot.text = red;
        blueBot.text = blue;
    }

    public void endLevel() {
        redBot.text = blueBot.text = "";
    }
}
