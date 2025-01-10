using UnityEngine;

[CreateAssetMenu(fileName = "CameraData", menuName = "ScriptableObjects/CreateCameraDataAsset")]
public class CameraData : ScriptableObject
{
	// �J�����̍Œ�FOV
	[SerializeField, Tooltip("�J�����̍Œ�FOV")]
	private float minFov = 65.0f;
	public float MinFov
	{
		get { return minFov; }
		private set { minFov = value; }
	}

	// �J�����̍ō�FOV
	[SerializeField, Tooltip("�J�����̍ō�FOV")]
	private float maxFov = 100.0f;
	public float MaxFov
	{
		get { return maxFov; }
		private set { maxFov = value; }
	}

	// FOV�̉��Z�l�i�������j
	[SerializeField, Tooltip("FOV�̉��Z�l�i�������j")]
	private float addFov = 0.85f;
	public float AddFov
	{
		get { return addFov; }
		private set { addFov = value; }
	}

	// FOV�̌��Z�l�i�������j
	[SerializeField, Tooltip("FOV�̌��Z�l�i�������j")]
	private float subFov = 0.15f;
	public float SubFov
	{
		get { return subFov; }
		private set { subFov = value; }
	}

	// ���@����]���Ă��Ȃ��Ƃ��̃I�t�Z�b�g�i�ύX����Ȃ�Y�����j
	[SerializeField, Tooltip("���@����]���Ă��Ȃ��Ƃ��̃I�t�Z�b�g�i�ύX����Ȃ�Y�����j")]
	private Vector3 startTrackedOffset = new Vector3(0.0f, 1.4f, 0.0f);
	public Vector3 StartTrackedOffset
	{
		get { return startTrackedOffset; }
		set { startTrackedOffset = value; }
	}


	// ���@����]���Ă���ۂɎ��@��[�Ɋ񂹂�I�t�Z�b�g
	[SerializeField, Tooltip("���@����]�����ۂɎ��@��[�Ɋ񂹂�I�t�Z�b�g")]
	private Vector3 trackedOffset = new Vector3(2.5f, 3.0f, 0.0f);
	public Vector3 TrackedOffset
	{
		get { return trackedOffset; }
		set { trackedOffset = value; }
	}


	// ���@����]���Ă���ۂɎ��@���[�Ɋ��܂ł̎���(3�������ꂼ��ɐݒ�ł���)
	[SerializeField, Tooltip("���@����]���Ă���ۂɎ��@���[�Ɋ��܂ł̎���(3�������ꂼ��ɐݒ�ł���)")]
	private Vector3 trackedTime = new Vector3(3.2f,5.0f,0.0f);
	public Vector3 TrackedTime
	{
		get { return trackedTime; }
		set { trackedTime = value; }
	}

	// ���@���[�ɂ���Đ^�񒆂܂Ŗ߂��Ă��鎞��(3�������ꂼ��ɐݒ�ł���)
	[SerializeField, Tooltip("���@���[�ɂ���Đ^�񒆂܂Ŗ߂��Ă��鎞��(3�������ꂼ��ɐݒ�ł���)")]
	private Vector3 returnTime = new Vector3(3.2f, 5.0f, 0.0f);
	public Vector3 ReturnTime
	{
		get { return returnTime; }
		set { returnTime = value; }
	}

	// ���e�B�N���̕W������
	[SerializeField, Tooltip("���e�B�N���̕W������")]
	private float reticleDistance = 100.0f;
	public float ReticleDistance
	{
		get { return reticleDistance; }
		set { reticleDistance = value; }
	}


}

