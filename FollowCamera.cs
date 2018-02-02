using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    [SerializeField, Header("カメラのスピード")]
    private float smoothing = 1;
    [SerializeField, Header("プレイヤー")]
    private Transform Target;
    [SerializeField, Header("カメラの初期位置")]
    private Transform initPos;

    private void Start()
    {
        transform.LookAt(Target, Vector3.up);
    }

    void FixedUpdate()
    {

        transform.position = Vector3.Lerp
            (transform.position, initPos.position, smoothing * Time.deltaTime);
        transform.rotation = Quaternion.Lerp
            (transform.rotation, FindObjectOfType<Player>().transform.rotation, smoothing * Time.deltaTime);

        transform.LookAt(Target, Vector3.up);

    }
}
