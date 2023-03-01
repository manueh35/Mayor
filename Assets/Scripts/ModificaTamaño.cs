using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModificaTamaño : MonoBehaviour
{
    public float ValorStat;
    public Animator m_Animator;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        //this.transform.localScale = new Vector3(5f, 5f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (AnimatorIsPlaying()==false){
            m_Animator.GetComponent<Animator>().enabled = false;
        }
        
        this.transform.localScale = new Vector3(1f, ValorStat, 1f);

        
    }
    bool AnimatorIsPlaying()
    {
        return m_Animator.GetCurrentAnimatorStateInfo(0).length >
               m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}
