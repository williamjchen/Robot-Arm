using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X_position : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void spawnX(Vector2 spawn_pos){
        //transform.position = transform.position + new Vector3(spawn_pos.x, 0.24f, spawn_pos.z);   
        //Debug.Log(spawn_pos); 
        Vector3 coords = new Vector3(spawn_pos.x, 0.271f, spawn_pos.y);
        //Debug.Log(coords);
        //transform.position = new Vector3(0f, 0f, 0f) + new Vector3(spawn_pos.x, 0.24f, spawn_pos.y);
        transform.localPosition = coords;
    }
}
