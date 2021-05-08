using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : MonoBehaviour
{
    public bool hammerMoved;
    public bool hammerRotated;
    public GameManager gameManager;



    public void ResetPosition()
    {
        transform.SetParent(null);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.position = new Vector3(-0.72855f, 0f, -0.01570f);
    }
    
    
    public IEnumerator HammerMoved()
    {
        hammerMoved = true;
        float timer = 0f;
        if (gameManager.hit.collider.GetComponent<Nail>())
        {
            transform.SetParent(gameManager.currentNail.nailHitPosition);
        }

        if (gameManager.hit.collider.GetComponent<Fence>())
        {
           transform.SetParent(gameManager.currentFence.fenceHitPosition); 
        }
        while (true)
        {
            timer += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, timer);
            transform.localRotation = Quaternion.Lerp(transform.localRotation,Quaternion.identity, timer);
            if (timer>=1)
            {
                hammerMoved = false;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    
    
    public IEnumerator HammerRotated()
    {
        hammerRotated = true;
        float timer = 0f;
        while (true)
        {
            timer += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, new Quaternion(0,0,.1f,.1f),timer);
            if (timer>=1)
            {
                hammerRotated = false;
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        if (gameManager.hit.collider !=null)
        {
            if (gameManager.hit.collider.GetComponent<Nail>())
            {
                gameManager.currentNail.NailDown();   
            }

            if (gameManager.hit.collider.GetComponent<Fence>())
            {
                gameManager.currentFence.FenceDown();
            } 
        }
        
    }

}
