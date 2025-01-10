using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    [SerializeField]
    CameraData data = null;
    [SerializeField]
    BulletData bulletData = null;
    private Camera mainCamera = null;
    public GameObject localPlayer { get; set; }
    private RectTransform reticleUI;
    public BulletSpawner bulletSpawner { get; set; }
    [SerializeField]
    List<GameObject> bulletUIList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        reticleUI=GetComponent<RectTransform>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(bulletSpawner != null)
        {
            for (int i = 0; i < bulletSpawner.CurrentShots; i++)
            {
                bulletUIList[i].SetActive(true);
            }
            for (int i = bulletData.MaxBullet-1; i >= bulletSpawner.CurrentShots; i--)
            {
                bulletUIList[i].SetActive(false);
            }
        }

        if (localPlayer == null || reticleUI == null)
        {
            return;
        }
		// プレイヤーからRayを飛ばす
		Ray ray = new Ray(localPlayer.transform.position, localPlayer.transform.forward);
		RaycastHit hit;
        Physics.Raycast(ray, out hit, data.ReticleDistance);
		Vector3 targetPosition=ray.GetPoint(data.ReticleDistance);
		// ワールド座標をスクリーン座標に
		Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);
		// スクリーン座標をUIの位置に変換
		reticleUI.position = screenPosition;
	}
}
