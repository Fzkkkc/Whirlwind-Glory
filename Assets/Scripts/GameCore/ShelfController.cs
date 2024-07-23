using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameCore
{
    public class ShelfController : MonoBehaviour
    {
        [System.Serializable]
        public class Shelf
        {
            public List<Transform> Spots;
        }

        [SerializeField] private bool _isBigShelf;
        
        [SerializeField] private List<Shelf> _shelves;
        [SerializeField] private List<GameObject> _itemPrefabs; 

        private List<Transform> _allSpots = new List<Transform>();

        private void Start()
        {
            if (_isBigShelf)
                GameInstance.ShelfMainMainController.OnBigShelfGameStarted += StartGame;
            else
                GameInstance.ShelfMainMainController.OnSmallShelfGameStarted += StartGame;
        }

        private void OnDestroy()
        {
            if (_isBigShelf)
                GameInstance.ShelfMainMainController.OnBigShelfGameStarted -= StartGame;
            else
                GameInstance.ShelfMainMainController.OnSmallShelfGameStarted -= StartGame;
        }

        private void StartGame()
        {
            InitializeAllSpots();
            RemoveAllChildObjects();
            SpawnItems();
        }

        private void InitializeAllSpots()
        {
            _allSpots.Clear();
            foreach (var shelf in _shelves)
            {
                _allSpots.AddRange(shelf.Spots);
            }
        }

        private void SpawnItems()
        {
            var randomObjectsCount = _isBigShelf ? Random.Range(10, 14) : Random.Range(7, 11);
            
            var usedSpots = new List<int>();
            var availablePrefabs = new List<GameObject>(_itemPrefabs);
        
            var totalSpawnedObjects = 20;
            
            for (var i = 0; i < randomObjectsCount; i++)
            {
                var randomIndex = Random.Range(0, availablePrefabs.Count);
                var itemPrefab = availablePrefabs[randomIndex];
                availablePrefabs.RemoveAt(randomIndex);

                for (var j = 0; j < 3; j++)
                {
                    var spotIndex = GetRandomSpotIndex(usedSpots);
                    var spot = _allSpots[spotIndex];

                    Instantiate(itemPrefab, spot.position, spot.rotation, spot);
                    totalSpawnedObjects++;
                }
            }

            GameInstance.ShelfMainMainController.SetPlayerMoves(totalSpawnedObjects);
        }

        private int GetRandomSpotIndex(List<int> usedSpots)
        {
            int spotIndex;
        
            do
            {
                spotIndex = Random.Range(0, _allSpots.Count);
            } while (usedSpots.Contains(spotIndex));

            usedSpots.Add(spotIndex);

            return spotIndex;
        }
        
        private void RemoveAllChildObjects()
        {
            foreach (var spot in _allSpots)
            {
                foreach (Transform child in spot)
                {
                    Destroy(child.gameObject);
                }
            }
        }
        
        public void RemoveTriosWithSameName()
        {
            var nameCounts = new Dictionary<string, List<Transform>>();
            
            foreach (var spot in _allSpots)
            {
                foreach (Transform child in spot)
                {
                    string name = child.name;

                    if (!nameCounts.ContainsKey(name))
                    {
                        nameCounts[name] = new List<Transform>();
                    }
                    nameCounts[name].Add(child);
                }
            }
            
            foreach (var entry in nameCounts)
            {
                if (entry.Value.Count >= 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Destroy(entry.Value[i].gameObject);
                    }
                    break; 
                }
            }
            
            GameInstance.ShelfMainMainController.IncreaseSliderAndScoreValue();
            GameInstance.ShelfMainMainController.IncreaseMatches();
            GameInstance.ShelfMainMainController.CheckWin();
            GameInstance.FXController.PlayBombFX();
        }
    }
}