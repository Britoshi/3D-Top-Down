using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	public float Speed = 4;
	public float TurnSpeed = 1;
	public Camera mCam;


	protected Vector3 mLeftVec = new Vector3(-1, 0, 0);
	protected Vector3 mFwdVec = new Vector3(0, 0, 1);
	protected Vector3 mGOFwdVec = new Vector3(0, 0, 1);
	protected Animator mAnim;
	protected float mTimePassed = 0;
	protected int mMove = Animator.StringToHash("move");
	protected int mSpeed = Animator.StringToHash("speed");

	protected bool mMecFPActive = true;
	protected GUIStyle mStyle;

	// Use this for initialization
	void Start () 
	{
		mStyle = new GUIStyle();
		mAnim = GetComponent<Animator>();
		mStyle.normal.textColor = new Color(0.5f, 0, 0.75f, 1);
		mStyle.fontSize = 20;
	}

	//GUI callback
	void OnGUI()
	{
		GUI.Label(new Rect(0, 0, 100, 100), "Use W, A, S, D to move around\nPress R to activate/deactivate Mec Foot Placer\nHold L to run", mStyle);

        /*if (GetComponent<MecFootPlacer> ().mAdjustPelvisVertically) {
			GUI.Label (new Rect (0, 0, 100, 100), "Pelvis Adjustment Is On", mStyle);
		}
		else
		{
			GUI.Label (new Rect (0, 0, 100, 100), "Pelvis Adjustment Is off", mStyle);
		}*/
	}

	// Update is called once per frame
	void Update ()
	{
		mAnim.SetFloat("time", mAnim.GetCurrentAnimatorStateInfo(0).normalizedTime - Mathf.Floor(mAnim.GetCurrentAnimatorStateInfo(0).normalizedTime));

		if(!Input.anyKey)
		{
			float lSpeed = mAnim.GetFloat(mSpeed);

			lSpeed -= 1.5f * Time.deltaTime;
			if(lSpeed < 0)
			{
				lSpeed = 0;
				mAnim.SetBool(mMove, false);
			}
			mAnim.SetFloat(mSpeed, lSpeed);
			return;
		}
		else
		{
			bool lMove = false;
			Vector3 lCurrentFwdVec = mCam.transform.rotation * mFwdVec;
			lCurrentFwdVec.y = 0;

			Vector3 lCurrentLeftVec = mCam.transform.rotation * mLeftVec;
			lCurrentLeftVec.y = 0;

			mGOFwdVec = gameObject.transform.rotation * new Vector3(0, 0, 1);

			//
			Vector3 lFinalVec = new Vector3(0, 0, 0);
			float lSign = 1;
			float lDir = 0;

			if(Input.GetKey(KeyCode.A))
			{
				lFinalVec += lCurrentLeftVec;
				lMove = true;
			}

			if(Input.GetKey(KeyCode.D))
			{
				lFinalVec -= lCurrentLeftVec;
				lMove = true;
			}

			if(Input.GetKey(KeyCode.W))
			{
				lFinalVec += lCurrentFwdVec;
				lMove = true;
			}

			if(Input.GetKey(KeyCode.S))
			{
				lFinalVec -= lCurrentFwdVec;
				lMove = true;
			}

			if(Input.GetKey(KeyCode.L))
			{
				float lSpeed = mAnim.GetFloat(mSpeed);

				if(lMove)
				{
					lSpeed += Time.deltaTime;
					if(lSpeed > 1)
					{
						lSpeed = 1;
					}
				}
				else
				{
					lSpeed -= 2 * Time.deltaTime;

					if(lSpeed < 0)
					{
						lSpeed = 0;
					}
				}

				mAnim.SetFloat(mSpeed, lSpeed);
			}
			else
			{
				float lSpeed = mAnim.GetFloat(mSpeed);
				lSpeed -= Time.deltaTime;

				if(lSpeed < 0)
				{
					lSpeed = 0;
				}
				mAnim.SetFloat(mSpeed, lSpeed);
			}

			if(Input.GetKey(KeyCode.PageDown))
			{
				if(mTimePassed > 0.1f)
				{
					Time.timeScale -= 0.1f;
					if(Time.timeScale < 0)
					{
						Time.timeScale = 0;
					}
					mTimePassed = 0;
					Debug.Log(Time.timeScale);
				}
			}

			if(Input.GetKey(KeyCode.PageUp))
			{
				if(mTimePassed > 0.1f)
				{
					Time.timeScale += 0.1f;
					mTimePassed = 0;
					Debug.Log(Time.timeScale);
				}
			}

			if(Input.GetKeyDown(KeyCode.R))
			{
				mMecFPActive = !mMecFPActive;
				GetComponent<MecFootPlacer>().SetActive(AvatarIKGoal.LeftFoot, mMecFPActive);
				GetComponent<MecFootPlacer>().SetActive(AvatarIKGoal.RightFoot, mMecFPActive);
				//GetComponent<MecFootPlacer>().mAdjustPelvisVertically = mMecFPActive;
			}

			if(lMove)
			{
    
                lSign = Vector3.Cross(mGOFwdVec, lFinalVec).y;
                if(lSign !=0)
				{
					lSign = lSign/Mathf.Abs(lSign);
				}
                lDir = Vector3.Angle(mGOFwdVec, lFinalVec);

                lDir *= TurnSpeed * Time.deltaTime;
                gameObject.transform.Rotate(new Vector3(0, lSign, 0), lDir, Space.World);
				mAnim.SetBool(mMove, true);

			}
			else
			{
				if(mAnim.GetFloat("speed") <= 0)
				{
					mAnim.SetBool(mMove, false);
				}
			}
			mTimePassed += Time.deltaTime;
		}
	}
}
