using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/CreatePlayerDataAsset")]
public class PlayerData : ScriptableObject
{
	[Header("��{����")]
	// �ő�HP
	[SerializeField, Tooltip("�ő�HP")]
	private int maxHp;
	public int MaxHp
	{
		get { return maxHp; }
		set { maxHp = value; }
	}


	// �Œᑬ�x
	[SerializeField,Tooltip("�Œᑬ�x")]
	private float minSpeed;
	public float MinSpeed
	{
		get { return minSpeed; }
		private set { minSpeed = value; }
	}

	// �ō����x
	[SerializeField, Tooltip("�ō����x")]
	private float maxSpeed;
	public float MaxSpeed
	{
		get { return maxSpeed; }
		private set { maxSpeed = value; }
	}

	[SerializeField, Tooltip("�u�[�X�g�����Ă��Ȃ����̑��x����")]
	private float speedDecay = 1.0f;

	public float SpeedDecay
	{
		get { return speedDecay; }
		set { speedDecay = value; }
	}

	// �u���[�L��
	[SerializeField, Tooltip("�u���[�L��")]
	private float brakePower = 1.0f;
	public float BrakePower
	{
		get { return brakePower; }
		set { brakePower = value; }
	}

	// �p���𒼂��܂ł̎���
	[SerializeField, Tooltip("�p���𒼂��܂ł̎���")]
	private float rollControllTime = 1.0f;
	public float RollControllTime
	{
		get { return rollControllTime; }
		private set { rollControllTime = value; }
	}

	// �p���𒼂�����
	[SerializeField, Tooltip("�p���𒼂�����")]
	private float returnRollSpeed = 1.0f;

	public float ReturnRollSpeed
	{
		get { return returnRollSpeed; }
		set { returnRollSpeed = value; }
	}


	[Header("�u�[�X�g�̐ݒ�")]
	[SerializeField, Tooltip("���v�u�[�X�g��")]
	private float totalBoost = 100.0f;
	public float TotalBoost
	{
		get { return totalBoost; }
		set { totalBoost = value; }
	}

	[SerializeField, Tooltip("1�b�Ԃŉ񕜂���u�[�X�g�̗�")]
	private float boostCharge = 20.0f;
	public float BoostCharge
	{
		get { return boostCharge; }
		set { boostCharge = value; }
	}



	[Header("�m�b�N�o�b�N�̐ݒ�")]
	// �ǂɂԂ������ۂ̃m�b�N�o�b�N�����
	[SerializeField, Tooltip("�ǂɂԂ������ۂ̃m�b�N�o�b�N�����")]
	private float knockbackPower = 10.0f;
	public float KnockbackPower
	{
		get { return knockbackPower; }
		private set { knockbackPower = value; }
	}

	// �m�b�N�o�b�N���Ă���i�ޑ��x
	[SerializeField,Tooltip("�m�b�N�o�b�N���Ă���i�ޑ��x")]
	private float returnSpeed;

	public float ReturnSpeed
	{
		get { return returnSpeed; }
		set { returnSpeed = value; }
	}

	[Header("��e���̐ݒ�")]
	// �C�⎞��
	[SerializeField, Tooltip("�C�⎞��")]
	private float stunTime = 3.0f;
	public float StunTime
	{
		get { return stunTime; }
		set { stunTime = value; }
	}

	// ��]��
	[SerializeField, Tooltip("��e�����Ƃ��Ɏ��@�̍��v��]��")]
	private int stunRotation = 4;
	public int StunRotation
	{
		get { return stunRotation; }
		set { stunRotation = value; }
	}



	[Header("�����蔻��̐ݒ�")]

	// 2�{��Ray�̊Ԋu
	[SerializeField, Tooltip("2�{��Ray�̊Ԋu")]
	private float rayOffset = 0.8f;
	public float RayOffset
	{
		get { return rayOffset; }
		set { rayOffset = value; }
	}

	// Ray�̒���
	[SerializeField, Tooltip("Ray�̒���")]
	private float rayDistance = 3.0f;
	public float RayDistance
	{
		get { return rayDistance; }
		set { rayDistance = value; }
	}

