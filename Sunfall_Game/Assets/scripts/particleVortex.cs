using UnityEngine;
using System.Collections;

public class particleVortex : MonoBehaviour {

	private ParticleSystem particleSystem;
	private ParticleSystem.Particle[] m_Particles;
	private ParticleSystem.ShapeModule shape;

	public float maxVelocity;

	public AnimationCurve growCurve;
	public float growSpeed;
	private float t = 0f;

	// Use this for initialization
	void Awake () {
		particleSystem = GetComponent<ParticleSystem> ();
		shape = particleSystem.shape;
	}

	// Update is called once per frame
	void LateUpdate () {
		InitializeIfNeeded ();

		if (t <= 1) {
			shape.radius = growCurve.Evaluate (t);
			t += Time.deltaTime*growSpeed;
		}

		int numParticlesAlive = particleSystem.GetParticles (m_Particles);
		for (int i = 0; i < numParticlesAlive; i++) {
			ParticleSystem.Particle p = m_Particles [i];//m_Particles [i].axisOfRotation = transform.position;

			float vel = 800f;// Mathf.Lerp (maxVelocity, 0f, m_Particles [i].lifetime / m_Particles [i].startLifetime);
			//m_Particles [i].angularVelocity3D = new Vector3(0f, vel, 0f);
			Vector3 perpendicular = Vector3.Cross (Vector3.up, transform.position-p.position);
			m_Particles [i].velocity = Vector3.Lerp (transform.position-p.position, perpendicular, 0.9f).normalized*(Mathf.Lerp(maxVelocity,0.01f,p.remainingLifetime/p.startLifetime));
		}

		particleSystem.SetParticles (m_Particles, numParticlesAlive);

	}

	void InitializeIfNeeded (){
		if (particleSystem == null) {

		}
		if (m_Particles == null || m_Particles.Length < particleSystem.maxParticles) {
			m_Particles = new ParticleSystem.Particle[particleSystem.maxParticles];
		}
	}
}
