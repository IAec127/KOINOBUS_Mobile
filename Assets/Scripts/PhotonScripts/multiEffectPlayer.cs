using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
using Cinemachine;

public class multiEffectPlayer : MonoBehaviourPunCallbacks
{
    private static readonly Joycon.Button[] m_buttons =
    Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    List<Joycon> joyconList;
    private Joycon joyconL;     // ���̃W���C�R��
    private Joycon joyconR;     // �E�̃W���C�R��

    private Vector3 gyroR = Vector3.zero;
    private Vector3 gyroL = Vector3.zero;

    [SerializeField]
    private new Rigidbody rigidbody = null;
    public CinemachineVirtualCamera virtualCamera = null;

    [SerializeField]
    private Vector3 rotateOffset = Vector3.zero;    // �W���C���̌덷�𖳎�����I�t�Z�b�g

    [SerializeField]
    private float minSpeed = 5.0f;      // �Œᑬ�x
    [SerializeField]
    private float maxSpeed = 100.0f;    // �ō����x
    [SerializeField]
    private float speedRate = 0.01f;    // �X�s�[�h�{��
    [SerializeField]
    private float speed = 0.0f;         // ���݂̑��x

    [SerializeField]
    private Vector3 totalGyroR;     // �E�̃W���C���̌X���̍��v�l
    [SerializeField]
    private Vector3 totalGyroL;     // ���̃W���C���̌X���̍��v�l

    Vector2 startJoyLRotateY = Vector2.zero;      // ���W���C�R���̃X�^�[�g���̊p�x �����ӁI��������ɂ��x����0.0����1.0 y����0.0����-90.0�ɂȂ�܂�
    Vector2 startJoyRRotateY = Vector2.zero;      // �E�W���C�R���̃X�^�[�g���̊p�x


    [SerializeField]
    private float updownRate = 0.01f;       // �㏸�E���~�̔{��
    [SerializeField]
    private float leftlightRate = 0.01f;    // ���E�ړ��̔{��
    [SerializeField]
    private float rollRate = 0.01f;         // ���[����]�̔{��
    [SerializeField]
    private bool isRotate = false;          // ���̃t���[���ŉ�]���Ă��邩��\���t���O
    [SerializeField]
    private float brakePower = 1.0f;        // �u���[�L��
    bool brakeFlag = false;

    [SerializeField]
    private float rollControlTime = 1.0f;   // �p���𒼂��܂ł̎���
    private Coroutine rollControlCoroutine = null;

    [SerializeField]
    private float minFov = 60.0f;
    [SerializeField]
    private float maxFov = 100.0f;
    [SerializeField]
    private float addFov = 0.4f;
    [SerializeField]
    private float subFov = 0.2f;


    [SerializeField]
    private GameObject particleObject;

    [SerializeField]
    private ParticleSystem testParticle;

