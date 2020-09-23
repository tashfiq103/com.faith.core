namespace com.faith.core
{
	using System;
	using UnityEngine;


    public static class MathFunction
	{
		public static Vector2 ConvertUnitVectorToVector(UnitVector2D t_UnitVector)
		{

			Vector2 t_Result = new Vector2(
				t_UnitVector.x,
				t_UnitVector.y
			);

			return t_Result;
		}

		public static Vector3 ConvertUnitVectorToVector(UnitVector t_UnitVector)
		{

			Vector3 t_Result = new Vector3(
				t_UnitVector.x,
				t_UnitVector.y,
				t_UnitVector.z
			);

			return t_Result;
		}

		public static Vector3 GetUnitVector(Vector3 startingPosition, Vector3 targetPosition)
		{

			Vector3 m_UnitVector = Vector3.zero;

			Vector3 m_Difference = new Vector3(
				targetPosition.x - startingPosition.x,
				targetPosition.y - startingPosition.y,
				targetPosition.z - startingPosition.z
			);

			float m_TotalDistance = Mathf.Abs(m_Difference.x) + Mathf.Abs(m_Difference.y) + Mathf.Abs(m_Difference.z);

			if (m_TotalDistance == 0f)
			{

				m_UnitVector = Vector2.zero;
			}
			else
			{

				m_UnitVector = new Vector3(
					m_Difference.x / m_TotalDistance,
					m_Difference.y / m_TotalDistance,
					m_Difference.z / m_TotalDistance
				);
			}

			return m_UnitVector;
		}

		public static float GetRotationInDegree(Vector2 t_DirectionVector)
		{

			float t_Result = 0.0f;

			if (t_DirectionVector.x > t_DirectionVector.y)
			{

				if (t_DirectionVector.y >= 0f)
				{

					t_Result = Vector2.Angle(t_DirectionVector, Vector2.right);
				}
				else
				{

					t_Result = 360.0f - Vector2.Angle(t_DirectionVector, Vector2.right);
				}
			}
			else
			{

				if (t_DirectionVector.y >= 0f)
				{

					t_Result = Vector2.Angle(t_DirectionVector, Vector2.right);
				}
				else
				{

					t_Result = 360.0f - Vector2.Angle(t_DirectionVector, Vector2.right);
				}
			}

			return t_Result;
		}

		public static float GetRotationInRadian(Vector2 t_DirectionVector)
		{

			return ((GetRotationInDegree(t_DirectionVector) * Mathf.PI) / 180.0f);
		}

		public static float SmoothTransitionCurve(float t_Value)
		{
			return SmoothTransitionCurve(t_Value, 0.66f, 0.33f, 0f);
		}

		public static float SmoothTransitionCurve(float t_Value, float t_ValueBoost)
		{
			return SmoothTransitionCurve(t_Value, 0.66f, 0.33f, t_ValueBoost);
		}

		public static float SmoothTransitionCurve(float t_Value, float t_SaturationPoint, float t_ValueTillSaturationPoint, float t_ValueBoost)
		{

			float t_Result = 0f;

			t_Value += t_ValueBoost;
			t_Value = t_Value > 1f ? 1f : t_Value;

			if (t_Value <= t_SaturationPoint)
			{
				t_Result = Mathf.Lerp(
						0f,
						t_ValueTillSaturationPoint,
						(t_Value / t_SaturationPoint)
					);
			}
			else
			{
				float t_Divider = 1f - t_SaturationPoint;
				t_Result = Mathf.Lerp(
						t_ValueTillSaturationPoint,
						1f,
						(t_Value - t_SaturationPoint) / t_Divider
					);
			}

			return t_Result;
		}


		public static PriorityBound[] GetPriorityBound(float[] priorityList)
		{

			PriorityBound[] t_Result = new PriorityBound[priorityList.Length];

			float[] t_SortedPriorityList = new float[priorityList.Length];
			System.Array.Copy(priorityList, t_SortedPriorityList, t_SortedPriorityList.Length);

			float t_TotalPriority = 0.0f;
			for (int index = 0; index < t_SortedPriorityList.Length; index++)
				t_TotalPriority += t_SortedPriorityList[index];

			System.Array.Sort(t_SortedPriorityList);

			bool[] t_IsFoundMatch = new bool[priorityList.Length];

			for (int pointerIndex = 0; pointerIndex < priorityList.Length; pointerIndex++)
			{

				for (int traverseIndex = 0; traverseIndex < t_SortedPriorityList.Length; traverseIndex++)
				{

					if (!t_IsFoundMatch[traverseIndex] &&
						priorityList[pointerIndex] == t_SortedPriorityList[traverseIndex])
					{

						t_IsFoundMatch[traverseIndex] = true;

						float t_LowerBound = 0.0f;
						for (int calculatedIndex = t_SortedPriorityList.Length - 1; calculatedIndex > traverseIndex; calculatedIndex--)
						{
							t_LowerBound += t_SortedPriorityList[calculatedIndex];
						}

						t_Result[pointerIndex].lowerPriority = t_LowerBound / t_TotalPriority;
						t_Result[pointerIndex].higherPriority = (t_LowerBound + priorityList[pointerIndex]) / t_TotalPriority;

						break;
					}
				}
			}
			return t_Result;
		}

		public static float GetTangent(
				Vector2 t_Point1,
				Vector2 t_Point2)
		{

			return (t_Point2.y - t_Point1.y) / (t_Point2.x - t_Point1.x);
		}

		public static Vector2 GetLineEquationOfTwoPoint(
			Vector2 t_Point1, 
			Vector2 t_Point2, 
			bool t_UsePoint1ForSlopeCalculation = true) {

			float x = t_UsePoint1ForSlopeCalculation ? t_Point1.x : t_Point2.x;
			float y = t_UsePoint1ForSlopeCalculation ? t_Point1.y : t_Point2.y;

			float t_Tangent		= GetTangent(t_Point1, t_Point2);
			return new Vector2(
					((t_Tangent * x) - y) / t_Tangent,
					y - (t_Tangent * x)
				);

		}

		public static Vector2 GetPerpendiculerLineEquationAtCertainPoint(
			Vector2 t_Point1,
			Vector2 t_Point2,
			Vector2 t_IntersectedPoint) {

			float t_InverseSlope	= -1 / GetTangent(t_Point1, t_Point2);

			return new Vector2(
					((t_InverseSlope * t_IntersectedPoint.x) - t_IntersectedPoint.y) / t_InverseSlope,
					t_IntersectedPoint.y - (t_InverseSlope * t_IntersectedPoint.x)
				);

		}

		/// <summary>
		/// if return 0		: The point is on the line
		/// if return 1		: The points are on the same side
		/// if return -1	: The points are on the other side
		/// </summary>
		/// <param name="t_Point1ForLine"></param>
		/// <param name="t_Point2ForLine"></param>
		/// <param name="t_Point1"></param>
		/// <param name="t_Point2"></param>
		/// <returns></returns>
		public static int IsTwoPointOnTheSameSide(
				Vector2 t_Point1ForLine,
				Vector2 t_Point2ForLine,
				Vector2 t_Point1,
				Vector2 t_Point2
			) {


			Vector2 t_LineEquation = GetLineEquationOfTwoPoint(t_Point1ForLine, t_Point2ForLine);
			float t_CheckForPoint1 = (t_Point1.x / t_LineEquation.x) + (t_Point1.y / t_Point1ForLine.y) - 1;
			float t_CheckForPoint2 = (t_Point2.x / t_LineEquation.x) + (t_Point2.y / t_Point1ForLine.y) - 1;

			float t_Multiply = t_CheckForPoint1 * t_CheckForPoint2;
			
			if (t_Multiply == 0)
				return 0;
			else if (t_Multiply > 0)
				return 1;

			return -1;
		}

		public static bool IsTwoLineIntersected(
				Vector2 p1,
				Vector2 p2,
				Vector2 p3,
				Vector2 p4
			) {

			float dx12 = p2.x - p1.x;
			float dy12 = p2.y - p1.y;
			float dx34 = p4.x - p3.x;
			float dy34 = p4.y - p3.y;

			float denominator = (dy12 * dx34) - (dx12 * dy34);
			float t1 = ((p1.x - p3.x) * dy34 + (p3.y - p1.y) * dx34) / denominator;

			if (t1 == Mathf.Infinity)
				return false;

			float t2 = ((p3.x - p1.x) * dy12 + (p1.y - p3.y) * dx12) / (-denominator);

			if ((t1 >= 0 && t1 <= 1) && (t2 >= 0 && t2 <= 1))
				return true;

			return false;
		}

		public static float GetAreaOfTriangle(
			Vector2 t_Vertex1, 
			Vector2 t_Vertex2, 
			Vector2 t_Vertex3) {

			return Math.Abs( (t_Vertex1.x * (t_Vertex2.y - t_Vertex3.y)) + (t_Vertex2.x * (t_Vertex3.y - t_Vertex1.y)) + (t_Vertex3.x * (t_Vertex1.y - t_Vertex2.y)) ) / 2.0f;
		}

		public static bool IsPointInsideTriangle(
			Vector2 t_Vertex1,
			Vector2 t_Vertex2,
			Vector2 t_Vertex3,
			Vector2 t_Point,
			float t_ErrorRate = 0.1f)
		{

			float t_AreaOfTriangle			= GetAreaOfTriangle(t_Vertex1, t_Vertex2, t_Vertex3);
			float t_AreaOfTrinagleOn1To2	= GetAreaOfTriangle(t_Point, t_Vertex2, t_Vertex3);
			float t_AreaOfTrinagleOn2To3	= GetAreaOfTriangle(t_Vertex1, t_Point, t_Vertex3);
			float t_AreaOfTrinagleOn3To1	= GetAreaOfTriangle(t_Vertex1, t_Vertex2, t_Point);

			float t_AbsoluteErrorRate				= t_AreaOfTriangle * t_ErrorRate;
			float t_SummationOfPredictionTriangle	= t_AreaOfTrinagleOn1To2 + t_AreaOfTrinagleOn2To3 + t_AreaOfTrinagleOn3To1;

			if (t_AreaOfTriangle >= (t_SummationOfPredictionTriangle - t_AbsoluteErrorRate) && t_AreaOfTriangle <= (t_SummationOfPredictionTriangle + t_AbsoluteErrorRate))
				return true;


			return false;
		}

		#region Public Callback	:	Currency Formation

		public static string GetCurrencyInFormatInNonDecimal(double t_AmountOfCurrency)
		{

			string t_Result = "";
			if (t_AmountOfCurrency >= 1000000000000000000)
			{
				t_Result = Convert.ToDouble((t_AmountOfCurrency / 1000000000000000000)).ToString("D0") + "Qt";
			}
			else if (t_AmountOfCurrency >= 1000000000000000)
			{
				t_Result = Convert.ToDouble((t_AmountOfCurrency / 1000000000000000)).ToString("D0") + "Qd";
			}
			else if (t_AmountOfCurrency >= 1000000000000)
			{
				t_Result = Convert.ToDouble((t_AmountOfCurrency / 1000000000000)).ToString("D0") + "T";
			}
			else if (t_AmountOfCurrency >= 1000000000)
			{
				t_Result = Convert.ToDouble((t_AmountOfCurrency / 1000000000)).ToString("D0") + "B";
			}
			else if (t_AmountOfCurrency >= 1000000)
			{
				t_Result = Convert.ToDouble((t_AmountOfCurrency / 1000000)).ToString("D0") + "M";
			}
			else if (t_AmountOfCurrency >= 1000)
			{
				t_Result = Convert.ToDouble((t_AmountOfCurrency / 1000)).ToString("D0") + "K";
			}
			else if (t_AmountOfCurrency == 0)
			{
				t_Result = "0";
			}
			else
			{
				t_Result = t_AmountOfCurrency.ToString("F0");
			}

			return t_Result;
		}

		public static string GetCurrencyInFormat(double t_AmountOfCurrency)
		{

			string t_Result = "";
			if (t_AmountOfCurrency >= 1000000000000000000)
			{
				t_Result = Convert.ToDouble((t_AmountOfCurrency / 1000000000000000000)).ToString("#.00") + "Qt";
			}
			else if (t_AmountOfCurrency >= 1000000000000000)
			{
				t_Result = Convert.ToDouble((t_AmountOfCurrency / 1000000000000000)).ToString("#.00") + "Qd";
			}
			else if (t_AmountOfCurrency >= 1000000000000)
			{
				t_Result = Convert.ToDouble((t_AmountOfCurrency / 1000000000000)).ToString("#.00") + "T";
			}
			else if (t_AmountOfCurrency >= 1000000000)
			{
				t_Result = Convert.ToDouble((t_AmountOfCurrency / 1000000000)).ToString("#.00") + "B";
			}
			else if (t_AmountOfCurrency >= 1000000)
			{
				t_Result = Convert.ToDouble((t_AmountOfCurrency / 1000000)).ToString("#.00") + "M";
			}
			else if (t_AmountOfCurrency >= 1000)
			{
				t_Result = Convert.ToDouble((t_AmountOfCurrency / 1000)).ToString("#.00") + "K";
			}
			else if (t_AmountOfCurrency == 0)
			{
				t_Result = "0";
			}
			else
			{
				t_Result = t_AmountOfCurrency.ToString("#.00");
			}

			return t_Result;
		}

		#endregion

		#region Public Callback	:	Color Correction

		public static float GetColorIntensityBasedOnContrast(
				Color t_ColorValue,
				float t_MaxIntensity,
				float t_Variation = 0.5f
			) {

			float t_ContrastLevel = ((t_ColorValue.r * 299) + (t_ColorValue.g * 587) + (t_ColorValue.b * 114)) / 1000f;
			float t_LowerBoundOfIntensity = (1 - t_Variation) * t_MaxIntensity;
			float t_FixedIntensity = Mathf.Lerp(
					t_LowerBoundOfIntensity,
					t_MaxIntensity,
					SmoothTransitionCurve((1f - t_ContrastLevel), 0.33f, 0.66f,0f)
				);

			return t_FixedIntensity;
		}

		#endregion

		#region Obsolute

		//#region Configuretion	:	CameraTranslation

		//private Camera m_CameraReference;
		//private Vector2 m_CameraPosition;
		//private float m_CameraOrthographicSize;

		//private Vector2 m_ViewBoundary;

		//private void PreprocessForTranslationByCamera()
		//{

		//	m_CameraReference = Camera.main;
		//}

		//#endregion

		// #region Public Callback	:	Camera Translation

		// public bool IsHitBoundary(Vector2 t_RequestedPosition, UnitVector2D t_LowerBoundOfView, UnitVector2D t_UpperBoundOfView)
		// {

		// 	bool t_Result = false;

		// 	m_CameraPosition = (Vector2)m_CameraReference.transform.position;
		// 	m_CameraOrthographicSize = m_CameraReference.orthographicSize;

		// 	if (DeviceInfoManager.Instance.IsPortraitMode())
		// 	{

		// 		m_ViewBoundary = new Vector2(
		// 			m_CameraPosition.x + (m_CameraOrthographicSize / DeviceInfoManager.Instance.GetAspectRatioFactor()),
		// 			m_CameraPosition.y + m_CameraOrthographicSize
		// 		);
		// 	}
		// 	else
		// 	{

		// 		m_ViewBoundary = new Vector2(
		// 			m_CameraPosition.x + (m_CameraOrthographicSize * DeviceInfoManager.Instance.GetAspectRatioFactor()),
		// 			m_CameraPosition.y + m_CameraOrthographicSize
		// 		);
		// 	}

		// 	float t_BoundaryLimitOnAxis = m_ViewBoundary.x * t_LowerBoundOfView.x;
		// 	if (t_BoundaryLimitOnAxis > t_RequestedPosition.x)
		// 	{
		// 		//Negetive : X-Axis
		// 		t_Result = true;
		// 	}

		// 	t_BoundaryLimitOnAxis = m_ViewBoundary.y * t_LowerBoundOfView.y;
		// 	if (t_BoundaryLimitOnAxis > t_RequestedPosition.y)
		// 	{
		// 		//Negetive : Y-Axis
		// 		t_Result = true;
		// 	}

		// 	t_BoundaryLimitOnAxis = m_ViewBoundary.x * t_UpperBoundOfView.x;
		// 	if (t_RequestedPosition.x > t_BoundaryLimitOnAxis)
		// 	{
		// 		//Positive : X-Axis
		// 		t_Result = true;
		// 	}

		// 	t_BoundaryLimitOnAxis = m_ViewBoundary.y * t_UpperBoundOfView.y;
		// 	if (t_RequestedPosition.y > t_BoundaryLimitOnAxis)
		// 	{
		// 		//Positive : Y-Axis
		// 		t_Result = true;
		// 	}

		// 	return t_Result;
		// }

		// public Vector2 TranslationByBoundedCameara(Vector2 t_RequestedPosition, UnitVector2D t_LowerBoundOfView, UnitVector2D t_UpperBoundOfView)
		// {

		// 	m_CameraPosition = (Vector2)m_CameraReference.transform.position;
		// 	m_CameraOrthographicSize = m_CameraReference.orthographicSize;

		// 	if (DeviceInfoManager.Instance.IsPortraitMode())
		// 	{

		// 		m_ViewBoundary = new Vector2(
		// 			m_CameraPosition.x + (m_CameraOrthographicSize / DeviceInfoManager.Instance.GetAspectRatioFactor()),
		// 			m_CameraPosition.y + m_CameraOrthographicSize
		// 		);
		// 	}
		// 	else
		// 	{

		// 		m_ViewBoundary = new Vector2(
		// 			m_CameraPosition.x + (m_CameraOrthographicSize * DeviceInfoManager.Instance.GetAspectRatioFactor()),
		// 			m_CameraPosition.y + m_CameraOrthographicSize
		// 		);
		// 	}

		// 	float t_BoundaryLimitOnAxis = m_ViewBoundary.x * t_LowerBoundOfView.x;
		// 	if (t_BoundaryLimitOnAxis > t_RequestedPosition.x)
		// 	{
		// 		//Negetive : X-Axis
		// 		t_RequestedPosition = new Vector2(
		// 			t_BoundaryLimitOnAxis,
		// 			t_RequestedPosition.y
		// 		);
		// 	}

		// 	t_BoundaryLimitOnAxis = m_ViewBoundary.y * t_LowerBoundOfView.y;
		// 	if (t_BoundaryLimitOnAxis > t_RequestedPosition.y)
		// 	{
		// 		//Negetive : Y-Axis
		// 		t_RequestedPosition = new Vector2(
		// 			t_RequestedPosition.x,
		// 			t_BoundaryLimitOnAxis
		// 		);
		// 	}

		// 	t_BoundaryLimitOnAxis = m_ViewBoundary.x * t_UpperBoundOfView.x;
		// 	if (t_RequestedPosition.x > t_BoundaryLimitOnAxis)
		// 	{
		// 		//Positive : X-Axis
		// 		t_RequestedPosition = new Vector2(
		// 			t_BoundaryLimitOnAxis,
		// 			t_RequestedPosition.y
		// 		);
		// 	}

		// 	t_BoundaryLimitOnAxis = m_ViewBoundary.y * t_UpperBoundOfView.y;
		// 	if (t_RequestedPosition.y > t_BoundaryLimitOnAxis)
		// 	{
		// 		//Positive : Y-Axis
		// 		t_RequestedPosition = new Vector2(
		// 			t_RequestedPosition.x,
		// 			t_BoundaryLimitOnAxis
		// 		);
		// 	}

		// 	return t_RequestedPosition;
		// }


		// #endregion

		#endregion
	}
}

