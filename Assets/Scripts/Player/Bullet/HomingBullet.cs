using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    [SerializeField]
    BulletData data = null;
    [SerializeField]
    EventData eventData = null;
    public BulletManager BulletManager { get; set; }

    public int ID { get; set; } = -1;   // ���̒e��ID
    public int OwnerID { get; set; } = -1;  // ���̒e���������v���C���[��ID
    public bool isCarge { get; set; } = false;
    private float speed;
    public Transform Target { get;  private set; }


    private bool isHomingActive = false; // ������Ԃł͒ǔ��𖳌���

	void Start()
    {
        StartCoroutine(EnableHomingAfterDelay()); // �ǔ�����莞�Ԍ�ɗL����
    }

    void FixedUpdate()
    {
		if (isHomingActive && Target != null)
		{

			Vector3 directionToTarget = (Target.position - transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, data.HomingPower * Time.fixedDeltaTime);
        }

       
        speed = Mathf.Min(speed + data.Acceleration * Time.fixedDeltaTime, data.MaxSpeed); // Limit speed to maxSpeed
        transform.position += transform.forward * speed * Time.fixedDeltaTime;
    }

    public void InitializeBullet(float speed, bool isFullyCharged, BulletSpawner.TARGET_TYPE targetType, int targetID)
    {
        this.speed = speed;

        // �`���[�W�V���b�g�̏ꍇ�̓�������
        if (isFullyCharged)
        {
            data.HomingPower *= 1.5f;      // �ǔ��̉�]���x�𑝉�
            // data.HomingTime *= 1.5f;     // �ǔ����Ԃ�����
        }
        if(targetType==BulletSpawner.TARGET_TYPE.NONE||targetID==-1)
        {
            Target = null;
        }
        else
        {
            // �^�[�Q�b�g��������
            Target = SceneController.instance.FindPlayer(PhotonNetwork.LocalPlayer.ActorNumber).
                                        GetComponent<BulletSpawner>().FindTarget(targetType, targetID);
        }
    }

    public void FindTarget(BulletSpawner.TARGET_TYPE type,int targetID)
    {

    }

    private IEnumerator DisableHomingAfterDuration()
    {
        var localPlayer = SceneController.instance.FindPlayer(OwnerID);
        // �����C�x���g���͒ǔ����Ԃ��L�т�
        if (localPlayer.EventType == EventManager.EVENT_TYPE.CRAZY_WIND)
        {
			yield return new WaitForSeconds(eventData.CrazyHomingTime); // �ǔ����ԕ��ҋ@
		}
		else
        {
            if (isCarge)
            {
				yield return new WaitForSeconds(data.ChargeHomingTime); // �ǔ����ԕ��ҋ@
			}
			else
            {
                yield return new WaitForSeconds(data.HomingTime); // �ǔ����ԕ��ҋ@
            }
        }
        isHomingActive = false; // �ǔ��𖳌���

        // �ǔ��I����A�����҂��Ă���e������
        yield return new WaitForSeconds(1.0f);
        if(PhotonNetwork.LocalPlayer.ActorNumber==OwnerID)
        {
            BulletManager.Remove(this);
        }
    }

    private IEnumerator EnableHomingAfterDelay()
    {
        yield return new WaitForSeconds(data.BeforeHomingTime); // �x�����ԑҋ@
		isHomingActive = true; // �ǔ���L����
        yield return StartCoroutine(DisableHomingAfterDuration()); // ��莞�Ԍ�ɒǔ��𖳌���
    }

}