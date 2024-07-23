using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class ShelfChecker : MonoBehaviour
    {
        [SerializeField] private List<SpotForObject> _spotObjects;

        private void Start()
        {
            GameInstance.ShelfMainMainController.OnPlayersMove += CheckSpots;
        }

        private void OnDestroy()
        {
            GameInstance.ShelfMainMainController.OnPlayersMove -= CheckSpots;
        }

        private void CheckSpots()
        {
            if (_spotObjects.Count < 6)
            {
                return;
            }

            var topSpots = _spotObjects.GetRange(0, 3);
            var bottomSpots = _spotObjects.GetRange(3, 3);

            if (CheckChildrenNames(topSpots))
            {
                RenameChildren(topSpots);
                GameInstance.ShelfMainMainController.IncreaseSliderAndScoreValue();
                GameInstance.ShelfMainMainController.IncreaseMatches();
                GameInstance.ShelfMainMainController.CheckWin();
                GameInstance.Audio.PlayThreeObject();
            }

            if (CheckChildrenNames(bottomSpots))
            {
                RenameChildren(bottomSpots);
                GameInstance.ShelfMainMainController.IncreaseSliderAndScoreValue();
                GameInstance.ShelfMainMainController.IncreaseMatches();
                GameInstance.ShelfMainMainController.CheckWin();
                GameInstance.Audio.PlayThreeObject();
            }
        }

        private bool CheckChildrenNames(List<SpotForObject> spots)
        {
            if (spots.Count == 0) return false;

            string commonName = spots[0].GetChildName();

            if (string.IsNullOrEmpty(commonName)) return false;

            for (int i = 1; i < spots.Count; i++)
            {
                if (spots[i].GetChildName() != commonName)
                {
                    return false;
                }
            }

            return true;
        }
        
        private void RenameChildren(List<SpotForObject> spots)
        {
            for (int i = 0; i < spots.Count; i++)
            {
                if (spots[i].HasChildren)
                {
                    spots[i].transform.GetChild(0).name = $"WIN{i + 1}";
                    spots[i].transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
    }
}