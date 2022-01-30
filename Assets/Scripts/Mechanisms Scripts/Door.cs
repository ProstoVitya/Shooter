using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{

    private bool _isOpen;
    private Transform _doorCube;

    private void Start()
    {
        _isOpen = false;
        _doorCube = transform.GetChild(0);
    }

    public void Open()
    {
        _isOpen = true;
        StartCoroutine(OpenAnimation());
    }

    public void Close()
    {
        _isOpen = false;
        StartCoroutine(CloseAnimation());
    }

    private IEnumerator OpenAnimation()
    {
        while (_isOpen && Mathf.Abs(_doorCube.transform.localPosition.y - 4.25f) > 0.01f)
        {
            _doorCube.localPosition += Vector3.up * Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator CloseAnimation()
    {
        while (!_isOpen && Mathf.Abs(_doorCube.transform.localPosition.y - 1.7f) > 0.01f)
        {
            _doorCube.localPosition += Vector3.down * Time.deltaTime;
            yield return null;
        }
    }
}
