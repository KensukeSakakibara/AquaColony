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
		private Vector2 keyPosition;

		// Use this for initialization
		void Start ()
		{
			this.keyPosition = new Vector2();
		}
		
		// Update is called once per frame
		void Update ()
		{
			// 初期化
			this.keyPosition = Vector2.zero;

			// キー入力を調べる（FixedUpdateで行ってはいけない）
			if (Input.GetKey(KeyCode.W)){
				this.keyPosition.x += 1;
			}
			if (Input.GetKey(KeyCode.S)){
				this.keyPosition.x -= 1;
			}
			if (Input.GetKey(KeyCode.A)){
				this.keyPosition.y += 1;
			}
			if (Input.GetKey(KeyCode.D)){
				this.keyPosition.y -= 1;
			}
		}

		void FixedUpdate()
		{
			// キャラ操作
			this.moveCharacter();
		}

		// キーボードを押した際の進行方向を取得する
		private void moveCharacter()
		{
			// 操作されている場合
			if (Vector2.zero != this.keyPosition) {
				// キー入力の角度を調べる
				float keyRadian = GetAim(Vector2.zero, this.keyPosition);

				// カメラの角度を取得する
				float cameraRadian = this.camera.GetComponent<ACCameraController>().getCameraRadian();

				// カメラの角度とキー入力から進行方向を算出する
				Vector2 movePosition = this.GetPosition(keyRadian + cameraRadian, 1.0f);

				// 力を加えて進ませる
				movePosition *= moveSpeed;
				this.GetComponent<Rigidbody>().velocity = new Vector3(movePosition.x, 0.0f, movePosition.y);

				// キャラクターをカメラの方向にセットする（ターゲットのY座標を自分と同じにすることで2次元に制限する）
				Vector3 targetPosition = this.camera.transform.position;
				targetPosition.y = this.transform.position.y;
				this.transform.LookAt(targetPosition);

			} else {
				// 止める
				this.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, this.GetComponent<Rigidbody>().velocity.y, 0.0f);
			}
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
