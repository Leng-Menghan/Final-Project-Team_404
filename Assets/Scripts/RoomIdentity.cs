using UnityEngine;

public class RoomIdentity : MonoBehaviour
{
    [Header("Configuration")]
    public bool isAnomaly; // CHECK this box if room is weird!

    public void CheckForward()
    {
        // If room is Normal, Forward is CORRECT
        if (isAnomaly == false) LoopManager.Instance.RegisterWin();
        else LoopManager.Instance.RegisterLoss();
    }

    public void CheckBackward()
    {
        // If room is Anomaly, Backward is CORRECT
        if (isAnomaly == true) LoopManager.Instance.RegisterWin();
        else LoopManager.Instance.RegisterLoss();
    }
}