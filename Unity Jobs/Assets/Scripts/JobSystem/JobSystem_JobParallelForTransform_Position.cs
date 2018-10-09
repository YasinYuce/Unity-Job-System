using UnityEngine;
using UnityEngine.Jobs;
using Unity.Collections;

public struct JobSystem_JobParallelForTransform_Position : IJobParallelForTransform 
{
	public float MoveSpeed;

	public void Execute (int index, TransformAccess transform)
	{
		Vector3 t = transform.position;

		t.x += MoveSpeed;
		t.y += MoveSpeed;
		t.z += MoveSpeed;

		transform.position = t;
	}
}
