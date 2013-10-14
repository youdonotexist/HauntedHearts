using UnityEngine;
using System.Collections;

public class TitleScreenControls : MonoBehaviour
{
	Material _progressBarMaterial;
	
	float health = 1.0f;
	
	// Use this for initialization
	void Start ()
	{
		_progressBarMaterial = renderer.material;
	}
	
	void Update() {
		float curHealth = Mathf.Abs (1.0f - _progressBarMaterial.GetFloat("_Cutoff"));
		
		if (Input.GetKeyDown(KeyCode.Space)) {
			curHealth += 0.1f;
			audio.PlayOneShot(audio.clip);
		}
		else {
			curHealth -= 0.1f * Time.deltaTime;
			curHealth = Mathf.Max(curHealth, 0.20f);
			if (curHealth <= 0.20f) {
				curHealth = 0.25f;
				audio.PlayOneShot(audio.clip);
			}
			
			
				
		}
		
		if (curHealth > 0.6f) {
			Application.LoadLevel("Scene01");
		}
		
		_progressBarMaterial.SetFloat("_Cutoff", Mathf.Abs (1.0f - curHealth));
	}
	
	public void SetAmt(float amt) {
		_progressBarMaterial.SetFloat("_Cutoff", Mathf.Abs (1.0f - amt));
	}
	
}

