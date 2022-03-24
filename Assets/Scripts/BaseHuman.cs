using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHuman : MonoBehaviour
{
    [Header("是否正在移动")]
    [Tooltip("是否正在移动")]
    [SerializeField]
    public bool isMoveing;


    [SerializeField]
    [Header("目标点")]
    public Vector3 targetPos;

    [Header("移动速度")]
    public float speed;

    [Header("动画对象")]
    public Animator animator;

    public void MoveTo(Vector3 target)
    {
        targetPos = target;
        isMoveing = true;
        animator.SetInteger("State", 1);
    }

    private void MoveUpdate()
    {
        if (isMoveing)
        {
            Vector3 pos = transform.position;
            transform.position = Vector3.MoveTowards(pos, targetPos, speed * Time.deltaTime);
            transform.LookAt(targetPos);

            if(Vector3.Distance(pos, targetPos) <0.05f)
            {
                isMoveing = false;
                animator.SetInteger("State", 0);
            }
        }
    }
    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        MoveUpdate();
    }
}
