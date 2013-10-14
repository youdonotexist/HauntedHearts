using UnityEngine;
using System.Collections;

public class HeartController : MonoBehaviour
{
	public PlayerControl _player;
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
			if (_player.SpiritOut()) {
				curHealth -= 0.1f * Time.deltaTime;	
			}
			else {
				curHealth -= 0.1f * Time.deltaTime;
				curHealth = Mathf.Max(curHealth, 0.20f);
				if (curHealth <= 0.20f) {
					curHealth = 0.25f;
					audio.PlayOneShot(audio.clip);
				}
			}
		}
		
		if (_player.SpiritOut() == false && curHealth > 0.6f) {
			_player.SpawnSpirit();	
		}
		
		_progressBarMaterial.SetFloat("_Cutoff", Mathf.Abs (1.0f - curHealth));
		
		if (curHealth <= 0.0f || curHealth >= 1.0f) {
			_player.DestroySpirit();
		}
	}
	
	public void SetAmt(float amt) {
		_progressBarMaterial.SetFloat("_Cutoff", Mathf.Abs (1.0f - amt));
	}
}

