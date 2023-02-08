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
    Rigidbody boxRig;
    bool holding;

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

        //Mouse Movement
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

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && rig.velocity.y == 0)
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        //Interact
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!holding)
            {
                RaycastHit hit;
                int layerMask = 1 << 8;

                if (Physics.Raycast(transform.position, GameObject.Find("Main Camera").transform.forward, out hit, 5, layerMask))
                {
                    holding = true;
                    boxRig = hit.rigidbody;

                    boxRig.gameObject.transform.position = transform.position + GameObject.Find("Main Camera").transform.forward * 2 + Vector3.up /2;
                    boxRig.useGravity = false;
                    boxRig.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                    boxRig.velocity = Vector3.zero;
                }
            }
            else
            {
                holding = false;
                boxRig.useGravity = true;
                boxRig.constraints = RigidbodyConstraints.None;
                boxRig = null;
            }
        }
        if (boxRig)
        {
            boxRig.gameObject.transform.position = transform.position + GameObject.Find("Main Camera").transform.forward * 2 + Vector3.up / 2;
            boxRig.rotation = Quaternion.Euler(new Vector3(0, GameObject.Find("Main Camera").transform.eulerAngles.y, 0));
        }

        //Debug
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log(new Vector3(GameObject.Find("Main Camera").transform.forward.x * Input.GetAxisRaw("Vertical") * speed, rig.velocity.y, GameObject.Find("Main Camera").transform.forward.z * Input.GetAxisRaw("Horizontal") * speed));
            Debug.Log(GameObject.Find("Main Camera").transform.forward);
            Debug.Log(new Vector2 (Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal")));
        }
    }

    private void FixedUpdate()
    {
        //Movement (probly a better way, but it works)
        rig.velocity = new Vector3(((float)Math.Sin(GameObject.Find("Main Camera").transform.rotation.eulerAngles.y * Math.PI / 180) * Input.GetAxisRaw("Vertical") * speed) + ((float)Math.Cos(GameObject.Find("Main Camera").transform.rotation.eulerAngles.y * Math.PI / 180) * Input.GetAxisRaw("Horizontal") * speed), rig.velocity.y, 
            ((float)Math.Cos(GameObject.Find("Main Camera").transform.rotation.eulerAngles.y * Math.PI / 180) * Input.GetAxisRaw("Vertical") * speed) - ((float)Math.Sin(GameObject.Find("Main Camera").transform.rotation.eulerAngles.y * Math.PI / 180) * Input.GetAxisRaw("Horizontal") * speed));
        //rig.velocity = new Vector3(GameObject.Find("Main Camera").transform.forward.x * Input.GetAxisRaw("Horizontal") * speed - GameObject.Find("Main Camera").transform.forward.x * Input.GetAxisRaw("Vertical") * speed, rig.velocity.y, GameObject.Find("Main Camera").transform.forward.z * Input.GetAxisRaw("Vertical") * speed - GameObject.Find("Main Camera").transform.forward.z * Input.GetAxisRaw("Horizontal") * speed);
    }
}
