using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FloorButton : MonoBehaviour
{
    [SerializeField] Material buttonColor;
    [SerializeField] Material activeButtonColor;
    [SerializeField] GameObject particles;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Object")
        {
            gameObject.GetComponent<Renderer>().material = activeButtonColor;
            particles.GetComponent<ParticleSystem>().Play();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Object")
        {
            gameObject.GetComponent<Renderer>().material = buttonColor;
            particles.GetComponent<ParticleSystem>().Stop();
        }
    }
}
