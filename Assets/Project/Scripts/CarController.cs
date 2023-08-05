using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Engine")]
    [SerializeField] int moveSpeed = 50; //acceseleration
    [SerializeField] int maxSpeed = 15;
    [SerializeField] float Drag = 0.98f;
    [SerializeField] int steerAngle = 20;
    [SerializeField] int wheelRotAngle = 20;
    [SerializeField] float traction = 0.1f; //drift

    [Header("Wheel")]
    [SerializeField] Transform frontWheelL;
    [SerializeField] Transform frontWheelR;

    [Header("Skid Mark")]
    [SerializeField] TrailRenderer trailRendererL;
    [SerializeField] TrailRenderer trailRendererR;
    

    Vector3 moveForce;

    private void Update()
    {
        //moving
        moveForce += transform.forward * moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        transform.position += moveForce * Time.deltaTime;

        //steering
        float steerInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerInput * moveForce.magnitude * steerAngle * Time.deltaTime);

        //front wheel
        float _steer = Mathf.Lerp(0, wheelRotAngle, Mathf.Abs(steerInput)) * -Mathf.Sign(steerInput);
        frontWheelL.localRotation = Quaternion.Euler(Vector3.up * -_steer);
        frontWheelR.localRotation = Quaternion.Euler(Vector3.up * -_steer);

        //drag and max speed limit
        moveForce *= Drag;
        moveForce = Vector3.ClampMagnitude(moveForce, maxSpeed);

        //drifting
        moveForce = Vector3.Lerp(moveForce.normalized, transform.forward, traction * Time.deltaTime) * moveForce.magnitude;

        //skid mark
        trailRendererR.emitting = steerInput != 0;
        trailRendererL.emitting = steerInput != 0;
    }
}
