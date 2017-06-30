using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AquaColony.Camera
{
	public class ACCameraController : MonoBehaviour
	{
		public float speedX = 0.5f;
		public float speedY = 0.2f;

		public GameObject focusObj;
		public GameObject ground;

		private int MOUSE_LEFT = 0;
		private int MOUSE_RIGHT = 1;
		private int MOUSE_MIDDLE = 2;

		// マウスクリックの位置保存用
		private Vector2 oldPosition;

		// フォーカスからカメラまでの距離
		private float distance = 10.0f;

		// カメラのラジアン角度
		private float cameraRadian = 0.0f;

		/**
		 * 現在のカメラのラジアン角を取得する
		 */
		public float getCameraRadian()
		{
			return this.cameraRadian;
		}

		// Use this for initialization
		void Start () {
			oldPosition = Vector2.zero;

			// カメラの角度をセットする
			Vector2 cammeraPositionV2 = new Vector2(this.transform.position.x, this.transform.position.z);
			Vector2 focusPositionV2 = new Vector2(this.focusObj.transform.position.x, this.focusObj.transform.position.z);
			this.cameraRadian = this.GetAim(cammeraPositionV2, focusPositionV2);
		}

		// Update is called once per frame
		void Update ()
		{
			// オブジェクトに対するカメラの位置調整
			this.setCammeraPosition();

			// 右クリック直後
			if (Input.GetMouseButtonDown(this.MOUSE_RIGHT)) {
				this.oldPosition = Input.mousePosition;
				this.mouseDrag();
			}

			// 右クリック中
			if (Input.GetMouseButton(this.MOUSE_RIGHT)) {
				this.mouseDrag();
			}
		}

		// オブジェクトに対するカメラの位置調整
		private void setCammeraPosition()
		{
			Vector3 focusPosition = focusObj.transform.position;

			// カメラに対するフォーカスの相対的な位置を取得
			Vector2 localFocusPosition = GetPosition(this.cameraRadian, this.distance);

			// カメラの位置をセットする
			this.transform.position = new Vector3(
				focusPosition.x - localFocusPosition.x,
				this.transform.position.y,
				focusPosition.z - localFocusPosition.y
			);
		}

		// ドラッグ中
		private void mouseDrag()
		{
			Vector2 currentPosition = Input.mousePosition;
			Vector2 diff = currentPosition - oldPosition;

			// 差分の長さが極少数より小さい場合はドラッグしていないことにする
			if (Vector3.kEpsilon < diff.magnitude) {
				this.cameraRotate(diff);

				// 現在のマウス位置を、次回のために保存する
				this.oldPosition = currentPosition;
			}
		}

		// カメラを回転させる
		private void cameraRotate(Vector2 diff)
		{
			// 地面を基準にしてカメラ回転
			Vector3 focusPosition = this.focusObj.transform.position;
			this.transform.RotateAround(focusPosition, this.ground.transform.up, this.speedX * diff.x);

			// 縦方向の回転はカメラのローカル座標系のX軸で回転する
			this.transform.RotateAround(focusPosition, this.transform.right, this.speedY * -diff.y);

			// カメラの角度をセットする
			Vector2 cammeraPositionV2 = new Vector2(this.transform.position.x, this.transform.position.z);
			Vector2 focusPositionV2 = new Vector2(focusPosition.x, focusPosition.z);
			this.cameraRadian = this.GetAim(cammeraPositionV2, focusPositionV2);
		}

		// 2点間のラジアン角度を求める
		private float GetAim(Vector2 p1, Vector2 p2)
		{
			float dx = p2.x - p1.x;
			float dy = p2.y - p1.y;
			float rad = Mathf.Atan2(dy, dx);
			return rad;
		}

		// ラジアン角と距離から座標を求める
		private Vector2 GetPosition(float cameraRadian, float radius)
		{
			float y2 = Mathf.Sin(cameraRadian) * radius;
			float x2 = Mathf.Cos(cameraRadian) * radius;
			return new Vector2 (x2, y2);
		}
	}
}
