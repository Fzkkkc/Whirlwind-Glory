using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public class FXController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _winFX1;
        [SerializeField] private ParticleSystem _winFX2;
        [SerializeField] private ParticleSystem _winFX3;
        [SerializeField] private ParticleSystem _winShowerFX;
        [SerializeField] private ParticleSystem _starStreamFX;
        
        [SerializeField] private ParticleSystem _energyFX;
        [SerializeField] private ParticleSystem _bombFX;
        
        [SerializeField] private ParticleSystem _loseFX3;
        
        [SerializeField] private List<ParticleSystem> _particleSystems;
        [SerializeField] private List<ParticleSystem> _backgroundParticleSystems;
        [SerializeField] private List<ParticleSystem> _bgParticleGame;
        
        public void Init()
        {
            DisableParticles();
        }
        
        public void PlayBackgroundParticleMenu()
        {
            foreach (var particle in _backgroundParticleSystems)
            {
                particle.gameObject.SetActive(true);
            }
        }
        
        public void DisableBackgroundParticleMenu()
        {
            foreach (var particle in _backgroundParticleSystems)
            {
                particle.gameObject.SetActive(false);
            }
        }
        
        public void PlayBackgroundParticleGame()
        {
            foreach (var particle in _bgParticleGame)
            {
                particle.gameObject.SetActive(true);
            }
        }
        
        public void DisableBackgroundParticleGame()
        {
            foreach (var particle in _bgParticleGame)
            {
                particle.gameObject.SetActive(false);
            }
        }
        
        public void PlayWinFX()
        {
            _winFX1.gameObject.SetActive(true);
            _winFX1.Play();
            _winFX2.gameObject.SetActive(true);
            _winFX2.Play();
            _winFX3.gameObject.SetActive(true);
            _winFX3.Play();
            _winShowerFX.gameObject.SetActive(true);
            _winShowerFX.Play();
        }

        public void DisableWinShower()
        {
            _winShowerFX.gameObject.SetActive(false);
            _winShowerFX.Stop();
        }

        public void PlayStarStream()
        {
            _starStreamFX.gameObject.SetActive(true);
            _starStreamFX.Play();
        }
        
        public void PlayLoseFX()
        {
            _loseFX3.gameObject.SetActive(true);
            _loseFX3.Play();
        }
        
        public void PlayBombFX()
        {
            _bombFX.gameObject.SetActive(true);
            _bombFX.Play();
        }
        
        public void PlayEnergyFX()
        {
            _energyFX.gameObject.SetActive(true);
            _energyFX.Play();
        }
        
        public void StopLoseFX()
        {
            _loseFX3.gameObject.SetActive(false);
            _loseFX3.Stop();
        }
        
        public void DisableParticles()
        {
            foreach (var particle in _particleSystems)
            {
                particle.gameObject.SetActive(false);
            }
        }
    }
}