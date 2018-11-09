using UnityEngine;
using UnityEngine.UI;

public class SimpleRotatorSlider : MonoBehaviour {

    public SimpleRotator rotator;
    private Slider slider;

	// Use this for initialization
	private void Start () {
        this.slider = this.GetComponent<Slider>();
        this.slider.value = this.rotator.rotationSpeed / 360.0f;
	}
	
	public void UpdateRotationSpeed()
    {
        this.rotator.rotationSpeed = this.slider.value * 360.0f;
    }
}
