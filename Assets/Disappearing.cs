using UnityEngine;

public class DisappearOnTouchCollision : MonoBehaviour
{
    public string playerTag = "Player"; // 玩家小球的Tag

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            gameObject.SetActive(false); // 或者：Destroy(gameObject);
        }
    }
}
