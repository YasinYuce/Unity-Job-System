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
				int index = x * xCount + z;
				_transforms [index] = Instantiate (prefab, new Vector3 (x, 0f, z), Quaternion.identity).transform;
				_transforms [index].parent = transform;
			}
		}

		transAccessArray = new TransformAccessArray (_transforms);
	}
	#endregion

	JobHandle handle;
	void FixedUpdate(){
		if (UnityJobs)
			scheduleJob ();
		 else
			withoutJob ();
	}

	void scheduleJob(){
		handle.Complete ();

		JobSystem_JobParallelForTransform job = new JobSystem_JobParallelForTransform () {
			Speed = speed,
		};

		handle = job.Schedule (transAccessArray);

		JobHandle.ScheduleBatchedJobs ();
	}

	void withoutJob(){
		for (int i = 0; i < _transforms.Length; i++) {
			Vector3 t = _transforms [i].rotation.eulerAngles;

			t.x += speed;
			t.y -= speed;
			t.z += speed;

			_transforms [i].rotation = Quaternion.Euler (t);
		}
	}

	void OnDestroy(){
		handle.Complete ();
		transAccessArray.Dispose ();
	}
}
