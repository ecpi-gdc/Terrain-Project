using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour {
	private AudioSource audioPlayer;

	// Start is called before the first frame update
	void Start() {
		audioPlayer = GetComponent<AudioSource>();

	}

	// Update is called once per frame
	void Update() { }

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("MainCamera")) {
			print("Played!");
			audioPlayer.PlayOneShot(audioPlayer.clip);
		}
	}

}