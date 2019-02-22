using UnityEngine;

public class DestroyObjectAfterTime : MonoBehaviour
{
    public float m_DestroyAfterSeconds;

    void Start()
    {
        new EnumTimer(() => { Destroy(gameObject); }, m_DestroyAfterSeconds).StartTimeout();
    }


}
