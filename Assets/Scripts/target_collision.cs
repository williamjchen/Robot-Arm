using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target_collision : MonoBehaviour
{
    public GameObject platform;
    public GameObject cylinder;
    public GameObject small_X;

    public Rigidbody target;

    public Vector3 center;
    public Vector2 platform_size;

    public float timer = 3f;
    public bool on = false;

    // Start is called before the first frame update
    void Start()
    {
        //spawnTarget();
    }

    void Awake(){
        //spawnTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if(on){
            timer -= Time.deltaTime;
            if(timer <= 0f){
                Debug.Log("Winner Winner Chicken Dinner!");
                platform.GetComponent<Roll>().reward(1f);
                platform.GetComponent<Roll>().end();
            }
        }
    }
    
    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.name == "Cylinder"){
            //Debug.Log("cashmoney");
            on = true;
            platform.GetComponent<Roll>().reward(0.5f);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.name == "Cylinder"){
            //Debug.Log("not so cash money");
            on = false;
            timer = 3f;
        }
    }
    
    public void spawnTarget(){
        float x = Random.Range(-platform_size.x/3, platform_size.x/3);
        float z = Random.Range(-platform_size.y/3, platform_size.y/3);
        Vector3 spawn_pos = center + new Vector3(x, 0, z);
        cylinder.transform.position = spawn_pos;
        //target.MovePosition(spawn_pos);

        small_X.transform.localPosition = new Vector3(x, 0.271f, z);
        //small_X.GetComponent<X_position>().spawnX(new Vector2(x, z));
    }
}
