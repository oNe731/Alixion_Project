using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    [SerializeField] private Slider m_bgmSlider;
    [SerializeField] private Slider m_effectSlider;

    private float m_bgmSound = 0.5f;
    private float m_effectSound = 0.5f;
    private float m_previousBgmSound;
    private float m_previousEffectSound;

    public float BgmSound { get => m_bgmSound; }
    public float EffectSound { get => m_effectSound; }

    private void Start()
    {
        m_bgmSlider.value    = m_bgmSound;
        m_effectSlider.value = m_effectSound;

        m_previousBgmSound    = m_bgmSound;
        m_previousEffectSound = m_effectSound;

        Update_AllAudioSources();
    }

    private void Update()
    {
        if (!m_bgmSlider.gameObject.activeSelf || !m_effectSlider.gameObject.activeSelf)
            return;

        m_bgmSound    = m_bgmSlider.value;
        m_effectSound = m_effectSlider.value;

        if (m_bgmSound != m_previousBgmSound || m_effectSound != m_previousEffectSound)
        {
            m_previousBgmSound    = m_bgmSound;
            m_previousEffectSound = m_effectSound;

            Update_AllAudioSources();
        }
    }

    public void Update_AllAudioSources()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (var audioSource in allAudioSources)
        {
            if (audioSource.gameObject.name.Contains("Main Camera"))
                audioSource.volume = m_bgmSound;
            else
                audioSource.volume = m_effectSound;
        }
    }
}