    // Start is called before the first frame update
    void Start()
    {
        // �W���C�R�����擾
        joyconList = JoyconManager.Instance.j;
        if (joyconList == null || joyconList.Count == 0)
        {
            Debug.Log("joycon��������܂���");
        }
        else
        {
            // ���E�̃W���C�R����T��
            joyconL = joyconList.Find(c => c.isLeft);
            joyconR = joyconList.Find(c => !c.isLeft);
        }
        // �X�s�[�h���Œ�X�s�[�h�ŏ�����
        speed = minSpeed;
        rigidbody.linearVelocity = transform.forward * speed;
        //���������������I�u�W�F�N�g�Ƀv���C���[�^�O��t�^���� > player�^�O���������Ă��Ď��@�ɃJ������Ǐ]������
        if (photonView.IsMine)
        {
            tag = "Player";
            enabled = true;
        }
        else
        {
            tag = "Untagged";
            enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //���������������I�u�W�F�N�g�ɂ̂ݏ������s��
        if (photonView.IsMine)
        {
            gyroR = Vector3.zero;
            gyroL = Vector3.zero;
            if (joyconList != null && joyconList.Count != 0)
            {
                // �W���C�R���̑O�t���[���Ƃ̊p�x�̍������擾
                gyroR = joyconR.GetGyro();
                gyroL = joyconL.GetGyro();
            }
            else
            {
                totalGyroL = Vector3.zero;
                totalGyroR = Vector3.zero;
            }

            // �f�o�b�N�p�̃L�[����
            float keyRate = 100.0f;
            if (Input.GetKey(KeyCode.W))
            {
                gyroR.x = -keyRate;
                gyroL.x = keyRate;
            }
            if (Input.GetKey(KeyCode.S))
            {
                gyroR.x = keyRate;
                gyroL.x = -keyRate;
            }
            if (Input.GetKey(KeyCode.A))
            {
                gyroR.z = keyRate;
                gyroL.z = -keyRate;
            }
            if (Input.GetKey(KeyCode.D))
            {
                gyroR.z = -keyRate;
                gyroL.z = keyRate;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                gyroR.y = -keyRate;
                gyroL.y = keyRate;
            }
            if (Input.GetKey(KeyCode.E))
            {
                gyroR.y = keyRate;
                gyroL.y = -keyRate;
            }


            // �������猻�݂̃W���C�R���̊p�x���Z�o
            totalGyroR += gyroR;
            totalGyroL += gyroL;


            // �����̌X���͖���
            if (gyroR.x < rotateOffset.x && gyroR.x > -rotateOffset.x)
            {
                gyroR.x = 0.0f;
            }
            if (gyroR.y < rotateOffset.y && gyroR.y > -rotateOffset.y)
            {
                gyroR.y = 0.0f;
            }
            if (gyroR.z < rotateOffset.z && gyroR.z > -rotateOffset.z)
            {
                gyroR.z = 0.0f;
            }
            if (gyroL.x < rotateOffset.x && gyroL.x > -rotateOffset.x)
            {
                gyroL.x = 0.0f;
            }
            if (gyroL.y < rotateOffset.y && gyroL.y > -rotateOffset.y)
            {
                gyroL.y = 0.0f;
            }
            if (gyroL.z < rotateOffset.z && gyroL.z > -rotateOffset.z)
            {
                gyroL.z = 0.0f;
            }

            // ���E�̃W���C�R����x���̊p�x�̍�����E�̃W���C�R�������Ђ˂��Ă��邩�i�A�N�Z���j�𔻒�
            float offsetGyroX = totalGyroL.x + totalGyroR.x;

            // �N���b�`(����)
            if (offsetGyroX < -20.0f)
            {
                speed += offsetGyroX * (-1.0f) * speedRate;
                virtualCamera.m_Lens.FieldOfView += addFov;
                if (virtualCamera.m_Lens.FieldOfView > maxFov)
                {
                    virtualCamera.m_Lens.FieldOfView = maxFov;
                }
            }
            // FOV�����Ƃɖ߂�
            else if (virtualCamera.m_Lens.FieldOfView > minFov)
            {
                virtualCamera.m_Lens.FieldOfView += -subFov;
                if (virtualCamera.m_Lens.FieldOfView <= minFov)
                {
                    virtualCamera.m_Lens.FieldOfView = minFov;
                }
            }

            if (Input.GetKey(KeyCode.Space))
            {
                speed += 0.2f;
                // FOV�ύX����
                virtualCamera.m_Lens.FieldOfView += addFov;
                if (virtualCamera.m_Lens.FieldOfView > maxFov)
                {
                    virtualCamera.m_Lens.FieldOfView = maxFov;
                }
            }
            // FOV�����Ƃɖ߂�
            else if (virtualCamera.m_Lens.FieldOfView > minFov)
            {
                virtualCamera.m_Lens.FieldOfView += -subFov;
                if (virtualCamera.m_Lens.FieldOfView <= minFov)
                {
                    virtualCamera.m_Lens.FieldOfView = minFov;
                }
            }

            // �Œ�E�ō����x�`�F�b�N
            if (speed < minSpeed)
            {
                speed = minSpeed;
            }
            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }

            // �u���[�L
            brakeFlag = false;
            if ((joyconR != null && joyconR.GetButton(Joycon.Button.SHOULDER_2)) || Input.GetKey(KeyCode.V))
            {
                brakeFlag = true;
            }

            // �W���C�����Z�b�g
            if ((joyconR != null && joyconR.GetButtonDown(Joycon.Button.SHOULDER_1)) || Input.GetKey(KeyCode.R))
            {
                totalGyroR = Vector3.zero;
                totalGyroL = Vector3.zero;
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0.0f);
                speed = minSpeed;
            }
        }

    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 position = transform.position + transform.forward * 100;

