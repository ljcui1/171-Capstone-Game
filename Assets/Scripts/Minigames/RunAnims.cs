using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RunAnims : MonoBehaviour
{
    [SerializeField] private AnimationClip run;
    [SerializeField] private Animator anim;
    [SerializeField] private GameManager gameMan;
    // Start is called before the first frame update
    void Start()
    {
        anim.Play(run.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMan.nightOrDay == GameManager.NightOrDay.DAY)
        {
            anim.speed = 1;
        }
        else
        {
            anim.speed = 0;
        }
    }
}
