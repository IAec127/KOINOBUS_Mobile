using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviourPunCallbacks
{

    protected PlayerMove playerMove = null;

    // 1�t���[���ł̃W���C�����͂̕ω���
    [SerializeField]
    protected Vector3 gyro;
    public Vector3 Gyro 
    { 
        get { return gyro; } 
        protected set {  gyro = value; } 
    }

    // ���݂̃W���C���̊p�x(-90.0�`90.0)
    [SerializeField]
    protected Vector3 totalGyro;
    public Vector3 TotalGyro
    {
        get { return totalGyro; }
        protected set { totalGyro = value; }
    }


    // ���[�s�b�`���[�����ꂼ��̓��͊��x
    private Vector3 yaw_pitch_role_sensitivity;

    public Vector3 Yaw_Pitch_Role_Sensitivity
	{
        get { return yaw_pitch_role_sensitivity; }
        protected set { yaw_pitch_role_sensitivity = value; }
    }


    // Start is called before the first frame update
    public void Start()
    {
        //if(photonView.IsMine)
        {
            playerMove = GetComponent<PlayerMove>();
        }
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    /// <summary>
    /// �W���C�������Z�b�g����֐�
    /// </summary>
    public virtual void ResetGyro() { }

    public float CheckCanBoost(float speed)
    {
        float ratio = playerMove.TotalBoostPower / playerMove.Data.TotalBoost;
        if (playerMove.TotalBoostPower >= speed)
        {
            // �X�s�[�h���c��u�[�X�g�ʂ����炷
            playerMove.TotalBoostPower -= speed;
            return speed;
        }
        // �X�s�[�h���̃u�[�X�g�ʂ��Ȃ��Ȃ炠�镪��������
        else
        {
            //float s = playerMove.TotalBoostPower;
            //playerMove.TotalBoostPower -= s;
            // return s;
            return 0.0f;
        }
    }
}