            Vector3 spwanPos = new Vector3(transform.position.x, transform.position.y, transform.position.z) + transform.forward * 500;
            particleObject = PhotonNetwork.Instantiate("TestParticleSystem", spwanPos, transform.rotation);
        }

        //���g�����������I�u�W�F�N�g�����Ɉړ��������s��   
        if (photonView.IsMine)
        {
            var vec = (transform.forward * speed) - rigidbody.linearVelocity;
            rigidbody.AddForce(vec, ForceMode.Force);

            if (brakeFlag)
            {
                speed -= brakePower;
                if (speed < 0.0f)
                {
                    speed = 0.0f;
                }
            }

            isRotate = false;

            // �ǂ����x�����X���Ă���Ȃ�㏸�E���~
            if (totalGyroL.x > 50.0f || totalGyroL.x < -50.0f || gyroL.x != 0.0f)
            {
                isRotate = true;
                if (rollControlCoroutine != null)
                {
                    StopCoroutine(rollControlCoroutine);
                    rollControlCoroutine = null;
                }
                float angle = totalGyroL.x * (-1.0f) * updownRate * Time.fixedDeltaTime;
                transform.Rotate(Vector3.right, angle, Space.Self);
                // -180����180�ɐ��K��
                float normAngle = Mathf.Repeat(transform.eulerAngles.x + 180.0f, 360.0f) - 180.0f;
                if (normAngle < -85.0f || normAngle > 85.0f)
                {
                    transform.Rotate(Vector3.right, -angle, Space.Self);
                }
            }

            // �ǂ����z�����X���Ă���Ȃ獶�E�ړ�
            if (totalGyroL.z > 50.0f || totalGyroL.z < -50.0f || gyroL.z != 0.0f)
            {
                isRotate = true;
                if (rollControlCoroutine != null)
                {
                    StopCoroutine(rollControlCoroutine);
                    rollControlCoroutine = null;
                }
                transform.Rotate(Vector3.up, totalGyroL.z * leftlightRate * Time.fixedDeltaTime, Space.World);
                float angle = totalGyroL.z * (-1.0f) * rollRate * Time.fixedDeltaTime;
                transform.Rotate(Vector3.forward, angle, Space.Self);
                // -180����180�ɐ��K��
                float normAngle = Mathf.Repeat(transform.eulerAngles.z + 180.0f, 360.0f) - 180.0f;
                if (normAngle < -45.0f || normAngle > 45.0f)
                {
                    transform.Rotate(Vector3.forward, -angle, Space.Self);
                }
            }
            if (!isRotate && (transform.eulerAngles.z > 0.001f || transform.eulerAngles.z < -0.001f) && rollControlCoroutine == null)
            {
                rollControlCoroutine = StartCoroutine(RollAttitudeControl());
            }
        }
    }

    /// <summary>
    /// ���[���̎p����߂��R���[�`��
    /// </summary>
    /// <returns></returns>
    public IEnumerator RollAttitudeControl()
    {
        // ���݂̃��[�������̌X���Ɛ��펞�̌X������A�p���𒼂��N�H�[�^�j�I�����쐬
        Quaternion fromQuat = transform.rotation;
        Quaternion toQuat = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0.0f);
        float nowTime = 0.0f;
        while (true)
        {
            nowTime += Time.fixedDeltaTime;
            Quaternion rot = Quaternion.Slerp(fromQuat, toQuat, nowTime / rollControlTime);
            transform.rotation = rot;
            if (nowTime / rollControlTime >= 1.0f)
            {
                rollControlCoroutine = null;
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
