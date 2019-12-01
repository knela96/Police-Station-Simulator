using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    float Speed = 50.0f;   
    private float dps = 1.0f;
    public float MouseSpeed = 200.0f;    
    private Vector3 MousePos;
    bool buttonDown;

    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            // Get mouse origin
            MousePos = Input.mousePosition;
            buttonDown = true;
        }

        if (!Input.GetMouseButton(1))
        {
            buttonDown = false;
        }

        if (buttonDown == true)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - MousePos);
            transform.RotateAround(transform.position, transform.right, -pos.y * MouseSpeed);
            transform.RotateAround(transform.position, Vector3.up, pos.x * MouseSpeed);
        }
       
        Vector3 p = WASDMovement();
        dps = Mathf.Clamp(dps * 0.5f, 1f, 1000f);
        p = p * Speed;        
        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        transform.Translate(p);
       
    }

    private Vector3 WASDMovement()
    { 
        Vector3 WASDSpeed = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            WASDSpeed += new Vector3(0, 0, 0.6f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            WASDSpeed += new Vector3(0, 0, -0.6f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            WASDSpeed += new Vector3(-1, 0, 0.6f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            WASDSpeed += new Vector3(1, 0, 0.6f);
        }
        return WASDSpeed;
    }
}