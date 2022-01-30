using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Button : MonoBehaviour
{
    public UnityEvent OnButtonStay;

    [Header("Optional")]
    public UnityEvent OnButtonLeave;

    private void OnTriggerEnter(Collider other)
    {
        transform.localPosition += new Vector3(0f, -0.1f, 0f);
        OnButtonStay.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (OnButtonLeave != null)
            OnButtonLeave.Invoke();
        transform.localPosition += new Vector3(0f, 0.1f, 0f);
    }
}
