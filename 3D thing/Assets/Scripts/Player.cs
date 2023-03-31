using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDataPersistence
{
    [SerializeField] Rigidbody rig;
    [SerializeField] int jumpForce;
    [SerializeField] int maxSpeed;
    [SerializeField] int speed;
    [SerializeField] int jumps = 50;
    Rigidbody hitRig;
    bool holding;
    bool grounded;
    bool crouching = false;

    [SerializeField] Vector2 sensitivity;
    [SerializeField] Vector2 rotation;
    [SerializeField] Vector3 lastPos;
    [SerializeField] RenderTexture camView;
    [SerializeField] GameManager game;


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
        if (!game.GetPaused())
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
            if (Physics.Raycast(transform.position, Vector3.down, GetComponent<Collider>().bounds.extents.y + 0.1f))
            {
                grounded = true;
            }
            else
            {
                grounded = false;
            }

            if (Input.GetKeyDown(game.GetControls(0)) && grounded && jumps > 0)
            {
                if (SceneManager.GetActiveScene().name == "Game")
                {
                    if (transform.position.x <= -3.14f && transform.position.x >= -13.17f && transform.position.z >= -5.55f && transform.position.z <= 4.45f)
                    {
                        rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                        jumps--;
                    }
                }
                else
                {
                    rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
            }

            //Crouch
            if (Input.GetKey(KeyCode.LeftControl) && !crouching)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 2, transform.localScale.z);
                crouching = true;
                transform.position += Vector3.down / 2;
            }

            if (Input.GetKeyUp(KeyCode.LeftControl) && crouching)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2, transform.localScale.z);
                crouching = false;
            }

            //Interact
            if (Input.GetKeyDown(game.GetControls(1)))
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
                            if (SceneManager.GetActiveScene().name == "Game")
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
                            else
                            {
                                SceneManager.LoadScene("Game");
                            }
                        }
                        //Objects
                        else
                        {
                            holding = true;
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
                hitRig.rotation = Quaternion.Euler(new Vector3(0, GameObject.Find("Main Camera").transform.eulerAngles.y, 0));
            }
        }

        //Pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (game.GetPaused())
            {
                game.SetPaused(false);
                Cursor.lockState = CursorLockMode.Locked;
                game.SetControls(false);
                Time.timeScale = 1;
            }
            else
            {
                game.SetPaused(true);
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
            }
        }

        //Debug
    }

    private void FixedUpdate()
    {
        if (!game.GetPaused())
        {
            //Movement (probably a better way, but it works)
            rig.velocity = new Vector3(((float)Math.Sin(GameObject.Find("Main Camera").transform.rotation.eulerAngles.y * Math.PI / 180) * Input.GetAxisRaw("Vertical") * speed) + ((float)Math.Cos(GameObject.Find("Main Camera").transform.rotation.eulerAngles.y * Math.PI / 180) * Input.GetAxisRaw("Horizontal") * speed), rig.velocity.y,
                ((float)Math.Cos(GameObject.Find("Main Camera").transform.rotation.eulerAngles.y * Math.PI / 180) * Input.GetAxisRaw("Vertical") * speed) - ((float)Math.Sin(GameObject.Find("Main Camera").transform.rotation.eulerAngles.y * Math.PI / 180) * Input.GetAxisRaw("Horizontal") * speed));

            //Moves held object infront of you while maintaining velocity
            if (hitRig)
            {
                hitRig.velocity = (GameObject.Find("Main Camera").transform.position + GameObject.Find("Main Camera").transform.forward * 2 - lastPos) / Time.deltaTime;
            }
            if (crouching)
            {
                rig.velocity = new Vector3(rig.velocity.x / 2, rig.velocity.y, rig.velocity.z / 2);
            }
        }
    }

    private void LateUpdate()
    {
        //remembering the last position of a held object to calculate velocity
        if (hitRig)
        {
            lastPos = hitRig.transform.position;
        }
    }

    public int GetJumps()
    {
        return jumps;
    }

    public void LoadData(GameData data)
    {
        this.jumps = data.jumps;
    }

    public void SaveData(ref GameData data)
    {
        data.jumps = this.jumps;
    }
}
