using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadPlayer : MonoBehaviour {
    public GameObject[] Playerlist = new GameObject[8];
    int num2=0;
    // Use this for initialization
    void Start() {
        num2 = PlayerPicker.num;
        Playerlist[num2].transform.localPosition = new Vector3(0.1f, -0.51f, 0);
        for(int i = 0; i < 8; i++)
        {
			if (i != num2) {
				Destroy (Playerlist [i]);
			} else {
				FindObjectOfType<DamageDisplay> ().damages [0] = Playerlist [i].GetComponent<Damage> ();
			}
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
