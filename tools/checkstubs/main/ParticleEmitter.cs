// Solo para la comprobación con Mono: la API pública de Scripts/Port/ParticleEmitter.cs (que usa
// el ParticleSystem de Unity 2019 y no compila contra el UnityEngine 3.5).
using UnityEngine;
public class ParticleEmitter : MonoBehaviour
{
	public bool emit { get { return false; } set { } }
	public float minSize, maxSize, minEnergy, maxEnergy, minEmission, maxEmission;
	public int particleCount { get { return 0; } }
	public void ClearParticles() { }
	public void Emit() { }
	public void Emit(int n) { }
}
