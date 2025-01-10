using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class Item : MonoBehaviour
{
    public const int ITEM_COUNT = 5;
    public const int ITEM_MAX = 10;
    // �A�C�e���̌��ʂ�\��
    public enum ITEM_EFFECT
    {
        NONE,
        MAX_SPEED,      // �ō����x
        ACCELERATION,   // �����x
        LIGHTNESS,      // �y��
        ROTATION,       // ����
        CHARGE,         // ��C�C�̃`���[�W
	}
	// �A�C�e���̎�ނ�\��
	public enum ITEM_TYPE
    {
        NONE,
        PLUS,
        MINUS,
    }

    [SerializeField]
    private ITEM_EFFECT itemEffect;
    public ITEM_EFFECT ItemEffect
    {
        get { return itemEffect; }
        set { itemEffect = value; }
    }

    [SerializeField]
    private ITEM_TYPE itemType;
    public ITEM_TYPE ItemType
    {
        get { return itemType; }
        set { itemType = value; }
    }

    public int ID { get; set; }

    [SerializeField]
    private Sprite[] plusSprites =  new Sprite[ITEM_COUNT];
    [SerializeField]
    private Sprite[] minusSprites = new Sprite[ITEM_COUNT];
    [SerializeField]
    private GameObject[] getEffects=new GameObject[ITEM_COUNT];
    public ItemManager ItemManager { get; set; } = null;
    private GameObject vCamera;

    // Start is called before the first frame update
    void Start()
    {
        vCamera = GameObject.FindGameObjectWithTag("VirtualCamera");
        // �X�v���C�g��ύX
        if (itemEffect == ITEM_EFFECT.NONE && itemType == ITEM_TYPE.NONE)
        {
            Debug.Log("�A�C�e���̌��ʂ���ނ��ݒ肳��Ă��܂���!");
            return;
        }
        SpriteRenderer sprite=GetComponent<SpriteRenderer>();
        if (itemType == ITEM_TYPE.PLUS)
        {
            sprite.sprite = plusSprites[(int)itemEffect-1];
        }
        else
        {
			sprite.sprite = minusSprites[(int)itemEffect-1];
		}
	}

    // Update is called once per frame
    void Update()
    {
		Vector3 p = vCamera.transform.position;
		transform.LookAt(p);
	}

    public void CreateEffect(Transform transform)
    {
		Instantiate(getEffects[(int)itemEffect - 1], transform);
	}

	private void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.tag == "Player")
        {
            int ownerID = PhotonNetwork.LocalPlayer.ActorNumber;
			// ���[�J���v���C���[�̋@�̂��`�F�b�N
			if (other.gameObject.GetPhotonView().CreatorActorNr != ownerID)
            {
                return;
            }
            // �v���C���[�̃p�����[�^�𑝌�
            ItemManager.Get(ID, ownerID);
        }
	}
}
