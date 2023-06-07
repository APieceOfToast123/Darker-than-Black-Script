using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMove : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;
    private float duration;
    private float startTime;
    private GameObject movingObject;

    public void MoveToDestination(GameObject obj, Vector3 destination)
    {
        movingObject = obj;
        startPos = obj.transform.position;
        endPos = CalculateWorldPosition(destination);
        duration = Vector3.Distance(startPos, endPos) / 5f; 
        startTime = Time.time;
        print("MoveToDestination");
        StartCoroutine(MoveObject());
    }

    private IEnumerator MoveObject()
    {
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            movingObject.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        movingObject.transform.position = endPos; 
    }

    private Vector3 CalculateWorldPosition(Vector3 relativePosition)
    {
        Vector3 worldPosition = transform.TransformPoint(relativePosition);
        return worldPosition;
    }
}
