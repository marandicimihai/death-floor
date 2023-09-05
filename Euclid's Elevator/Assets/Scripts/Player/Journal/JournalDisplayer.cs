using DeathFloor.Journal;
using UnityEngine;
using UnityEngine.UI;

namespace DeathFloor.HUD
{
    internal class JournalDisplayer : MonoBehaviour, IJournalDisplayer
    {
        [SerializeField] private float _displayDelay;
        [SerializeField] private Text _leftNumber;
        [SerializeField] private Text _rightNumber;
        [SerializeField] private Image _background;
        [SerializeField] private GameObject _leftPage;
        [SerializeField] private GameObject _rightPage;
        [SerializeField] private Animator _journalAnimator;

        private bool _canDisplay;
        private float _delayTimeElapsed;

        private void Start()
        {
            _journalAnimator.gameObject.SetActive(false);
            CloseJournal();
            Clear();
        }

        private void Update()
        {
            if (_delayTimeElapsed < _displayDelay)
            {
                _delayTimeElapsed += Time.deltaTime;
            }
            else if (_canDisplay)
            {
                _leftPage.SetActive(true);
                _rightPage.SetActive(true);
                _background.gameObject.SetActive(true);
            }
        }

        public void OpenJournal()
        {
            Enable();
            _journalAnimator.gameObject.SetActive(true);
            _journalAnimator.SetBool("Open", true);
        }

        public void CloseJournal()
        {
            Disable();
            _journalAnimator.SetBool("Open", false);
        }

        public void DisplayPages(PageProperties left, PageProperties right, int leftPageIndex, int rightPageIndex)
        {
            Clear();
            if (left != null) Instantiate(left.Prefab, _leftPage.transform);
            if (right != null) Instantiate(right.Prefab, _rightPage.transform);

            _leftNumber.text = leftPageIndex.ToString();
            _rightNumber.text = rightPageIndex.ToString();
        }

        public void Clear()
        {
            for (int i = 0; i < _leftPage.transform.childCount; i++)
            {
                Destroy(_leftPage.transform.GetChild(i).gameObject);
            }
            for (int i = 0; i < _rightPage.transform.childCount; i++)
            {
                Destroy(_rightPage.transform.GetChild(i).gameObject);
            }
            _background.gameObject.SetActive(false);
        }

        public void Disable()
        {
            _canDisplay = false;
            _leftPage.SetActive(false);
            _rightPage.SetActive(false);
            _background.gameObject.SetActive(false);
        }

        public void Enable()
        {
            _canDisplay = true;
            _delayTimeElapsed = 0;
        }
    }
}