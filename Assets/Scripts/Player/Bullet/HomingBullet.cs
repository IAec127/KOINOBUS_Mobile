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

    public int ID { get; set; } = -1;   // この弾のID
    public int OwnerID { get; set; } = -1;  // この弾を撃ったプレイヤーのID
    public bool isCarge { get; set; } = false;
    private float speed;
    public Transform Target { get;  private set; }


    private bool isHomingActive = false; // 初期状態では追尾を無効化

	void Start()
    {
        StartCoroutine(EnableHomingAfterDelay()); // 追尾を一定時間後に有効化
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

        // チャージショットの場合の特性調整
        if (isFullyCharged)
        {
            data.HomingPower *= 1.5f;      // 追尾の回転速度を増加
            // data.HomingTime *= 1.5f;     // 追尾時間を延長
        }
        if(targetType==BulletSpawner.TARGET_TYPE.NONE||targetID==-1)
        {
            Target = null;
        }
        else
        {
            // ターゲットを見つける
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
        // 狂風イベント中は追尾時間が伸びる
        if (localPlayer.EventType == EventManager.EVENT_TYPE.CRAZY_WIND)
        {
			yield return new WaitForSeconds(eventData.CrazyHomingTime); // 追尾時間分待機
		}
		else
        {
            if (isCarge)
            {
				yield return new WaitForSeconds(data.ChargeHomingTime); // 追尾時間分待機
			}
			else
            {
                yield return new WaitForSeconds(data.HomingTime); // 追尾時間分待機
            }
        }
        isHomingActive = false; // 追尾を無効化

        // 追尾終了後、少し待ってから弾を消す
        yield return new WaitForSeconds(1.0f);
        if(PhotonNetwork.LocalPlayer.ActorNumber==OwnerID)
        {
            BulletManager.Remove(this);
        }
    }

    private IEnumerator EnableHomingAfterDelay()
    {
        yield return new WaitForSeconds(data.BeforeHomingTime); // 遅延時間待機
		isHomingActive = true; // 追尾を有効化
        yield return StartCoroutine(DisableHomingAfterDuration()); // 一定時間後に追尾を無効化
    }

}