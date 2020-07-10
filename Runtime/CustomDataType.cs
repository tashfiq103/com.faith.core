using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct UnitVector {
	[Range (-1.0f, 1.0f)]
	public float x;
	[Range (-1.0f, 1.0f)]
	public float y;
	[Range (-1.0f, 1.0f)]
	public float z;
}

[System.Serializable]
public struct UnitVector2D {
	[Range (-1.0f, 1.0f)]
	public float x;
	[Range (-1.0f, 1.0f)]
	public float y;
}


[System.Serializable]
public struct PlayerStateAttribute {
	public string playerStateID;

	[Space (5.0f)]
	[Header ("Value")]
	[Range (0.0f, 100.0f)]
	public float defaultValue;
	[Range (0.0f, 1000000.0f)]
	public float maxValue;

	[Header ("Value Modification")]
	[Range (0.0f, 10.0f)]
	public float defaultIncreaseFactor;
}

[System.Serializable]
public struct PriorityBound {
	public float lowerPriority;
	public float higherPriority;
}

public enum Direction {
	left,
	right,
	up,
	down,
	undirected
}