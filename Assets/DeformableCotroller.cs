using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deform;

public class vinnydrunk : MonoBehaviour
{
public SquashAndStretchDeformer squash;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.X))
        {squash.Factor+=0.03f;}
        if(Input.GetKey(KeyCode.Z))
        {squash.Factor-=0.03f;}
    }
}