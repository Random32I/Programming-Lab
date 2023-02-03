using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody rig;
    [SerializeField] int jumpForce;
    [SerializeField] int maxSpeed;
    [SerializeField] int speed;

    [SerializeField] Vector2 sensitivity;
    [SerializeField] Vector2 rotation;

    private Vector2 GetInput()
    {
        Vector2 input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        return input;
    }


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rotation = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 wantedVelocity = GetInput() * sensitivity;

        if (rotation.y < 90 && rotation.y > -90)
        {
            rotation += wantedVelocity * Time.deltaTime;

            rotation = new Vector2(rotation.x % 360, rotation.y % 360);

            GameObject.Find("Main Camera").transform.localEulerAngles = new Vector3(-rotation.y, rotation.x, 0);
        }
        if (rotation.y > 90)
        {
            rotation.y = 89.99f;
        }
        else if (rotation.y < -90)
        {
            rotation.y = -89.99f;
        }

        //transform.Rotate(new Vector3(0, Input.GetAxisRaw("Horizontal") / 3, 0));

        if (Input.GetKeyDown(KeyCode.Space) && rig.velocity.y == 0)
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        rig.velocity = new Vector3(((float)Math.Sin(GameObject.Find("Main Camera").transform.rotation.eulerAngles.y * Math.PI / 180) * Input.GetAxisRaw("Vertical") * speed) + ((float)Math.Cos(GameObject.Find("Main Camera").transform.rotation.eulerAngles.y * Math.PI / 180) * Input.GetAxisRaw("Horizontal") * speed), rig.velocity.y, 
            ((float)Math.Cos(GameObject.Find("Main Camera").transform.rotation.eulerAngles.y * Math.PI / 180) * Input.GetAxisRaw("Vertical") * speed) - ((float)Math.Sin(GameObject.Find("Main Camera").transform.rotation.eulerAngles.y * Math.PI / 180) * Input.GetAxisRaw("Horizontal") * speed));

        //rig.MovePosition(rig.position + movement * speed * Time.deltaTime);
        
        /*if (rig.velocity.x >= maxSpeed)
        {
            rig.velocity = new Vector3(maxSpeed, rig.velocity.y, rig.velocity.z);
        }
        else if (rig.velocity.z >= maxSpeed)
        {
            rig.velocity = new Vector3(rig.velocity.x, rig.velocity.y, maxSpeed);
        }*/
    }
}
