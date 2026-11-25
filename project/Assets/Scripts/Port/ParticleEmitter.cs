using UnityEngine;

// Sustituto del sistema de partículas antiguo de Unity 3 (EllipsoidParticleEmitter +
// ParticleAnimator + ParticleRenderer), que Unity 2018.3 eliminó. Las explosiones, el humo y las
// llamas del juego lo usaban. tools/convert_legacy_particles.py cambia esos tres componentes de
// los prefabs por este, con los mismos parámetros.
//
// La simulación reproduce la del antiguo: emisión continua (o de golpe con oneShot) dentro de un
// elipsoide, velocidad del mundo + local + aleatoria, fuerza constante y aleatoria, tamaño que
// crece a ritmo fijo, rotación, cinco colores a lo largo de la vida y autodestrucción. El
// ParticleSystem moderno solo se usa para dibujar: las partículas se calculan aquí.
// Se llama ParticleEmitter para que el código del juego (GetComponent<ParticleEmitter>().emit,
// .maxEnergy) siga funcionando sin cambios.
public class ParticleEmitter : MonoBehaviour
{
	// --- EllipsoidParticleEmitter
	[SerializeField] private bool m_Emit = true;
	public float minSize = 0.1f;
	public float maxSize = 0.1f;
	public float minEnergy = 3f;
	public float maxEnergy = 3f;
	public float minEmission = 50f;
	public float maxEmission = 50f;
	public Vector3 worldVelocity;
	public Vector3 localVelocity;
	public Vector3 rndVelocity;
	public float emitterVelocityScale = 0.05f;
	public float angularVelocity;
	public float rndAngularVelocity;
	public bool rndRotation;
	public bool useWorldSpace = true;
	public bool oneShot;
	public Vector3 ellipsoid = Vector3.one;
	public float minEmitterRange;
	// --- ParticleAnimator
	public bool doesAnimateColor = true;
	public Color[] colorAnimation = new Color[5] { Color.white, Color.white, Color.white, Color.white, Color.white };
	public float sizeGrow;
	public Vector3 rndForce;
	public Vector3 force;
	public float damping = 1f;
	public bool autodestruct;
	// --- ParticleRenderer
	public Material material;
	public float maxParticleSize = 0.25f;

	private struct P
	{
		public Vector3 pos, vel;
		public float size, rot, angVel, energy, startEnergy;
	}

	private P[] parts = new P[64];
	private int count;
	private float emitAccum;
	private bool burstPending;
	private bool hadParticles;
	private ParticleSystem ps;
	private ParticleSystem.Particle[] buffer = new ParticleSystem.Particle[64];
	private Vector3 lastPos;

	public bool emit
	{
		get { return m_Emit; }
		set
		{
			if (value && !m_Emit && oneShot)
			{
				burstPending = true;
			}
			m_Emit = value;
		}
	}

	public int particleCount
	{
		get { return count; }
	}

	public void ClearParticles()
	{
		count = 0;
	}

	public void Emit()
	{
		Spawn(Mathf.RoundToInt(Random.Range(minEmission, maxEmission)));
	}

	public void Emit(int n)
	{
		Spawn(n);
	}

	private void Awake()
	{
		ps = GetComponent<ParticleSystem>();
		if (ps == null)
		{
			ps = gameObject.AddComponent<ParticleSystem>();
		}
		ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		ParticleSystem.MainModule main = ps.main;
		main.playOnAwake = false;
		main.loop = false;
		main.simulationSpeed = 0f;  // las partículas las mueve este componente
		main.simulationSpace = ParticleSystemSimulationSpace.World;
		main.maxParticles = 1000;
		ParticleSystem.EmissionModule em = ps.emission;
		em.enabled = false;
		ParticleSystem.ShapeModule shape = ps.shape;
		shape.enabled = false;
		ParticleSystemRenderer r = GetComponent<ParticleSystemRenderer>();
		r.renderMode = ParticleSystemRenderMode.Billboard;
		r.maxParticleSize = maxParticleSize;
		if (material != null)
		{
			r.sharedMaterial = material;
		}
		lastPos = transform.position;
		burstPending = m_Emit && oneShot;
	}

