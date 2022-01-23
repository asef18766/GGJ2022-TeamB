using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class fog : MonoBehaviour
{
    [SerializeField] VisualEffect fogvfx;
    [SerializeField] Transform pos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fogvfx.SetVector3("cp3", pos.position);
    }
}
