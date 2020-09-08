using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetCameraSpeed : MonoBehaviour
{
    CameraController cameraController;

    public Slider cameraSpeedSlider;
    public Slider zoomSpeedSlider;

    public TextMeshProUGUI cameraSpeedCounter;
    public TextMeshProUGUI zoomSpeedCounter;

    private void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();

        if (PlayerPrefs.GetFloat("CameraSpeed") == 0)
        {
            PlayerPrefs.SetFloat("CameraSpeed", 50);
        }
        if (PlayerPrefs.GetFloat("ZoomSpeed") == 0)
        {
            PlayerPrefs.SetFloat("ZoomSpeed", 50);
        }
        cameraSpeedCounter.text = "CAMERA SPEED " + string.Format("{0:0.00}", PlayerPrefs.GetFloat("CameraSpeed")) + " %";
        cameraSpeedSlider.value = PlayerPrefs.GetFloat("CameraSpeed") / 100f;

        zoomSpeedCounter.text = "ZOOM SPEED " + PlayerPrefs.GetFloat("CameraSpeed") + " %";
        zoomSpeedSlider.value = PlayerPrefs.GetFloat("ZoomSpeed") / 100f;
        if (cameraController)
        {
            cameraController.panSpeed = PlayerPrefs.GetFloat("CameraSpeed");

            cameraController.zoomSpeed = PlayerPrefs.GetFloat("ZoomSpeed");
        }
    }

    public void SetPanSpeed(float speed)
    {
        float _speed = speed * 100;
        PlayerPrefs.SetFloat("CameraSpeed", _speed);
        cameraSpeedCounter.text = "CAMERA SPEED " + string.Format("{0:0.00}", _speed) + " %";
        if (cameraController)
            cameraController.panSpeed = PlayerPrefs.GetFloat("CameraSpeed");
    }

    public void SetZoomSpeed(float speed)
    {
        float _speed = speed * 100;
        PlayerPrefs.SetFloat("ZoomSpeed", _speed);
        zoomSpeedCounter.text = "ZOOM SPEED " + string.Format("{0:0.00}", _speed) + " %";
        if (cameraController)
            cameraController.zoomSpeed = PlayerPrefs.GetFloat("ZoomSpeed");
    }
}
