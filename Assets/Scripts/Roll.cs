using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System.IO.Ports;


public class Roll : Agent
{
    EnvironmentParameters defaultParameters;

    public GameObject Cube;
    public GameObject Sphere;
    public GameObject Cylinder;
    public GameObject Big_X;
    public GameObject smol_X;
    public Rigidbody platform;
    public Rigidbody ball;
    public Rigidbody target;

    public Vector2 move;
    public Controls controls;

    public float max_angle;
    public float rotation_speed;
    public float rotation_friction;
    public float rotation_smoothness;

    public float game_timer = 10f;
    public bool off = false;
    public bool on = false;

    public Vector2 size;
    public Vector3 center;

    SerialPort Serial_port = new SerialPort("COM3", 38400);

    void OnApplicationQuit(){
        Serial_port.Close();
    }

    public override void Initialize(){
        Time.timeScale = 1f;
        platform = GetComponent<Rigidbody>();
        ball = Sphere.GetComponent<Rigidbody>();
        target = Cylinder.GetComponent<Rigidbody>();
        defaultParameters = Academy.Instance.EnvironmentParameters;
        Reset();
        Serial_port.Open();

        controls = new Controls();
        controls.Player.Movement.performed += ctx => move=ctx.ReadValue<Vector2>();
        controls.Player.Movement.Enable();
        controls.Player.Movement.canceled += ctx => move=Vector2.zero;
    }

    public override void OnActionReceived(float[] vectorAction){
        Debug.Log("h:"+vectorAction[0]+"v:"+vectorAction[1]);
        // Debug.Log("mh:"+move[0]+"v:"+move[1]);

        Quaternion rotate_To = Quaternion.Euler(vectorAction[1]*max_angle, 0, vectorAction[0]*max_angle*-1);
        Quaternion rotation = Quaternion.Lerp(transform.rotation, rotate_To, Time.deltaTime*rotation_smoothness);
        transform.rotation = rotation;
        Big_X.transform.rotation = rotation;
        //Big_X.GetComponent<X_rotation>().Rotation(rotation);
        sendToArduino(vectorAction[1], vectorAction[0]);


        if(Sphere.transform.position.y - transform.position.y < -3.5f || Mathf.Abs(Sphere.transform.position.x - transform.position.x) > 3.5f || Mathf.Abs(Sphere.transform.position.z - transform.position.z) > 3.5f){
            Debug.Log("Dead!");
            reward(-1f);
            end();
        }else{
            float distance = Vector2.Distance(new Vector2(Cylinder.transform.position.x, Cylinder.transform.position.z), new Vector2(Sphere.transform.position.x, Sphere.transform.position.z));
            distance = Mathf.Clamp(distance, 0f, 1f);

            reward(0.1f);
            reward((1-distance)/10, false);
            reward(-0.05f, false);
        }
        //Debug.Log(GetCumulativeReward());
    }

    public override void OnEpisodeBegin(){
        Vector3 spawn_pos = center + new Vector3(Random.Range(-size.x/4, size.x/4), 3, Random.Range(-size.y/4, size.y/4));
        ball.MovePosition(spawn_pos);
        //Sphere.transform.position = spawn_pos;
        ball.velocity = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-3, 0), Random.Range(-2.5f, 2.5f));
        Sphere.GetComponent<target_collision>().spawnTarget();
        Reset();
    } 

    public void Reset(){
        ball.mass = defaultParameters.GetWithDefault("mass", 1.0f);
        var scale = defaultParameters.GetWithDefault("scale", 1.0f);
        Sphere.transform.localScale = new Vector3(scale, scale, scale);
    }

    public override void CollectObservations(VectorSensor sensor){
        if(ball != null){
            sensor.AddObservation(ball.velocity);
            //sensor.AddObservation(Sphere.transform.position);
            sensor.AddObservation(Sphere.transform.position - transform.position);
            sensor.AddObservation(Cylinder.transform.position.x);
            sensor.AddObservation(Cylinder.transform.position.z);
            sensor.AddObservation(transform.rotation.z);
            sensor.AddObservation(transform.rotation.x);
        }else{
            Debug.Log("NULLLLLLLL");
            sensor.AddObservation(Vector3.zero);
            sensor.AddObservation(Vector3.zero - transform.position);
            sensor.AddObservation(Cylinder.transform.position.x);
            sensor.AddObservation(Cylinder.transform.position.z);
            sensor.AddObservation(transform.rotation.z);
            sensor.AddObservation(transform.rotation.x);
        }
    }

    public override void Heuristic(float[] actionsOut){
        actionsOut[0] = controls.Player.Movement.ReadValue<Vector2>()[0];
        actionsOut[1] = controls.Player.Movement.ReadValue<Vector2>()[1];
    }

    public void FixedUpdate(){
        if(game_timer > 0){
            game_timer -= Time.deltaTime;
        }else{
            Debug.Log("Time is up!");
            end();
        }
    }

    public void sendToArduino(float vertical, float horizontal){
        byte[] message = new byte[6];
        message[0] = 254;
        message[5] = 255;

        //Debug.Log((int)(vertical*253));
        int converted_vertical_byte = Mathf.Abs((int)(vertical * 253));
        byte vertical_byte = (byte)converted_vertical_byte;

        int converted_horizontal_byte = Mathf.Abs((int)(horizontal * 253));
        byte horizontal_byte = (byte)converted_horizontal_byte;
        //Debug.Log((int)(horizontal*253));
        
        message[1] = (byte)(vertical < 0 ? 0 : 1);
        message[2] = vertical_byte;
        message[3] = (byte)(horizontal < 0 ? 1 : 0);
        message[4] = horizontal_byte;

        //Serial_port.Open();
        Serial_port.Write(message, 0, 6);
        //Serial_port.Close();
    }

    public void reward(float amount, bool set = true){
        if(set){
            SetReward(amount);
        }else{
            AddReward(amount);
        }
    }

    public void end(){
        EndEpisode();
        game_timer = 10f;
    }
}
