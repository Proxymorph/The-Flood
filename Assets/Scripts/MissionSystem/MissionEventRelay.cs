using UnityEngine;
using UnityEngine.Events;

public class MissionEventRelay : MonoBehaviour
{
    public UnityEvent onMissionCompleteRelay;
    public UnityEvent onMissionFailRelay;

    public void RelayComplete()
    {
        onMissionCompleteRelay?.Invoke();
    }

    public void RelayFail()
    {
        onMissionFailRelay?.Invoke();
    }
}