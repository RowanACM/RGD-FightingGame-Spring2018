using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class loadtogame : MonoBehaviour {
   public void Loadlevel(string name)
    {
		SceneManager.LoadScene(name);
    }
}