	// �n�ʂƂ̍ŒZ����
	[SerializeField, Tooltip("�n�ʂƂ̍ŒZ����")]
	private float minHeight = 2.0f;
	public float MinHeight
	{
		get { return minHeight; }
		set { minHeight = value; }
	}


	[Header("�e���̓f�o�C�X�̊��x�ݒ�")]
	// �L�[�{�[�h
	[SerializeField, Tooltip("�L�[�{�[�h�ł̐��񊴓x")]
	private Vector3 keyboardSens = new Vector3(1.2f, 1.0f, 0.8f);
	public Vector3 KeyboardSens
	{
		get { return keyboardSens; }
		set { keyboardSens = value; }
	}

	// �W���C�R��
	[SerializeField, Tooltip("�W���C�R���ł̐��񊴓x")]
	private Vector3 joyconSens = new Vector3(1.0f, 1.0f, 1.0f);
	public Vector3 JoyconSens
	{
		get { return joyconSens; }
		set { joyconSens = value; }
	}

	// �X�}�[�g�t�H��
	[SerializeField, Tooltip("�X�}�[�g�t�H���ł̐��񊴓x")]
	private Vector3 smartphoneSens = new Vector3(1.0f, 1.0f, 1.0f);
	public Vector3 SmartphoneSens
	{
		get { return smartphoneSens; }
		set { smartphoneSens = value; }
	}

	[Header("�W���C�R���̐ݒ�")]

	[SerializeField, Tooltip("�W���C�R���̍ő�p�x")]
	private Vector2 maxJoyconRot = new Vector2(90.0f, 90.0f);
	public Vector2 MaxJoyconRot
	{
		get { return maxJoyconRot; }
		set { maxJoyconRot = value; }
	}

	[SerializeField, Tooltip("�W���C�R���̃f�b�h�]�[���i�������Ȃ��͈�0.0����1.0�܂Łj")]
	private Vector2 joyconDeadzone = new Vector2(0.1f, 0.1f);
	public Vector2 JoyconDeadzone
	{
		get { return joyconDeadzone; }
		set { joyconDeadzone = value; }
	}

	[SerializeField, Tooltip("�A�N�Z���𔻒肷��E�W���C�R���̊p�x")]
	private float accelRot = 30.0f;
	public float AccelRot
	{
		get { return accelRot; }
		set { accelRot = value; }
	}

	[SerializeField, Tooltip("�A�N�Z����P�������̉����̑傫��")]
	private float accelPower = 1.0f;
	public float AccelPower
	{
		get { return accelPower; }
		set { accelPower = value; }
	}


	[Header("�A�C�e���̐ݒ�")]
	// �ō����x�̃A�C�e��
	[SerializeField, Tooltip("�ō����x�̃A�C�e��")]
	private float item_maxSpeed = 3.0f;
	public float Item_maxSpeed
	{
		get { return item_maxSpeed; }
		set { item_maxSpeed = value; }
	}

	// �����x�̃A�C�e��
	[SerializeField, Tooltip("�����x�̃A�C�e��")]
	private float item_acceleration = 0.01f;
	public float Item_acceleration
	{
		get { return item_acceleration; }
		set { item_acceleration = value; }
	}

	// �y�ʂ̃A�C�e��
	[SerializeField, Tooltip("�y�ʂ̃A�C�e��")]
	private float item_lightness = 0.1f;
	public float Item_lightness
	{
		get { return item_lightness; }
		set { item_lightness = value; }
	}

	// ����̃A�C�e��
	[SerializeField, Tooltip("����̃A�C�e��")]
	private Vector3 item_rotation = new Vector3(0.05f, 0.05f, 0.05f);
	public Vector3 Item_rotation
	{
		get { return item_rotation; }
		set { item_rotation = value; }
	}

	// ��C�C�̃`���[�W�A�C�e��
	[SerializeField, Tooltip("��C�C�̃`���[�W�A�C�e��")]
	private float item_charge = -0.05f;
	public float Item_charge
	{
		get { return item_charge; }
		set { item_charge = value; }
	}


}
