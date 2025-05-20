using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class offAnim : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // tat anim goi y cua piece
    public void SetOffAnimHint()
    {
        transform.localPosition = Vector3.zero;
        board.instance.timer = 0;
        board.instance.listHint = new List<GameObject>();
        anim.SetBool("hint", false);
    }
}
