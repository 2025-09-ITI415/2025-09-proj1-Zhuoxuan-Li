using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    public Transform target; public Vector3 offset = new Vector3(0, 8, -10); public float lerp = 10f;
    void LateUpdate() { if (!target) return; transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * lerp); transform.LookAt(target.position); }
}
