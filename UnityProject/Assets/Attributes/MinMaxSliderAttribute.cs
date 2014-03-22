using UnityEngine;

public class MinMaxSliderAttribute : PropertyAttribute  
{
	public float minValue;
	public float maxValue;
	public float minLimit;
	public float maxLimit;
	
	public MinMaxSliderAttribute (float minValue, float maxValue, float minLimit, float maxLimit) 
	{
		this.minValue = minValue;
		this.maxValue = maxValue;
		this.minLimit = minLimit;
		this.maxLimit = maxLimit;
	}
}
