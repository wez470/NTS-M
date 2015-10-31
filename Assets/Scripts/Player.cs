using UnityEngine;
using XboxCtrlrInput;
using System.Collections;

public class Player : MonoBehaviour {
	private const float ROT_DEAD_ZONE = 0.2f;

	public float Speed;
	public float RotSpeed;
	public int PlayerNum;
	
	public void SetPlayerNum(int playerNum) {
		PlayerNum = playerNum;    
	}

	void Update() {
		setRotation();
		setMovement();
	}

	private void setMovement() {
		float speedX = XCI.GetAxis(XboxAxis.LeftStickX, PlayerNum) * Speed;
		float speedY = XCI.GetAxis(XboxAxis.LeftStickY, PlayerNum) * Speed;
		GetComponent<Rigidbody2D>().AddForce(new Vector2(speedX, speedY));
	}

	private void setRotation() {
		float rotX = XCI.GetAxis(XboxAxis.RightStickX, PlayerNum);
		float rotY = -XCI.GetAxis(XboxAxis.RightStickY, PlayerNum);
		float desiredAngle = Mathf.Atan2(-rotY, rotX) * Mathf.Rad2Deg - 90f;
		float currAngle = transform.eulerAngles.z;
		
		float nextAngle = getNextAngle(desiredAngle, currAngle);
		
		if(Mathf.Abs(rotX) > ROT_DEAD_ZONE || Mathf.Abs(rotY) > ROT_DEAD_ZONE) {
			transform.rotation = Quaternion.Euler(0, 0, nextAngle);
			GetComponent<Rigidbody2D>().angularVelocity = 0;
		}
	}
	
	private float getNextAngle(float desiredAngle, float currAngle) {
		/* Get the shortest angle between the current angle we are facing and the angle
		   we desire to be facing.  The sign of the angle between tells us which is the
		   shortest way to rotate to get to the desired angle */
		float angleBetween = Mathf.Atan2(Mathf.Sin((desiredAngle - currAngle) * Mathf.Deg2Rad),
		                                 Mathf.Cos((desiredAngle - currAngle) * Mathf.Deg2Rad)) * Mathf.Rad2Deg;
		float nextAngle = currAngle;
		
		if(Mathf.Abs(angleBetween) < RotSpeed) {
			nextAngle = desiredAngle;
		}
		else if(angleBetween < 0) {
			nextAngle -= RotSpeed;
		}
		else {
			nextAngle += RotSpeed;
		}
		
		return nextAngle;
	}

}
