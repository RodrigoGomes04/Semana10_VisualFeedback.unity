using UnityEngine;
using UnityEngine.Audio;

public class ProximityFeedback : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Transform jogador;
    [SerializeField] private Renderer objetoRenderer;
    [SerializeField] private ParticleSystem particulas;
    [SerializeField] private AudioClip Sound;
    [SerializeField] private AudioSource audioSource;

    [Header("Ambience")]
    [SerializeField] private AudioClip ambienceSound;
    [SerializeField] private AudioSource ambienceSource;

    [Header("Configuração da proximidade")]
    [SerializeField] private float distanciaAtivacao = 3f;

    [Header("Materiais dos estados")]
    [SerializeField] private Material materialInativo;
    [SerializeField] private Material materialProximo;

    private bool estavaProximo = false;

    void Start()
    {
        if (jogador == null && Camera.main != null)
        {
            jogador = Camera.main.transform;
        }

        if (objetoRenderer == null)
        {
            objetoRenderer = GetComponent<Renderer>();
        }

        if (particulas != null)
        {
            particulas.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );
        }

        AtualizarEstadoVisual(false);

        PlaySource();
    }

    public void Update()
    {
        if (jogador == null)
        {
            return;
        }

        float distancia = Vector3.Distance(
            jogador.position,
            transform.position
        );

        bool estaProximo = distancia <= distanciaAtivacao;

        if (estaProximo != estavaProximo)
        {
            AtualizarEstadoVisual(estaProximo);
            estavaProximo = estaProximo;
        }
    }

    public void AtualizarEstadoVisual(bool proximo)
    {
        if (objetoRenderer != null)
        {
            Material novoMaterial = proximo
                ? materialProximo
                : materialInativo;

            if (novoMaterial != null)
            {
                objetoRenderer.sharedMaterial = novoMaterial;
            }
        }

        if (Sound != null)
        {
            if (proximo)
            {
                audioSource.PlayOneShot(Sound);
            }
            else
            {
                audioSource.Stop();
            }
        }

        if (Sound != null)
        {
            if (estavaProximo == true)
            {
                PlaySource();
            }
            else
            {
                KillSource();
            }
        }

        if (particulas != null)
            {
                if (proximo)
                {
                    particulas.Play();
                }
                else
                {
                    particulas.Stop(
                        true,
                        ParticleSystemStopBehavior.StopEmittingAndClear
                    );
                }
            }
    }
    public void PlaySource()
    {
        ambienceSource.PlayOneShot(ambienceSound);
    }

    public void KillSource()
    {
        ambienceSource.Stop();
    }
}