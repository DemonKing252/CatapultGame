using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultController : MonoBehaviour
{
    [SerializeField] private LineRenderer projectileLine;
    //[SerializeField] private Vector2 impulse = new Vector2(5f, 5f);
    [SerializeField] private Rigidbody2D ballRigidBody;
    private Vector3 originalPosition;
    [SerializeField] private int sliceCount;
    private bool ballHeld = false;
    [SerializeField] Transform catapultOrgin;
    [SerializeField] private float impulseFactor = 2f;
    [SerializeField] private LineRenderer ropeLine;
    [SerializeField] private LineRenderer line1;
    [SerializeField] private LineRenderer line2;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }
    private void Awake()
    {
        BallController.Instance.onBallStateChanged += OnBallStateChanged;
    }
    private void OnDestroy()
    {
        BallController.Instance.onBallStateChanged -= OnBallStateChanged;
    }

    public void OnBallStateChanged(BallState state)
    {
        Debug.Log("Ball state: " + state.ToString());

        ballHeld = state switch
        {
            BallState.Held      => true,
            BallState.Released => false,
            _ => throw new System.Exception("Unknown ball state")
        };
        if (state == BallState.Released)
        {
            Vector2 impulse = (catapultOrgin.position - ballRigidBody.transform.position)/*.normalized*/ * impulseFactor;
            
            this.CalculateProjectilePath(impulse);
            this.LaunchBall(impulse);

            ropeLine.enabled = false;
            line1.enabled = false;
            line2.enabled = false;
        }
    }

    public void CalculateProjectilePath(Vector3 impulse)
    {
        projectileLine.positionCount = sliceCount;
        for (int i = 0; i < projectileLine.positionCount; i++)
        {
            // Newtons Kinematic equation for finding Displacement, given intial velocity, acceleration, and time.
            // dx = vi * t      <---- ax is 0, so the right side cancels out.
            // dy = vi * t + 1/2 * a * t^2

            float t = 0.1f * (float)i;


            float offsetX = impulse.x * t;
            float offsetY = impulse.y * t + 0.5f * Physics2D.gravity.y * Mathf.Pow(t, 2);

            projectileLine.SetPosition(i, ballRigidBody.transform.position + new Vector3(offsetX, offsetY));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ballHeld)
        {
            Vector2 impulse = (catapultOrgin.position - ballRigidBody.transform.position)/*.normalized*/ * impulseFactor;
            this.CalculateProjectilePath(impulse);
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            ballRigidBody.transform.position = mousePosition;
        }
    }

    private void LaunchBall(Vector3 impulse)
    {
        ballRigidBody.gravityScale = 1f;
        ballRigidBody.velocity = impulse;
    }
}
