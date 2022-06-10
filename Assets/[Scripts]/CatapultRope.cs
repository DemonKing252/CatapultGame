using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultRope : MonoBehaviour
{
    [SerializeField] private LineRenderer ropeLine1;
    [SerializeField] private LineRenderer ropeLine2;
    [SerializeField] Transform ballTransform;
    [SerializeField] Transform originTransformLine1;
    [SerializeField] Transform originTransformLine2;

    // Start is called before the first frame update
    void Start()
    {
        ropeLine1.SetPosition(0, originTransformLine1.position);
        ropeLine2.SetPosition(0, originTransformLine2.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ropeLine1.SetPosition(1, ballTransform.position);
        ropeLine2.SetPosition(1, ballTransform.position);

    }
}
