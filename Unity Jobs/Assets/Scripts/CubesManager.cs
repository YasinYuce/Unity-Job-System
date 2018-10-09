using UnityEngine;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;

public class CubesManager : MonoBehaviour 
{
	[SerializeField]
	private bool UnityJobs = false;
	[SerializeField]
	private int xCount = 150, zCount = 150;
	private float speed = 3f;

	//Arrays
	private Transform[] _transforms;

	//Native Arrays
	private TransformAccessArray transAccessArray;

	#region Initialize
	//Cube Prefab
	private GameObject prefab;
	void Awake(){
		prefab = Resources.Load ("Cube") as GameObject;

		int size = xCount * zCount;

		_transforms = new Transform[size];
		for (int x = 0; x < xCount; x++) {
			for (int z = 0; z < zCount; z++) {
				int index = x * zCount + z;
				_transforms [index] = Instantiate (prefab, new Vector3 (x, 0f, z), Quaternion.identity).transform;
				_transforms [index].parent = transform;
			}
		}

		transAccessArray = new TransformAccessArray (_transforms);
	}
	#endregion

	void FixedUpdate(){
		if (UnityJobs)
			scheduleJobs ();
		 else
			withoutJobs ();
	}

	private JobHandle handle;
	private bool reverse = false;
	void scheduleJobs(){
		handle.Complete ();

		// Rotation job
		JobSystem_JobParallelForTransform_Rotation job0 = new JobSystem_JobParallelForTransform_Rotation () {
			RotationSpeed = speed,
		};

		JobHandle handle0 = job0.Schedule (transAccessArray);

		//Position job
		JobSystem_JobParallelForTransform_Position job1 = new JobSystem_JobParallelForTransform_Position () {
			MoveSpeed = reverse ? speed : -speed,
		};

		handle = job1.Schedule (transAccessArray, handle0);

		JobHandle.ScheduleBatchedJobs ();

		reverse = !reverse;
	}

	void withoutJobs(){
		//Rotate all transforms on x, y ,z same as job
		for (int i = 0; i < _transforms.Length; i++) {
			Vector3 t = _transforms [i].rotation.eulerAngles;

			t.x += speed;
			t.y -= speed;
			t.z += speed;

			_transforms [i].rotation = Quaternion.Euler (t);
		}
		//change positions of all transforms on x, y ,z same as job
		float _speed = reverse ? speed : -speed;
		for (int i = 0; i < _transforms.Length; i++) {
			Vector3 t = _transforms [i].position;

			t.x += _speed;
			t.y += _speed;
			t.z += _speed;

			_transforms [i].position = t;
		}

		reverse = !reverse;
	}

	void OnDestroy(){
		handle.Complete ();
		transAccessArray.Dispose ();
	}
}
