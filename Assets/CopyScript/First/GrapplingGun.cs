using Cinemachine;
using UnityEngine;

public class GrapplingGun : MonoBehaviour {
    
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    
    public CinemachineVirtualCamera camera;
    public CharacterMovement cm;
    public Transform gunTip;
    private SpringJoint joint;
    public CharacterInput inputC;
    bool isGrappling;
    
    private float maxDistance = 100f;

    [Header("Cooldown")]
    public float grapplingCd = 1f;
    private float grapplingCdTimer;

    private void Awake()
    {
        grapplingCdTimer = grapplingCd;
    }
    void Update() 
    {
        if (inputC.wire && !isGrappling) {
            StartGrapple();
        }

        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;
    }



    void StartGrapple() 
    {
        inputC.wire = false;
        isGrappling = true;
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            joint = cm.gameObject.GetComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(cm.transform.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.3f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            joint.spring = 90f;
            joint.damper = 2f;
            joint.massScale = 100f;
        }
        Invoke(nameof(StopGrapple), 3f);
    }


    void StopGrapple() 
    {
        grapplingCdTimer = grapplingCd;
        isGrappling = false;
    }



    public bool IsGrappling() {
        return isGrappling;
    }

    public Vector3 GetGrapplePoint() {
        return grapplePoint;
    }
}
