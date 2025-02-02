using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public Canvas mouseGame;

    [SerializeField]
    private Image pawa;

    [SerializeField]
    private Image paws;

    [SerializeField]
    private Image pawd;

    [SerializeField]
    private Image pawf;

    // Start is called before the first frame update
    void Start()
    {
        mouseGame.enabled = false;
    }

    // Update is called once per frame
    void Update() { }

    public void MouseMiniGamePlay()
    {
        mouseGame.enabled = true;
        pawa.enabled = false;
        paws.enabled = false;
        pawd.enabled = false;
        pawf.enabled = false;
        if (Input.GetKey(KeyCode.A))
        {
            pawa.enabled = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            paws.enabled = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            pawd.enabled = true;
        }
        if (Input.GetKey(KeyCode.F))
        {
            pawf.enabled = true;
        }
    }
}
