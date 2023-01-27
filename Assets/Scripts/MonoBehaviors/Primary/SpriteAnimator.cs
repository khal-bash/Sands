using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{

    public Animator animator;

    private void Start()
    {
        animator.SetBool("isMovingLeft", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("isMovingLeft", true);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            animator.SetBool("isMovingLeft", false);
        }
    }
}
