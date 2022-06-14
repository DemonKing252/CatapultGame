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


    [SerializeField] private GameObject replicateParentTransform;
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
            this.LaunchBall(impulse);

            ropeLine.enabled = false;
            line1.enabled = false;
            line2.enabled = false;
        }
    }

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

        // Replicate all objects in this scene.
        List<GameObject> replicatedObjects = new List<GameObject>();
        for (int i = 0; i < replicateParentTransform.transform.childCount; i++)
        {
            GameObject temp = Instantiate(replicateParentTransform.transform.GetChild(i).gameObject);

            temp.transform.position = replicateParentTransform.transform.GetChild(i).gameObject.transform.position;
            temp.transform.rotation = replicateParentTransform.transform.GetChild(i).gameObject.transform.rotation;
            temp.transform.localScale = replicateParentTransform.transform.GetChild(i).gameObject.transform.localScale;

            replicatedObjects.Add(temp);
            SceneManager.MoveGameObjectToScene(temp, _simulationScene);
        }
        


        int stepCount = 200;
        projectileLine.positionCount = stepCount;
        for (int i = 0; i < stepCount; i++)
        {
            _physicsSimulationScene.Simulate( Time.fixedDeltaTime );
            projectileLine.SetPosition(i, dummyObject.transform.position);
        }

        for (int i = 0; i < replicatedObjects.Count; i++)
        {
            Destroy(replicatedObjects[i]);
        }
        replicatedObjects.Clear();
        Destroy(dummyObject);
    }

    private void LaunchBall(Vector3 impulse)
    {
        ballRigidBody.gravityScale = 1f;
        ballRigidBody.velocity = impulse;
    }
}
