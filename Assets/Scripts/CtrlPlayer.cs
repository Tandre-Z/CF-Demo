using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlPlayer : BaseHuman
{
    // Start is called before the first frame update
   new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit))
            {
                //Debug.Log(hit.point);
                if(hit.transform.tag.Equals("Plane"))
                {
                    MoveTo(hit.point);
                    NetManager.Manager.SendAsync("Move");
                }
            }
        }
    }
}
