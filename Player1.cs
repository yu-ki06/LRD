using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour {

	public static Player1 instance;
	//エフェクトを入れる配列
	public EffekseerEmitter[] effekseerEmitter;
	//アニメーション
	private Animator animator;
	//コリジョンを入れる配列
	public GameObject[] Colliders;
	//ダンゴムシの体のパーツを入れる配列
	public GameObject[] Legs;

	[SerializeField, Header("プレイヤーの初速")]
	private float MinSpeed;
    [SerializeField, Header("プレイヤーの最高速度")]
    private float MaxSpeed;
    [SerializeField, Header("プレイヤーのカーブスピード")]
	private float CurveSpeed;
	[SerializeField, Header("プレイヤーの番号")]
	public int PlayerNum;

	[Header("プレイヤーの入力インデックス")]
	private int m_playerNum;
	public LocalPlayer m_localPlayer;
	public float Speed;
    private float axis_x;
	private float axis_y;
    private bool button_z;
    private bool button_c;
    public string nextflag;
	public int flag_num;
    public bool button_cz;
    public int gamemove;
    private float starttime;

	// 傾きベクトルの調整値
	float devideNum = 5f;

    void Start()
	{
        nextflag = "flag1";
		flag_num = 1;
        gamemove = 0;
        button_cz = false;
		animator = GetComponent<Animator>();
        m_localPlayer = GetComponent<LocalPlayer>();
    }

	void Update()
	{
		if (gamemove == 0 || gamemove == 1) {
            switch (PlayerNum)
            {
			case 1:
				axis_x = Input.GetAxisRaw ("GamePad1_X");
				axis_y = Input.GetAxisRaw ("GamePad1_Y");
                axis_x = Input.GetAxisRaw("Horizontal");
                axis_y = Input.GetAxisRaw("Vertical");
				if (gamemove == 0) {
					button_z = Input.GetButtonDown ("Z_1") || Input.GetKeyDown ("z");
					button_c = Input.GetButtonDown ("C_1") || Input.GetKeyDown ("c");
				}
                    break;

			case 2:
				axis_x = Input.GetAxisRaw ("GamePad2_X");
				axis_y = Input.GetAxisRaw ("GamePad2_Y");
				if (gamemove == 0) {
					button_z = Input.GetButtonDown ("Z_2");
					button_c = Input.GetButtonDown ("C_2");
				}
                    break;

			case 3:
				axis_x = Input.GetAxisRaw ("GamePad3_X");
				axis_y = Input.GetAxisRaw ("GamePad3_Y");
				if (gamemove == 0) {
					button_z = Input.GetButtonDown ("Z_3");
					button_c = Input.GetButtonDown ("C_3");
				}
                    break;

			case 4:
				axis_x = Input.GetAxisRaw ("GamePad4_X");
				axis_y = Input.GetAxisRaw ("GamePad4_Y");
				if (gamemove == 0) {
					button_z = Input.GetButtonDown ("Z_4");
					button_c = Input.GetButtonDown ("C_4");
				}
                    break;
            }
        }
        else
        {
            axis_x = 0;
            axis_y = 0;
        }


//		axis_x = Input.GetAxisRaw("Horizontal");
//		axis_y = Input.GetAxisRaw("Vertical");

        if (gamemove == 0 || gamemove == 2)
        {
            if(gamemove == 0)
            {
                if (starttime <= 3)
                {
                    if (button_z && button_cz == false)
                    {
                        Speed += 2f;
						button_cz = true;
                    }

                    if (button_c && button_cz == true)
                    {
                        Speed += 2f;
                        button_cz = false;

                    }
                    starttime += Time.deltaTime;
					//スピードによって回転数が変わる
					Legs[3].transform.Rotate(Speed, 0, 0);
                }else
                {
                    gamemove = 1;
                }

            }
            axis_x = 0f;
            axis_y = 0f;
        }

        if (gamemove == 1)
        {
            if (axis_y > 0.1)
            {
                if (Speed < MaxSpeed)
                {
                    Speed += MinSpeed * axis_y / 40;//加速
                }
            }
            else if (axis_y < -0.1)
            {
                if (Speed >= 0)
                {
                    Speed += axis_y;//ブレーキ
                }
            }
            else
            {
                if (Speed > 0)
                {
                    Speed -= 0.3f;//自然に減速
                }
            }
        }
        if (transform.position.y < -20)
        {
            GameObject getflag = GameObject.Find("flag" + (flag_num - 1).ToString());
            Speed = 0;
            transform.position = new Vector3(getflag.transform.position.x, getflag.transform.position.y, getflag.transform.position.z);
        }
    }

	void FixedUpdate()
	{
        if (gamemove != 0) {
            Move();
        }
    }

	public void Move()
	{
		float v = Speed * Time.deltaTime;
		float h = axis_x * CurveSpeed * Time.deltaTime;

		transform.position += transform.forward * v + transform.right * h ;
		transform.Rotate(0, Mathf.DeltaAngle(transform.rotation.y, (transform.rotation.y + 20) - (Speed / 4)) * h, 0);
		//スピードによって回転数が変わる
		Legs[3].transform.Rotate((Speed / 2) * v, 0, 0);

		if (Speed >= 45) {
			animator.SetBool ("bool", true);
			Colliders[1].SetActive(true);
			Colliders[0].SetActive(false);
			Legs[0].SetActive (false);
            Legs[1].SetActive(false);
            if (Speed >= 50)
            {
                Legs[2].SetActive(false);
                Legs[3].SetActive(true);
            } 
        } else if(Speed <= 35){
            animator.SetBool("bool", false);
            Legs[2].SetActive(true);
            Legs[3].SetActive(false);
            Colliders[1].SetActive(false);
		    Colliders[0].SetActive (true);
			Legs[0].SetActive (true);
            Legs[1].SetActive(true);
        }

		if (Speed >= 50) {
			// 再生してるかチェックして再生してないなら再生させるようにする
			if (!effekseerEmitter[0].exists) 
			{
				effekseerEmitter[0].Play();
				Debug.Log ("バースト");
			}
		} else if(Speed <= 45){
			effekseerEmitter[0].Stop ();
		}

	}

	public void Goal()
	{
		gamemove = 2;
	}

	public void OnCollisionStay(Collision ef){
		if (ef.gameObject.tag == "Stage") 
		{ 
			if (Speed >= 16)
			{
				Debug.Log ("砂");
				if (!effekseerEmitter[1].exists)
				{
					Debug.Log ("砂");
					effekseerEmitter[1].Play ();
				}
			} else if (Speed <= 15) 
			{
				effekseerEmitter[1].Stop ();
			}

		}
	}

	public void OnCollisionExit(Collision ef)
	{
		if (ef.gameObject.tag == "Stage") {
			effekseerEmitter[1].Stop ();
		}
	}
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == nextflag)
        {
			GameObject Controller = GameObject.Find("GameController");
			switch (PlayerNum) {
			case 1:
				Controller.GetComponent<GameContoller> ().Dango1Input ();
				break;

			case 2:
				Controller.GetComponent<GameContoller> ().Dango2Input ();
				break;

			case 3:
				Controller.GetComponent<GameContoller> ().Dango3Input ();
				break;

			case 4:
				Controller.GetComponent<GameContoller> ().Dango4Input ();
				break;
			}

            flag_num++;
            if(flag_num > 7)
            {
                flag_num = 1;
            }
            nextflag = "flag" + flag_num.ToString();
        }
    }
}

