using UnityEngine;

[CreateAssetMenu(fileName = "EventData", menuName = "ScriptableObjects/CreateEventDataAsset")]

public class EventData : ScriptableObject
{
	// �C�x���g�̎���
	[SerializeField, Tooltip("�C�x���g�̎���")]
	private float eventTime = 30;
	public float EventTime
	{
		get { return eventTime; }
		set { eventTime = value; }
	}

	// �C�x���g���쐬����܂ł̎���
	[SerializeField, Tooltip("�C�x���g���쐬����܂ł̎���")]
	private float createEventTime = 90;
	public float CreateEventTime
	{
		get { return createEventTime; }
		set { createEventTime = value; }
	}

	// �ǂ����ŏ㏸����ō����x
	[SerializeField, Tooltip("�ǂ����ŏ㏸����ō����x")]
	private float tailWindSpeed = 30;
	public float TailWindSpeed
	{
		get { return tailWindSpeed; }
		set { tailWindSpeed = value; }
	}

	// ���������Ō�������ō����x
	[SerializeField, Tooltip("���������Ō�������ō����x")]
	private float headWindSpeed = -60;

	public float HeadWindSpeed
	{
		get { return headWindSpeed; }
		set { headWindSpeed = value; }
	}

	// �㏸�C���̃x�N�g��
	[SerializeField, Tooltip("�㏸�C���̃x�N�g��")]
	private Vector3 upBurstVec = new Vector3(0.0f, 1.0f, 0.0f);
	public Vector3 UpBurstVec
	{
		get { return upBurstVec; }
		set { upBurstVec = value; }
	}

	// ���~�C���̃x�N�g��
	[SerializeField, Tooltip("���~�C���̃x�N�g��")]
	private Vector3 downBurstVec = new Vector3(0.0f, -1.0f, 0.0f);
	public Vector3 DownBurstVec
	{
		get { return downBurstVec; }
		set { downBurstVec = value; }
	}


	// �������̐��񑬓x�̉��Z�l
	[SerializeField, Tooltip("�������̐��񑬓x�̉��Z�l")]
	private Vector3 tornadoSens = new Vector3(1.0f, 1.0f, 0.8f);
	public Vector3 TornadoSens
	{
		get { return tornadoSens; }
		set { tornadoSens = value; }
	}

	// �������̑��������e�̒ǔ�����
	[SerializeField, Tooltip("�������̑��������e�̒ǔ�����")]
	private float crazyHomingTime = 5.0f;
	public float CrazyHomingTime
	{
		get { return crazyHomingTime; }
		set { crazyHomingTime = value; }
	}


	[Header("UI�p�e�L�X�g�̐ݒ�")]

	[SerializeField]
	private float popupTime = 5.0f;
	public float PopupTime
	{
		get { return popupTime; }
		set { popupTime = value; }
	}


	[SerializeField]
	private string tailWindText = "�˕��������I30�b�ԋ@�̂̑��x���ō����x�ɌŒ�!!";
	public string TailWindText
	{
		get { return tailWindText; }
		set { tailWindText = value; }
	}

	[SerializeField]
	private string headWindText = "�t���������I30�b�ԋ@�̂̑��x���ō����x���ቺ!!";
	public string HeadWindText
	{
		get { return headWindText; }
		set { headWindText = value; }
	}

	[SerializeField]
	private string upBurstText = "�㏸�C���������I30�b�ԋ@�̂������グ����!!";
	public string UpBurstText
	{
		get { return upBurstText; }
		set { upBurstText = value; }
	}

	[SerializeField]
	private string downBurstText = "�_�E���o�[�X�g�������I30�b�ԋ@�̂�����!!";
	public string DownBurstText
	{
		get { return downBurstText; }
		set { downBurstText = value; }
	}

	[SerializeField]
	private string tornadoText = "�����������I30�b�ԋ@�̂̐��񂪏㏸!!";
	public string TornadoText
	{
		get { return tornadoText; }
		set { tornadoText = value; }
	}

	[SerializeField]
	private string crazyWindText = "�����������I30�b�Ԓe����������!!";
	public string CrazyWindText
	{
		get { return crazyWindText; }
		set { crazyWindText = value; }
	}

}
