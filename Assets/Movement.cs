using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBallController : MonoBehaviour
{
    [Header("Movement")]
    public float moveForce = 18f;        
    public float airControl = 0.4f;      
    public float maxSpeed = 8f;          
    public bool cameraRelative = true;   

    [Header("Drag & Friction")]
    public float dragOnGround = 1.5f;    
    public float dragInAir = 0.1f;    
    public PhysicsMaterial groundPhysicMat;

    [Header("Jump (optional)")]
    public bool enableJump = true;
    public float jumpForce = 6.5f;      
    public float groundCheckRadius = 0.25f;
    public float groundCheckDistance = 0.55f;
    public LayerMask groundLayers = ~0;  

    Rigidbody rb;
    Camera cam;
    bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        rb.interpolation = RigidbodyInterpolation.Interpolate; 
        rb.maxAngularVelocity = 50f; 
        if (TryGetComponent<Collider>(out var col) && groundPhysicMat != null)
        {
            col.material = groundPhysicMat; 
        }
    }

    void FixedUpdate()
    {
        Vector3 origin = transform.position + Vector3.up * 0.05f;
        isGrounded = Physics.SphereCast(origin, groundCheckRadius, Vector3.down,
                                        out _, groundCheckDistance, groundLayers, QueryTriggerInteraction.Ignore);


        float h = Input.GetAxisRaw("Horizontal"); 
        float v = Input.GetAxisRaw("Vertical");   
        Vector3 input = new Vector3(h, 0f, v);
        input = Vector3.ClampMagnitude(input, 1f);


        Vector3 moveDir;
        if (cameraRelative && cam != null)
        {
            Vector3 camF = cam.transform.forward; camF.y = 0f; camF.Normalize();
            Vector3 camR = cam.transform.right; camR.y = 0f; camR.Normalize();
            moveDir = (camF * input.z + camR * input.x).normalized;
        }
        else
        {
            moveDir = input;
        }

        rb.linearDamping = isGrounded ? dragOnGround : dragInAir;
        float forceScale = isGrounded ? 1f : airControl;

        Vector3 force = moveDir * moveForce * forceScale;
        rb.AddForce(force, ForceMode.Force);

        Vector3 vel = rb.linearVelocity;
        Vector3 velXZ = new Vector3(vel.x, 0f, vel.z);
        if (velXZ.magnitude > maxSpeed)
        {
            velXZ = velXZ.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(velXZ.x, vel.y, velXZ.z);
        }


    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.green;
        Vector3 origin = Application.isPlaying ? transform.position + Vector3.up * 0.05f
                                               : transform.position + Vector3.up * 0.05f;
        Gizmos.DrawWireSphere(origin + Vector3.down * groundCheckDistance, groundCheckRadius);
    }
#endif
}

