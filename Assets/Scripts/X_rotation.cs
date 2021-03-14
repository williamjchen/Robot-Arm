using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X_rotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rotation(Quaternion rotation){
        transform.rotation = rotation;
    }
}
