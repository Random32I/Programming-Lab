using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] TMP_Text jumpText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        jumpText.text = $"Jumps:\n{player.GetJumps()}";
    }
}
