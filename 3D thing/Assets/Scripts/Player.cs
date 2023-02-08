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
    Rigidbody hitRig;
    bool holding;

    [SerializeField] Vector2 sensitivity;
    [SerializeField] Vector2 rotation;
    [SerializeField] RenderTexture camView;

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
            RaycastHit hit;
            int layerMask;

            if (!holding)
            {
                layerMask = 1 << 8;

                if (Physics.Raycast(GameObject.Find("Main Camera").transform.position, GameObject.Find("Main Camera").transform.forward, out hit, 5, layerMask))
                {
                    hitRig = hit.rigidbody;

                    //Button
                    if (hit.transform.tag == "Button")
                    {
                        if (hit.transform.name == "Button 1")
                        {
                            GameObject.Find("Camera 1").GetComponent<Camera>().targetTexture = camView;
                            GameObject.Find("Camera 2").GetComponent<Camera>().targetTexture = null;
                        }
                        else if (hit.transform.name == "Button 2")
                        {
                            GameObject.Find("Camera 2").GetComponent<Camera>().targetTexture = camView;
                            GameObject.Find("Camera 1").GetComponent<Camera>().targetTexture = null;
                        }
                    }
                    //Objects
                    else
                    {
                        holding = true;
                        hitRig.gameObject.transform.position = transform.position + GameObject.Find("Main Camera").transform.forward * 2 + Vector3.up / 2;
                        hitRig.useGravity = false;
                        hitRig.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                        hitRig.velocity = Vector3.zero;
                    }
                }
            }
            else
            {
                holding = false;
                hitRig.useGravity = true;
                hitRig.constraints = RigidbodyConstraints.None;
                hitRig = null;
            }
        }
        if (hitRig)
        {
            hitRig.gameObject.transform.position = transform.position + GameObject.Find("Main Camera").transform.forward * 2 + Vector3.up / 2;
            hitRig.rotation = Quaternion.Euler(new Vector3(0, GameObject.Find("Main Camera").transform.eulerAngles.y, 0));
        }

        //Exit
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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
        //Movement (probably a better way, but it works)
        rig.velocity = new Vector3(((float)Math.Sin(GameObject.Find("Main Camera").transform.rotation.eulerAngles.y * Math.PI / 180) * Input.GetAxisRaw("Vertical") * speed) + ((float)Math.Cos(GameObject.Find("Main Camera").transform.rotation.eulerAngles.y * Math.PI / 180) * Input.GetAxisRaw("Horizontal") * speed), rig.velocity.y, 
            ((float)Math.Cos(GameObject.Find("Main Camera").transform.rotation.eulerAngles.y * Math.PI / 180) * Input.GetAxisRaw("Vertical") * speed) - ((float)Math.Sin(GameObject.Find("Main Camera").transform.rotation.eulerAngles.y * Math.PI / 180) * Input.GetAxisRaw("Horizontal") * speed));
        //rig.velocity = new Vector3(GameObject.Find("Main Camera").transform.forward.x * Input.GetAxisRaw("Horizontal") * speed - GameObject.Find("Main Camera").transform.forward.x * Input.GetAxisRaw("Vertical") * speed, rig.velocity.y, GameObject.Find("Main Camera").transform.forward.z * Input.GetAxisRaw("Vertical") * speed - GameObject.Find("Main Camera").transform.forward.z * Input.GetAxisRaw("Horizontal") * speed);
    }
}
