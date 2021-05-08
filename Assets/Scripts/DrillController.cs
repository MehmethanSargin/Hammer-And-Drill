using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillController : MonoBehaviour
{
    public bool drillMoved;
    public GameManager gameManager;
    
    public void ResetPosition()
    {
        transform.SetParent(null);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.position = new Vector3(0f, 0.349f, 0f);
    }

    public IEnumerator DrillMoved()
    {
        drillMoved = true;
        float timer = 0f;
        transform.SetParent(gameManager.currentScrew.screwHitPosition);
        while (true)
        {
            timer += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition,Vector3.zero,timer);
            transform.localRotation = Quaternion.Lerp(transform.localRotation,Quaternion.Euler(-90,0,0), timer );
            if (timer>=1)
            {
                drillMoved = false;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
