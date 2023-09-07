using DeathFloor.HUD;
using DeathFloor.Input;
using DeathFloor.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace DeathFloor.Journal
{
    internal class JournalManager : MonoBehaviour, IJournalManager
    {
        [SerializeField] private InputReader _inputReader;
        [SerializeField, RequireInterface(typeof(IHUDManager))] private Object _hudManager;
        [SerializeField, RequireInterface(typeof(IJournalDisplayer))] private Object _journalDisplayer;

        private IHUDManager _managerInterface;
        private IJournalDisplayer _displayer;

        private bool _isOpen;

        private List<PageProperties> _pages;
        private int _currentPage = 1;//1, 3, 5 (left page basically, substract 1 for index)

        private void Start()
        {
            _pages ??= new();
            
            _managerInterface = _hudManager as IHUDManager;
            _displayer = _journalDisplayer as IJournalDisplayer;

            _inputReader.JournalToggled += ToggleJournal;
            _inputReader.PauseToggled += CloseJournal;
            _inputReader.PageLeft += PreviousPage;
            _inputReader.PageRight += NextPage;
        }

        private void OnDisable()
        {
            _inputReader.JournalToggled -= ToggleJournal;
            _inputReader.PauseToggled -= CloseJournal;
            _inputReader.PageLeft -= PreviousPage;
            _inputReader.PageRight -= NextPage;
        }

        private void NextPage()
        {
            if (_currentPage <= _pages.Count - 2)
            {
                _currentPage += 2;
                if (_currentPage < _pages.Count)
                {
                    _displayer.DisplayPages(_pages[_currentPage - 1], _pages[_currentPage], _currentPage, _currentPage + 1);
                }
                else if (_currentPage - 1 < _pages.Count)
                {
                    _displayer.DisplayPages(_pages[_currentPage - 1], null, _currentPage, _currentPage + 1);
                }
            }
        }

        private void PreviousPage()
        {
            if (_currentPage >= 3)
            {
                _currentPage -= 2;
                if (_currentPage + 1 < _pages.Count)
                {
                    _displayer.DisplayPages(_pages[_currentPage - 1], _pages[_currentPage], _currentPage, _currentPage + 1);
                }
                else if (_currentPage < _pages.Count)
                {
                    _displayer.DisplayPages(_pages[_currentPage - 1], null, _currentPage, _currentPage + 1);
                }
            }
        }

        private void ToggleJournal()
        {
            if (_isOpen)
            {
                _displayer.CloseJournal();
                _managerInterface.EnableHUD();
                _inputReader.DefaultInput();
                _isOpen = false;
            }
            else
            {
                _managerInterface.DisableHUD();

                _inputReader.PauseInput();
                _displayer.OpenJournal();
                if (_currentPage + 1 < _pages.Count)
                {
                    _displayer.DisplayPages(_pages[_currentPage - 1], _pages[_currentPage], _currentPage, _currentPage + 1);
                }
                else if (_currentPage < _pages.Count)
                {
                    _displayer.DisplayPages(_pages[_currentPage - 1], null, _currentPage, _currentPage + 1);
                }
                else
                {
                    _displayer.DisplayPages(null, null, 1, 2);
                }

                _isOpen = true;
            }
        }

        private void CloseJournal()
        {
            if (_isOpen)
            {
                _inputReader.DefaultInput();
                _displayer.CloseJournal();
                _isOpen = false;
            }
        }

        public void AddPage(PageProperties page)
        {
            if (!_pages.Contains(page))
            {
                _pages.Add(page);
            }
        }

        public void Disable()
        {
            _displayer.Disable();
        }

        public void Enable()
        {
            if (_isOpen)
            {
                _displayer.Enable();
            }
        }
    }
}