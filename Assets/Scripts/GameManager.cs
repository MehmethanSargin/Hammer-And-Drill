using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public List<Nail> FirstNails = new List<Nail>();
    public List<Nail> SecondNails = new List<Nail>();
    public List<Screw> FirstScrews = new List<Screw>();
    public List<Screw> SecondScrews = new List<Screw>();
    public List<Fence> FirstFences = new List<Fence>();
    public List<Fence> SecondFences = new List<Fence>();
    private Ray _lastRay;
    public HammerController hammerController;
    public DrillController drillController;
    public Screw currentScrew;
    public Nail currentNail;
    public Fence currentFence;
    private Camera _mainCamera;
    public Transform CameraSecondPos;
    public  RaycastHit hit;
    public GameObject GameOverPanel;
    private void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            _lastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(_lastRay, out hit);
            if (hit.collider == null)
            {
                return;
            }
            if (hit.collider.GetComponent<Screw>())
            {
                currentScrew = hit.collider.GetComponent<Screw>();
                currentScrew.ScrewDown();
            }
            Debug.DrawRay(_lastRay.origin,_lastRay.direction * 10,Color.red);
            
            if (!hammerController.hammerMoved && !hammerController.hammerRotated && !drillController.drillMoved)
            {
                if (hit.collider.GetComponent<Fence>())
                {
                    currentFence = hit.collider.gameObject.GetComponent<Fence>();
                    StartCoroutine(hammerController.HammerMoved());
                    if (currentFence)
                    {
                        StartCoroutine(hammerController.HammerRotated());
                    }
                }
                
                if (hit.collider.GetComponent<Nail>())
                {
                    currentNail = hit.collider.GetComponent<Nail>();
                    drillController.ResetPosition();
                    StartCoroutine(hammerController.HammerMoved());
                    if (currentNail)
                    {
                        StartCoroutine(hammerController.HammerRotated());
                    }
                }
                if (hit.collider.GetComponent<Screw>())
                {
                    currentScrew = hit.collider.GetComponent<Screw>();
                    hammerController.ResetPosition();
                    StartCoroutine(drillController.DrillMoved());
                }
            }
        }
        
        foreach (Fence fence in FirstFences.ToList())
        {
            if (fence.finish)
            {
                FirstFences.Remove(fence);
            }
        }

        foreach (Fence fence in SecondFences.ToList())
        {
            if (fence.finish)
            {
                SecondFences.Remove(fence);
            }
        }

        if (FirstFences.Count<=0)
        {
            foreach (Nail nail in FirstNails)
            {
                nail.gameObject.SetActive(true);
            }

            foreach (Screw screw in FirstScrews)
            {
                screw.gameObject.SetActive(true);
            }
        }
        
        foreach (Nail nail in FirstNails.ToList())
        {
            if (nail.finish)
            {
                FirstNails.Remove(nail);
            }
        }
        
        foreach (Screw screw in FirstScrews.ToList())
        {
            if (screw.finish)
            {
                FirstScrews.Remove(screw);
            }
        }
        foreach (Nail nail in SecondNails.ToList())
        {
            if (nail.finish)
            {
                SecondNails.Remove(nail);
            }
        }
        
        foreach (Screw screw in SecondScrews.ToList())
        {
            if (screw.finish)
            {
                SecondScrews.Remove(screw);
            }
        }
        
        if (FirstNails.Count<=0 && FirstScrews.Count <=0)
        {
            MoveToCameraSecondPosition();
            foreach (Fence secondFence in SecondFences)
            {
                secondFence.GetComponent<Collider>().enabled = true;
            }
        }

        if (SecondFences.Count<=0)
        {
            foreach (Nail nail in SecondNails)
            {
                nail.gameObject.SetActive(true);
            }

            foreach (Screw screw in SecondScrews)
            {
                screw.gameObject.SetActive(true);
            }
            foreach (Nail secondNail in SecondNails)
            {
                secondNail.GetComponent<Collider>().enabled = true;
            }
            foreach (Screw secondScrew in SecondScrews)
            {
                secondScrew.GetComponent<Collider>().enabled = true;
            }
        }

        if (SecondNails.Count <=0 && SecondScrews.Count <=0)
        {
            GameOverPanel.SetActive(true);
        }
    }
    
    private void MoveToCameraSecondPosition()
    {
        _mainCamera.transform.SetParent(CameraSecondPos);
        _mainCamera.transform.localPosition = Vector3.Lerp(_mainCamera.transform.localPosition,Vector3.zero,Time.deltaTime);
        _mainCamera.transform.localRotation = Quaternion.Lerp(_mainCamera.transform.localRotation,Quaternion.identity, Time.deltaTime); 
    }

    private void RetryButton()
    {
        SceneManager.LoadScene(0);
    }

    public void Retry()
    {
        Invoke("RetryButton",1f);
    }
}
