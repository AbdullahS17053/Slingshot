using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCaller : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator _animator;
    public string Animation_Name;
    private void Awake()
    {
        _animator.keepAnimatorStateOnDisable = true;
    }
    private void OnEnable()
    {
        //Debug.Log("Enabled!");
        // _animator = GetComponent<Animator>();
        _animator.SetTrigger(Animation_Name);
        
    }
}