	private void OnEnable()
	{
		lastPos = transform.position;
		if (ps != null)
		{
			ps.Play(false);
		}
	}

	private Vector3 Rnd(Vector3 r)
	{
		return new Vector3(Random.Range(-0.5f, 0.5f) * r.x, Random.Range(-0.5f, 0.5f) * r.y, Random.Range(-0.5f, 0.5f) * r.z);
	}

	private void Spawn(int n)
	{
		Transform t = transform;
		Vector3 emitterVel = (t.position - lastPos) / Mathf.Max(Time.deltaTime, 0.0001f);
		for (int k = 0; k < n; k++)
		{
			if (count == parts.Length)
			{
				System.Array.Resize(ref parts, parts.Length * 2);
			}
			Vector3 dir = Random.onUnitSphere * Mathf.Lerp(minEmitterRange, 1f, Mathf.Pow(Random.value, 1f / 3f));
			Vector3 local = Vector3.Scale(dir, ellipsoid * 0.5f);
			P p;
			p.pos = useWorldSpace ? t.TransformPoint(local) : local;
			Vector3 v = worldVelocity + t.TransformDirection(localVelocity) + Rnd(rndVelocity) + emitterVel * emitterVelocityScale;
			p.vel = useWorldSpace ? v : t.InverseTransformDirection(v);
			p.size = Random.Range(minSize, maxSize);
			p.rot = rndRotation ? Random.Range(0f, 360f) : 0f;
			p.angVel = angularVelocity + Random.Range(-rndAngularVelocity, rndAngularVelocity);
			p.energy = p.startEnergy = Mathf.Max(Random.Range(minEnergy, maxEnergy), 0.0001f);
			parts[count++] = p;
		}
	}

	private Color ColorAt(float age01)
	{
		if (!doesAnimateColor || colorAnimation == null || colorAnimation.Length == 0)
		{
			return Color.white;
		}
		float f = Mathf.Clamp01(age01) * (colorAnimation.Length - 1);
		int i = Mathf.Min((int)f, colorAnimation.Length - 2);
		if (i < 0)
		{
			return colorAnimation[0];
		}
		return Color.Lerp(colorAnimation[i], colorAnimation[i + 1], f - i);
	}

	private void LateUpdate()
	{
		float dt = Time.deltaTime;
		if (m_Emit)
		{
			if (oneShot)
			{
				if (burstPending)
				{
					burstPending = false;
					Emit();
				}
			}
			else
			{
				emitAccum += Random.Range(minEmission, maxEmission) * dt;
				int n = (int)emitAccum;
				emitAccum -= n;
				Spawn(n);
			}
		}
		lastPos = transform.position;
		float damp = damping < 1f ? Mathf.Pow(damping, dt * 60f) : 1f;
		for (int i = 0; i < count;)
		{
			P p = parts[i];
			p.energy -= dt;
			if (p.energy <= 0f)
			{
				parts[i] = parts[--count];
				continue;
			}
			p.vel += (force + Rnd(rndForce)) * dt;
			p.vel *= damp;
			p.pos += p.vel * dt;
			p.size = Mathf.Max(0f, p.size + sizeGrow * dt);
			p.rot += p.angVel * dt;
			parts[i] = p;
			i++;
		}
		if (count > 0)
		{
			hadParticles = true;
		}
		if (buffer.Length < count)
		{
			buffer = new ParticleSystem.Particle[parts.Length];
		}
		Transform t = transform;
		for (int i = 0; i < count; i++)
		{
			P p = parts[i];
			ParticleSystem.Particle q = default(ParticleSystem.Particle);
			q.position = useWorldSpace ? p.pos : t.TransformPoint(p.pos);
			q.velocity = Vector3.zero;
			q.startSize = p.size;
			q.rotation = p.rot;
			q.startLifetime = p.startEnergy;
			q.remainingLifetime = p.energy;
			q.startColor = ColorAt(1f - p.energy / p.startEnergy);
			buffer[i] = q;
		}
		ps.SetParticles(buffer, count);
		if (autodestruct && hadParticles && count == 0)
		{
			Object.Destroy(gameObject);
		}
	}
}
