using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AquaColony.Camera;

namespace AquaColony.Character
{
	public class ACCharacterController : MonoBehaviour
	{
		public GameObject camera;

		private float moveSpeed = 3.0f;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		void FixedUpdate()
		{
			// キャラ操作
			this.moveCharacter();
		}

		// キーボードを押した際の進行方向を取得する
		private void moveCharacter()
		{
			// キー入力を調べる
			Vector2 localPosition = new Vector2();
			if (Input.GetKey(KeyCode.W)){
				localPosition.x += 1;
			}
			if (Input.GetKey(KeyCode.S)){
				localPosition.x -= 1;
			}
			if (Input.GetKey(KeyCode.A)){
				localPosition.y += 1;
			}
			if (Input.GetKey(KeyCode.D)){
				localPosition.y -= 1;
			}

			// 何か押したのであれば
			if (Vector2.zero != localPosition) {
				// 角度を調べる
				float localRadian = GetAim(Vector2.zero, localPosition);

				// カメラの角度を取得する
				float cameraRadian = this.camera.GetComponent<ACCameraController>().getCameraRadian();

				// カメラの角度とキー入力から進行方向を算出する
				Vector2 position = GetPosition(localRadian + cameraRadian, 1.0f);

				// 力を加えて進ませる
				position *= moveSpeed;
				this.GetComponent<Rigidbody>().velocity = new Vector3(position.x, 0.0f, position.y);

				// キャラクターをカメラの方向にセットする
				Vector2 cameraPosition = this.GetPosition(cameraRadian, 1.0f);
				Quaternion q = Quaternion.LookRotation(new Vector3(cameraPosition.x, 0.0f, cameraPosition.y));
				Debug.Log (q);
				this.transform.rotation = q;

			} else {
				// 止める
				this.GetComponent<Rigidbody>().velocity = Vector3.zero;
			}

			// キャラクターはY軸以外では回転しません。
			Quaternion charRotation = this.GetComponent<Rigidbody>().rotation;
			this.transform.rotation = new Quaternion(0.0f, charRotation.y, 0.0f, 0.0f);
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
