using UnityEngine;
using UnityEngine.Jobs;
using Unity.Collections;

public struct JobSystem_JobParallelForTransform_Rotation : IJobParallelForTransform
{
	public float RotationSpeed;

	public void Execute (int index, TransformAccess transform)
	{
		Vector3 t = transform.rotation.eulerAngles;

		t.x += RotationSpeed;
		t.y -= RotationSpeed;
		t.z += RotationSpeed;

		transform.rotation = Quaternion.Euler (t);
	}
}

