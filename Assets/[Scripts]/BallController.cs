using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallState
{
    Held,
    Released
}


public class BallController : MonoBehaviour
{
    public delegate void OnBallStateChanged(BallState state);
    public event OnBallStateChanged onBallStateChanged;


    private static BallController s_pInstance;
    private void Awake()
    {
        s_pInstance = this;
    }

    public static BallController Instance { get { return s_pInstance; } }

    private void OnMouseDown()
    {
        onBallStateChanged?.Invoke(BallState.Held);
    }
    private void OnMouseUp()
    {
        onBallStateChanged?.Invoke(BallState.Released);
    }
}
