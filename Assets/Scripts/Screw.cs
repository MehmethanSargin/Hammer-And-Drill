using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : MonoBehaviour
{
    public Transform screwHitPosition;
    private float _lastPos = -.2f;
    public bool finish = false;
    public Transform modelRoot;
   
    private void Update()
    {
        if (modelRoot.localPosition.y <= _lastPos)
        {
            finish = true;
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }
   

    public void ScrewDown()
    {
        if (!finish)
        {
           modelRoot.localPosition = Vector3.Lerp(modelRoot.localPosition,new Vector3(modelRoot.localPosition.x,modelRoot.localPosition.y - .3f,modelRoot.localPosition.z),Time.deltaTime);
        }
    }
}
