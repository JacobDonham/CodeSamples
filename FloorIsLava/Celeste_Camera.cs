using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celeste_Camera : MonoBehaviour
{
    private Vector3 _currentPosition;
    private Vector3 _nextPosition;
    [SerializeField] private float _moveTime = 30f;
    [SerializeField] PlayerController _player;

    public void CameraMove(Vector3 cameraPos)
    {
        if(cameraPos == transform.position)
        {
            return;
        }
        else if(cameraPos != transform.position)
        {
            _player.SetCameraTransition(true);
            StartCoroutine(CameraCoroutine(cameraPos));
        }
    }

    private IEnumerator CameraCoroutine(Vector3 moveTo)
    {
        float temp = _moveTime;

        while(Vector3.Distance(transform.position, moveTo) > .01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTo, _moveTime * Time.deltaTime);

            yield return null;
        }

        _moveTime = temp;
        _player.SetCameraTransition(false);
        yield return null;
    }
}
