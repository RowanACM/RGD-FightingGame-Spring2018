using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This component displays the damage value stored in multiple Damage components.
/// </summary>
public class DamageDisplay : MonoBehaviour {

	// null entries in this array are allowed
	public Damage[] damages; // collection of Damage components, NOT their stored values

	// DO NOT use null entries in this array
	public Text[] damageGUI; // collection of UI Text components to display the damage amounts

	// Use this for initialization
	void Start () {
		
	}
	
	void OnGUI () {
		for (int i = 0; i < damageGUI.Length; i++) {
			Damage d;
			if ((d = damages [i]) != null) {
				damageGUI [i].text = Mathf.FloorToInt (d.damage) + "%";
			} else {
				damageGUI [i].text = "";
			}
		}
	}
}
