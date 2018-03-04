using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileScreen : MonoBehaviour {
    public Text NameText; 

    public void Set(User user) {
        NameText.text = user.Name;
    }
}
