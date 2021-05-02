using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class visualJoyStick : MonoBehaviour
{

	private float Width, WidthHalf;
	private float Height, HeightHalf;
	private RectTransform RectTransform;
	[SerializeField] private Transform Stick, Block;
	[SerializeField] private UnityEngine.UI.Text OuptText;
	[SerializeField] private bool IsFloatJoystick;
	private bool IsTouch;
	public Vector2 joyStickVec;
	public Transform cam;
	Vector3 v3;
	public float speed;
	// Use this for initialization

	void Start()
	{
		joyStickVec = Vector2.zero;
		RectTransform = GetComponent<RectTransform>();
		Width = RectTransform.sizeDelta.x;
		Height = RectTransform.sizeDelta.y;
		WidthHalf = Width * 0.5f;
		HeightHalf = Height * 0.5f;
		IsTouch = false;
	}


	// Update is called once per frame
	void Update()
	{
		UpdateJoyStickVec();
		v3 = new Vector3(joyStickVec.x,0, joyStickVec.y);
		cam.Translate(speed * v3 * Time.deltaTime, Space.World);
		
	}

	void PrintJoyStickVec(Vector2 vec)
	{
		OuptText.text = vec.ToString("F2");
	}


	private void UpdateJoyStickVec()
	{
		//如果滑鼠為點擊或點擊到的介面不是此搖桿則跳出程式
		if (Input.GetMouseButton(0) == false)
		{
			RestJoyStick();
			IsTouch = false;
			return;
		}

		// 如果開啟漂浮搖桿功能時..
		if (IsFloatJoystick)
		{
			if (Input.GetMouseButtonDown(0)) transform.position = Input.mousePosition;
		}

		//取得點位置與搖桿距離
		float dist = Vector2.Distance(Input.mousePosition, transform.position);


		//如果滑鼠為點擊位置與搖桿間距離小於WidthHalof 則 Istouch= true;
		if (dist < WidthHalf)
		{
			IsTouch = true;
		}

		//如果點擊處為搖桿範圍內
		if (dist < WidthHalf)
		{
			Stick.position = Input.mousePosition;
			GetJoyStickVec();
			PrintJoyStickVec(joyStickVec);
			return;
		}

		//如果滑鼠拖曳出搖桿盤內
		if (IsTouch && dist > WidthHalf)
		{
			//如果拖拉滑鼠盤脫離搖桿盤的範圍，取得圓的交點
			GetIntersections();
			GetJoyStickVec();
			PrintJoyStickVec(joyStickVec);
			Block.transform.position = Input.mousePosition;
			return;
		}

	}

	private void GetIntersections()
	{
		Vector2 a = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		Vector2 b = new Vector2(transform.position.x, transform.position.y);
		Vector3 c = new Vector3(transform.position.x, transform.position.y, WidthHalf);
		Vector2 x = _getIntersections(a.x, a.y, b.x, b.y, c.x, c.y, c.z);
		Stick.transform.position = new Vector3(x.x, x.y, 0);
	}

	private void GetJoyStickVec()
	{
		joyStickVec = Stick.transform.position - this.transform.position;
		joyStickVec.x /= WidthHalf;
		joyStickVec.y /= HeightHalf;
	}

	private void RestJoyStick()
	{
		joyStickVec = Vector2.zero;
		Stick.transform.position = this.transform.position;
		PrintJoyStickVec(Vector3.zero);
	}



	//取得交點用
	Vector2 _getIntersections(float ax, float ay, float bx, float by, float cx, float cy, float cz)
	{
		float[] a = { ax, ay }, b = { bx, by }, c = { cx, cy, cz };
		// Calculate the euclidean distance between a & b
		float eDistAtoB = Mathf.Sqrt(Mathf.Pow(b[0] - a[0], 2) + Mathf.Pow(b[1] - a[1], 2));

		// compute the direction vector d from a to b
		float[] d = {
			(b[0] - a[0]) / eDistAtoB,
			(b[1] - a[1]) / eDistAtoB
		};

		// Now the line equation is x = dx*t + ax, y = dy*t + ay with 0 <= t <= 1.

		// compute the value t of the closest point to the circle center (cx, cy)
		var t = (d[0] * (c[0] - a[0])) + (d[1] * (c[1] - a[1]));

		// compute the coordinates of the point e on line and closest to c
		var ecoords0 = (t * d[0]) + a[0];
		var ecoords1 = (t * d[1]) + a[1];

		// Calculate the euclidean distance between c & e
		var eDistCtoE = Mathf.Sqrt(Mathf.Pow(ecoords0 - c[0], 2) + Mathf.Pow(ecoords1 - c[1], 2));

		// test if the line intersects the circle
		if (eDistCtoE < c[2])
		{
			// compute distance from t to circle intersection point
			var dt = Mathf.Sqrt(Mathf.Pow(c[2], 2) - Mathf.Pow(eDistCtoE, 2));

			// compute first intersection point
			var fcoords0 = ((t - dt) * d[0]) + a[0];
			var fcoords1 = ((t - dt) * d[1]) + a[1];
			// check if f lies on the line
			//        f.onLine = is_on (a, b, f.coords);

			// compute second intersection point
			// var gcoords0 = ((t + dt) * d[0]) + a[0];
			// var gcoords1 = ((t + dt) * d[1]) + a[1];
			Vector2 finalAnswer = new Vector2(fcoords0, fcoords1);

			// check if g lies on the line
			return (finalAnswer);

		}

		return Vector2.zero;

	}

	public void ToggleIsFloatJoystick()
	{
		IsFloatJoystick = !IsFloatJoystick;

	}

}
