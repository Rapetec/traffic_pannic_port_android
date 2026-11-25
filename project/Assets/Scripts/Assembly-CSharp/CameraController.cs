using System;
using System.Collections;
using System.Collections.Generic;
using NeonPlay;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public enum CameraId
	{
		CamDefault = 0,
		CamA = 1,
		CamB = 2,
		CamC = 3,
		CamD = 4,
		CamAWide = 5,
		CamBWide = 6,
		CamCWide = 7,
		CamDWide = 8,
		CamFireworks = 9,
		CamReallyWide = 10,
		CamGas = 11,
		CamMatrix = 12
	}

	private const int wideOffset = 4;

	public List<Camera> cameraList;

	public GameObject collisionCube;

	private CollisionCube collisionCubeScript;

	public GameManager gameManager;

	public Camera currentCamera;

	private bool ableToChange = true;

	private float matrixAngle;

	private Transform cachedCubeTrans;

	private void OnDestroy()
	{
		Time.timeScale = 1f;
		Time.fixedDeltaTime = 0.02f;
	}

	private void Start()
	{
		SingletonBase<CameraManager>.Instance.Controller = this;
		currentCamera = cameraList[0];
		for (int i = 1; i < cameraList.Count; i++)
		{
			cameraList[i].enabled = false;
		}
		cachedCubeTrans = collisionCube.transform;
		collisionCube.GetComponent<Renderer>().enabled = false;
		collisionCubeScript = collisionCube.GetComponent<CollisionCube>();
		SingletonBase<CameraManager>.Instance.Reset();
	}

	private void SelectCamera(bool force)
	{
		if (!gameManager.GameOver)
		{
			return;
		}
		int num = 0;
		float num2 = 0f;
		if (SingletonBase<CameraManager>.Instance.IsMatrixMode())
		{
			Camera camera = currentCamera;
			if (SingletonBase<CameraManager>.Instance.SetActiveCamera(12))
			{
				matrixAngle = camera.transform.eulerAngles.y;
			}
		}
		if (SingletonBase<CameraManager>.Instance.IsWideAngle())
		{
			num2 = 0.66f;
		}
		if (ableToChange || force)
		{
			if (SingletonBase<CameraManager>.Instance.IsFireworks())
			{
				StartCoroutine(WaitToChange(2f));
				SingletonBase<CameraManager>.Instance.SetActiveCamera(9);
			}
			else if (SingletonBase<CameraManager>.Instance.IsGasStation())
			{
				StartCoroutine(WaitToChange(2.5f));
				SingletonBase<CameraManager>.Instance.SetActiveCamera(11);
			}
			else if (collisionCubeScript.currentlyIn == "A")
			{
				StartCoroutine(WaitToChange(1.5f + num2));
				SingletonBase<CameraManager>.Instance.SetActiveCamera(1 + num);
				SnapCamera();
			}
			else if (collisionCubeScript.currentlyIn == "B")
			{
				StartCoroutine(WaitToChange(1.5f + num2));
				SingletonBase<CameraManager>.Instance.SetActiveCamera(2 + num);
				SnapCamera();
			}
			else if (collisionCubeScript.currentlyIn == "C")
			{
				StartCoroutine(WaitToChange(1.5f + num2));
				SingletonBase<CameraManager>.Instance.SetActiveCamera(3 + num);
				SnapCamera();
			}
			else if (collisionCubeScript.currentlyIn == "D")
			{
				StartCoroutine(WaitToChange(1.5f + num2));
				SingletonBase<CameraManager>.Instance.SetActiveCamera(4 + num);
				SnapCamera();
			}
			else
			{
				StartCoroutine(WaitToChange(0.75f));
				SingletonBase<CameraManager>.Instance.SetActiveCamera(0);
				SnapCamera();
			}
		}
	}

	private IEnumerator WaitToChange(float time)
	{
		ableToChange = false;
		yield return new WaitForSeconds(time);
		ableToChange = true;
	}

	private void Update()
	{
		if (!gameManager.GameOver)
		{
			return;
		}
		if (SingletonBase<CameraManager>.Instance.IsMatrixMode() && SingletonBase<CameraManager>.Instance.MatrixModeUpdate())
		{
			ableToChange = true;
		}
		if (ableToChange || SingletonBase<CameraManager>.Instance.IsMatrixMode())
		{
			SelectCamera(false);
		}
		if (SingletonBase<CameraManager>.Instance.IsMatrixMode())
		{
			GameObject gameObject = SingletonBase<CameraManager>.Instance.MatrixModeObject();
			matrixAngle += Time.deltaTime * 500f;
			Vector3 vector = new Vector3(Mathf.Sin(matrixAngle * ((float)Math.PI / 180f)) * 20f, 7f, Mathf.Cos(matrixAngle * ((float)Math.PI / 180f)) * 20f);
			Camera camera = cameraList[12];
			camera.transform.position = gameObject.transform.position + vector;
			camera.transform.LookAt(gameObject.transform.position);
		}
		if (currentCamera != cameraList[9] && currentCamera != cameraList[10] && currentCamera != cameraList[11])
		{
			cachedCubeTrans = collisionCube.transform;
			if (cachedCubeTrans.position != currentCamera.transform.position)
			{
				Quaternion to = Quaternion.LookRotation(cachedCubeTrans.position - currentCamera.transform.position);
				currentCamera.transform.rotation = Quaternion.RotateTowards(currentCamera.transform.rotation, to, 15f * Time.deltaTime);
			}
		}
		if (SingletonBase<CameraManager>.Instance.IsWideAngle())
		{
			cameraList[0].fieldOfView = Mathf.Lerp(cameraList[0].fieldOfView, 85f, Time.deltaTime);
		}
	}

	private void SnapCamera()
	{
		if (currentCamera != cameraList[9] && currentCamera != cameraList[10] && currentCamera != cameraList[11])
		{
			Quaternion rotation = Quaternion.LookRotation(cachedCubeTrans.position - currentCamera.transform.position);
			currentCamera.transform.rotation = rotation;
		}
	}
}
