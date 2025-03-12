using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RunAnims : MonoBehaviour
{
    [SerializeField] private AnimationClip run;
    [SerializeField] private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim.Play(run.name);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
