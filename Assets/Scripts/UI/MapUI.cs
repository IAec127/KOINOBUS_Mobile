using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> iconList = new List<GameObject>();
    private GameObject minMapPoint = null;
	private GameObject maxMapPoint = null;

	// Start is called before the first frame update
	void Start()
    {
        minMapPoint = GameObject.Find("MinMapPoint");
        maxMapPoint = GameObject.Find("MaxMapPoint");
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < iconList.Count; i++)
        {
            var player = SceneController.instance.FindPlayer(i + 1);
            if(player == null)
            {
                iconList[i].SetActive(false);
                return;
            }
            // マップ外なら表示しない
            if (player.transform.position.x < minMapPoint.transform.position.x || player.transform.position.x > maxMapPoint.transform.position.x ||
                    player.transform.position.z < minMapPoint.transform.position.z || player.transform.position.z > maxMapPoint.transform.position.z)
            {
                iconList[i].SetActive(false);
                return;
            }
            if (!iconList[i].activeSelf)
            {
                iconList[i].SetActive(true);
            }
            // 現在のプレイヤーの座標とマップのサイズから現在地の割合(0～1)を算出
            Vector2 mapSize = new Vector2(maxMapPoint.transform.position.x - minMapPoint.transform.position.x,
                                                maxMapPoint.transform.position.z - minMapPoint.transform.position.z);
            Vector2 playerPosRatio = new Vector2((player.transform.position.x - minMapPoint.transform.position.x) / mapSize.x,
                                                    (player.transform.position.z - minMapPoint.transform.position.z) / mapSize.y);
			playerPosRatio.x = 1.0f - playerPosRatio.x;
			playerPosRatio.y = 1.0f - playerPosRatio.y;
            var rect = iconList[i].GetComponent<RectTransform>();
            rect.pivot = playerPosRatio;
            rect.localPosition = Vector3.zero;
            var bodyRect = iconList[i].transform.GetChild(0).GetComponent<RectTransform>();
            bodyRect.rotation = Quaternion.Euler(0.0f, 0.0f, -player.transform.eulerAngles.y);
		}
	}
}
