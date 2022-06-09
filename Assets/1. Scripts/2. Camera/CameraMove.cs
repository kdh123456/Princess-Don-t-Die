using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Header("카메라 기본속성")]
    [SerializeField]
    private GameObject objTarget = null;
    private Transform objTargetTransform = null;
    private Transform cameraTransform = null;

    //카메라 기본타입 3인칭
    private CameraTypeState cameraState = CameraTypeState.Third;
    [Header("1인칭 카메라")]
    //마우스 카메라 조절 좌표
    [SerializeField]
    private float detailx = 5f;
    [SerializeField]
    private float detaily = 5f;

    //마우스 회전값
    [SerializeField]
    private float rotationX = 0f;
    [SerializeField]
    private float rotationY = 0f;

    //캐릭터에 카메라 눈 장착 포인트
    [SerializeField]
    private Transform posFirstCameraTarget = null;

    [Header("2인칭 카메라")]
    [SerializeField]
    private float rotationSpd = 10f;

    [Header("3인칭 카메라")]
    //떨어진 거리
    [SerializeField]
    private float distance = 6.0f;

    //추가된 높이
    [SerializeField]
    private float height = 1.75f;

    //높이 뎀프를 만들기 위해
    [SerializeField]
    private float heightDamp = 2.0f;

    //거리 뎀프를 만들기 위해
    [SerializeField]
    private float rotateDamp = 3.0f;
	private void Start()
	{
        cameraTransform = GetComponent<Transform>();
	}

	private void LateUpdate()
	{
        objTargetTransform = objTarget.transform;

        switch (cameraState)
		{
            case CameraTypeState.First:
                FirstCamera();
                break;
            case CameraTypeState.Second:
                SecondCamera();
                break;
            case CameraTypeState.Third:
                ThirdCamera();
                break;
		}
	}
	void FirstCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        rotationX = cameraTransform.localEulerAngles.y + mouseX * detailx;
        rotationX = (rotationX > 180.0f) ? rotationX - 360.0f : rotationX;

        rotationY = rotationY + mouseY * detaily;
        rotationY = (rotationY > 180f) ? rotationY - 360.0f : rotationY;

        cameraTransform.localEulerAngles = new Vector3(-rotationY, rotationX, 0f);
        cameraTransform.position = posFirstCameraTarget.position;
    }

    /// <summary>
    /// 2인칭 카메라
    /// </summary>
    void SecondCamera()
    {
        cameraTransform.RotateAround(objTargetTransform.transform.position, Vector3.up, rotationSpd * Time.deltaTime);

        cameraTransform.LookAt(objTargetTransform);
    }

    /// <summary>
    /// 3인칭 카메라
    /// </summary>
    void ThirdCamera()
    {
        float objTargetHeight = objTargetTransform.position.y + height;
        float objTargetAngle = objTargetTransform.eulerAngles.y;

        float nowAngle = cameraTransform.eulerAngles.y;
        float nowHeigh = cameraTransform.position.y;

        nowAngle = Mathf.LerpAngle(nowAngle, objTargetAngle, 4f * Time.deltaTime);

        nowHeigh = Mathf.Lerp(nowHeigh, objTargetHeight, 5f * Time.deltaTime);

        Quaternion nowRotation = Quaternion.Euler(0f, nowAngle, 0f);

        cameraTransform.position = objTargetTransform.position;
        cameraTransform.position -= nowRotation * Vector3.forward * 5f;

        cameraTransform.position = new Vector3(cameraTransform.position.x, nowHeigh, cameraTransform.position.z);

        cameraTransform.LookAt(objTargetTransform);
    }
}
