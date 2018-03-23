using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPicker : MonoBehaviour {

    public GameObject[] Playerlist = new GameObject[8]; // just a list of player 
    public GameObject[] playerveiwer = new GameObject[8];// list of the game objects
    public GameObject highlight; // show the player pointer is at
    public GameObject getplayer;
    public static int num = 0;
    	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.RightArrow)) { num += 1; }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { num -= 1; }
        if (num == 8) { num = 0; }
        if (num == -1) { num = 7; }

        float X = playerveiwer[num].transform.position.x;
        float Y = playerveiwer[num].transform.position.y;
        highlight.transform.localPosition = new Vector3(X, Y, highlight.transform.position.z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            getplayer = Playerlist[num];
            Application.LoadLevel("DefaultLevel");
        }
    }
   public GameObject GPlayer()
    {
    return getplayer;
    }
}

