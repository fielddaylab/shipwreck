﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoSlot : MonoBehaviour
{
    public string targetKey;

    // Start is called before the first frame update
    void Start()
    {
        PlayerProgress.instance?.SetPhotoPresence(this);
    }

    // Update is called once per frame
    void Update()
    {

    }
}