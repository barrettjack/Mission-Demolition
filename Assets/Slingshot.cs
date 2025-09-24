using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public GameObject launchPoint;

    void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false); // "tells the game whether or not to ignore a particular GO."
                                      // more specifically:
                                      // - the GO will not be rendered
                                      // - will not "receive" calls to Update(), OnCollisionEnter(), etc.
                                      // - remark: the checkbox next to the GO name in the inspector is checked if active
                                      // - unchecked ow.
                                      // - in general, components (e.g. Renderer, Collider) will have such checkboxes too
                                      // - which we can play around with in our scripts.
    }
    void OnMouseEnter()
    {
        // print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        // print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }
}
