using UnityEngine;
using System.Collections;

public class Girl : MonoBehaviour
{
	float waitForGiggle = 5.0f;
	float elapsed = 5.1f;
	exSpriteAnimation _animation;
	
	void Start() {
		_animation = GetComponent<exSpriteAnimation>();	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (elapsed > waitForGiggle + 2.0f) {
			_animation.Play ("GirlStand");	
			elapsed = 0.0f;
		}
		else if (elapsed > waitForGiggle) {
			if (!_animation.IsPlaying("GirlGiggle")) {
				audio.PlayOneShot(audio.clip);
				_animation.Play ("GirlGiggle");	
			}
		}
		
		elapsed += Time.deltaTime;
	}
}

