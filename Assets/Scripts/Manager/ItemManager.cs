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
	// ���ݐ�������Ă���A�C�e��
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
			// ���Ԋu�ŃG�l�~�[���[
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
		// �A�C�e����T��
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
		// �G���N���͈͓����烉���_���ňʒu������
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
        // ID���Z�b�g
        item.ID = id++;
        // �A�C�e���̎�ނƌ��ʂ��Z�b�g
        item.ItemEffect = effect;
        item.ItemType = type;
		item.ItemManager = this;
        // ���X�g�ɒǉ�
        items.Add(item);
    }

    public void FindItems()
    {
		// ���łɃ��[���h�ɂ���A�C�e����T���ă��X�g�Ɋi�[
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
			Debug.Log("�A�C�e����������܂���ł����B");
            return;
		}
		if (player == null)
		{
			Debug.Log("�v���C���[��������܂���ł����B");
			return;
		}
		// �A�C�e���擾����
		player.GetItem(item.ItemEffect, item.ItemType);
		item.CreateEffect(player.transform);
		items.Remove(item);
		if(item.gameObject != null)
		{
			Destroy(item.gameObject);
		}
    }

}
