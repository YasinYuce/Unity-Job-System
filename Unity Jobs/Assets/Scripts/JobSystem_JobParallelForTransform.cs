using UnityEngine;
using UnityEngine.Jobs;
using Unity.Collections;

public struct JobSystem_JobParallelForTransform : IJobParallelForTransform
{
	public float Speed;

	public void Execute (int index, TransformAccess transform)
	{
		Vector3 t = transform.rotation.eulerAngles;

		t.x += Speed;
		t.y -= Speed;
		t.z += Speed;

		transform.rotation = Quaternion.Euler (t);
	}
}

