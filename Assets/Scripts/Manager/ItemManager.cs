using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject itemPrefab = null;
	[SerializeField]
	private GameObject parent = null;
	[SerializeField]
	private BoxCollider spawnArea = null;
	// 現在生成されているアイテム
	List<Item> items = new List<Item>();
    private int id = 0;
	private int maxItem = 30;
	private float createTime = 2.0f;
	private bool isCreating = false;

	// Start is called before the first frame update
	void Start()
    {
        FindItems();
    }

	private void Update()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			// 一定間隔でエネミーを補充
			if (items.Count < maxItem && !isCreating)
			{
				StartCoroutine(SpawnItem());
			}
		}
	}

	private IEnumerator SpawnItem()
	{
		isCreating = true;
		yield return new WaitForSeconds(createTime);
		Create();
		isCreating = false;
		yield break;
	}


	public void SpawnMaxItem()
	{
		for (int i = 0; i < maxItem; i++)
		{
			Create();
		}
	}


	private Item FindItem(int id)
    {
		// アイテムを探す
		Item item = null;
		foreach (var itemObj in items)
		{
			if (itemObj.ID == id)
			{
				item = itemObj;
			}
		}
        return item;
	}

	public void Create()
	{
		Item.ITEM_EFFECT effect = (Item.ITEM_EFFECT)Random.Range(1, 6);
		Item.ITEM_TYPE type = (Item.ITEM_TYPE)Random.Range(1, 3);
		// 敵が湧く範囲内からランダムで位置を決定
		Vector3 pos = Vector3.zero;
		Bounds bounds = spawnArea.bounds;
		pos = new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), Random.Range(bounds.min.z, bounds.max.z));

		photonView.RPC(nameof(CreateItem), RpcTarget.All, effect, type, pos);
	}

	public void Create(Item.ITEM_EFFECT effect, Item.ITEM_TYPE type, Vector3 pos)
	{
		photonView.RPC(nameof(CreateItem), RpcTarget.All, effect, type, pos);
	}

	[PunRPC]
	public void CreateItem(Item.ITEM_EFFECT effect,Item.ITEM_TYPE type, Vector3 pos)
    {
        GameObject itemObj = Instantiate(itemPrefab, pos, Quaternion.identity,parent.transform);
        Item item = itemObj.GetComponent<Item>();
        // IDをセット
        item.ID = id++;
        // アイテムの種類と効果をセット
        item.ItemEffect = effect;
        item.ItemType = type;
		item.ItemManager = this;
        // リストに追加
        items.Add(item);
    }

    public void FindItems()
    {
		// すでにワールドにあるアイテムを探してリストに格納
		var itemObjs = GameObject.FindGameObjectsWithTag("Item");
		foreach (var itemObj in itemObjs)
		{
            Item item = itemObj.GetComponent<Item>();
			items.Add(item);
            item.ID= id++;
			item.ItemManager = this;
		}
	}

	public void Get(int id,int ownerID)
	{
		photonView.RPC(nameof(GetItem), RpcTarget.All, id, ownerID);
	}

    [PunRPC]
	public void GetItem(int id, int ownerId)
    {
		Item item = FindItem(id);
        PlayerMove player = SceneController.instance.FindPlayer(ownerId);
		if (item == null)
		{
			Debug.Log("アイテムが見つかりませんでした。");
            return;
		}
		if (player == null)
		{
			Debug.Log("プレイヤーが見つかりませんでした。");
			return;
		}
		// アイテム取得処理
		player.GetItem(item.ItemEffect, item.ItemType);
		item.CreateEffect(player.transform);
		items.Remove(item);
		if(item.gameObject != null)
		{
			Destroy(item.gameObject);
		}
    }

}
