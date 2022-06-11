using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    private Scene _currentScene;
    private PhysicsScene2D _physicsCurrentScene;

    private Scene _simulationScene;
    private PhysicsScene2D _physicsSimulationScene;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;
        BallController.Instance.onBallStateChanged += OnBallStateChanged;
        originalPosition = transform.position;

        _currentScene = SceneManager.GetActiveScene();
        _physicsCurrentScene = _currentScene.GetPhysicsScene2D();

        _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        _physicsSimulationScene = _simulationScene.GetPhysicsScene2D();
    }
    private void Awake()
    {
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
            BallState.Released  => false,
            _ => throw new System.Exception("Unknown ball state")
        };
        if (state == BallState.Released)
        {
            Vector2 impulse = (catapultOrgin.position - ballRigidBody.transform.position)/*.normalized*/ * impulseFactor;
            this.PredictProjectilePath(impulse);
            //this.CalculateProjectilePath(impulse);
            this.LaunchBall(impulse);

            //ropeLine.enabled = false;
            line1.enabled = false;
            line2.enabled = false;
        }
    }

    //public void CalculateProjectilePath(Vector3 impulse)
    //{
    //    projectileLine.positionCount = sliceCount;
    //    for (int i = 0; i < projectileLine.positionCount; i++)
    //    {
    //        // Newtons Kinematic equation for finding Displacement, given intial velocity, acceleration, and time.
    //        // dx = vi * t      <---- ax is 0, so the right side cancels out.
    //        // dy = vi * t + 1/2 * a * t^2
    //
    //        float t = 0.1f * (float)i;
    //
    //
    //        float offsetX = impulse.x * t;
    //        float offsetY = impulse.y * t + 0.5f * Physics2D.gravity.y * Mathf.Pow(t, 2);
    //
    //        projectileLine.SetPosition(i, ballRigidBody.transform.position + new Vector3(offsetX, offsetY));
    //    }
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        _physicsCurrentScene.Simulate(Time.fixedDeltaTime);
        
        
        if (ballHeld)
        {
            Vector2 impulse = (catapultOrgin.position - ballRigidBody.transform.position)/*.normalized*/ * impulseFactor;
            this.PredictProjectilePath(impulse);

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            ballRigidBody.transform.position = mousePosition;
        }
    }
    private void PredictProjectilePath(Vector2 impulse)
    {
        GameObject dummyObject = Instantiate(ballRigidBody.gameObject);
        SceneManager.MoveGameObjectToScene(dummyObject, _simulationScene);

        dummyObject.transform.position = ballRigidBody.transform.position;
        dummyObject.GetComponent<Rigidbody2D>().velocity = impulse;
        dummyObject.GetComponent<Rigidbody2D>().gravityScale = 1f;


        int stepCount = 200;
        projectileLine.positionCount = stepCount;
        for (int i = 0; i < stepCount; i++)
        {
            _physicsSimulationScene.Simulate( Time.fixedDeltaTime );
            projectileLine.SetPosition(i, dummyObject.transform.position);
        }

        Destroy(dummyObject);
    }

    private void LaunchBall(Vector3 impulse)
    {
        ballRigidBody.gravityScale = 1f;
        ballRigidBody.velocity = impulse;
    }
}
