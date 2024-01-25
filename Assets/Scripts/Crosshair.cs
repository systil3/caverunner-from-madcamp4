using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{

    public float crosshairSpeed; // Crosshair 이동 속도
    public Vector3 mousePosition;

    void Update()
    {
        // 마우스의 화면 기준 좌표를 감지
        mousePosition = Input.mousePosition;

        // 마우스 좌표를 월드 좌표로 변환
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldMousePosition.z = 0f; // 2D 공간에서 z 값은 0으로 설정

        // Crosshair를 마우스 위치로 부드럽게 이동
        transform.position = Vector3.Lerp(transform.position, worldMousePosition, Time.deltaTime * crosshairSpeed);
    }

}