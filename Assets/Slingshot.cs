using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject projectilePrefab;
    public float velocityMult = 10f;


    [Header("Dynamic")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    void Update()
    {
        if (!aimingMode)
        {
            return;
        }

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - launchPos;
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0))
        // remark: NEVER use GetMouseButtonXXX() in FixedUpdate(). Since FixedUpdate() runs every 1/50th of a second
        // and not on each and every frame, it is quite likely that whatever change in a button's state you are trying
        // to observe did NOT occur in coincidence with the FixedUpdate. IF for whatever reason we do need to do something
        // in response to a button press in a fixed update, then we should set a boolean in Update, and if it is true in
        // FixedUpdate, do whatever we need to do and then flip the boolean.

        // it is noted that Other Input methods like Input.GetAxis(), Input.GetKey(), and Input. GetButton() that do not end
        // in …Up() or …Down() work perfectly well inside either FixedUpdate() or Update() because they respond with the current
        // state of the axis, key, or button, which is completely valid to check in a FixedUpdate.

        // the key difference being highlighted here is a difference in state-based polling (i.e. what is the state of this button
        // right now) vs edge-triggered polling, which indicate whether a change ocurred *this frame*; the latter category would
        // be inappropriate to use in FixedUpdate().

        // Standard practice: get/handle inputs in Update, handle simulation in FixedUpdate()
        {
            aimingMode = false;
            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            projRB.isKinematic = false;
            projRB.collisionDetectionMode = CollisionDetectionMode.Continuous; // remark: there are at least 3 other collision modes
                                                                               // and it is probably worth knowing a bit about each of them!
            projRB.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
        }
    }

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
        launchPos = launchPointTrans.position;
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

    void OnMouseDown()
    {
        aimingMode = true;
        projectile = Instantiate(projectilePrefab) as GameObject;
        projectile.transform.position = launchPos;
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }
}
