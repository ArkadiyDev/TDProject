using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIService : IUIService
    {
        private readonly Dictionary<Type, IUiMediator> _mediatorMap = new();
        private readonly Stack<IUiMediator> _windowStack = new();
        
        private IUiMediator _defaultWindowMediator;
        private IUiPopupMediator _currentPopup;
        private IUiDialogMediator _currentDialog;

        public UIService(List<IUiMediator> allMediators, IUiMediator defaultWindowMediator)
        {
            foreach (var mediator in allMediators)
                _mediatorMap[mediator.GetType()] = mediator;

            _defaultWindowMediator = defaultWindowMediator;
            
            foreach (var mediator in _mediatorMap.Values)
            {
                if (mediator != _defaultWindowMediator)
                    mediator.Hide();
            }
            
            _defaultWindowMediator?.Show();
        }
        
        public void OpenWindow<TMediator>() where TMediator : IUiMediator
        {
            if (!_mediatorMap.TryGetValue(typeof(TMediator), out var mediatorToOpen))
            {
                Debug.LogError($"Mediator of type {typeof(TMediator).Name} not found in map!");
                return;
            }
            
            CloseCurrentDialog();
            CloseCurrentPopup();
            
            if (_windowStack.Count > 0 && _windowStack.Peek() == mediatorToOpen)
                return;

            if (_windowStack.Count > 0)
            {
                var current = _windowStack.Peek();
            
                if (current != _defaultWindowMediator) 
                {
                    current.Hide();
                    _windowStack.Pop();
                }
            }
        
            Subscribe(mediatorToOpen);
            mediatorToOpen.Show();
            _windowStack.Push(mediatorToOpen);
        }
        
        public void OpenPopup<TMediator>() where TMediator : IUiPopupMediator
        {
            CloseCurrentPopup();
            
            if (!_mediatorMap.TryGetValue(typeof(TMediator), out var mediator))
            {
                Debug.LogError($"Mediator of type {typeof(TMediator).Name} not found in map!");
                return;
            }
            
            Unsubscribe(_currentPopup);
            _currentPopup = (IUiPopupMediator)mediator;
            
            Subscribe(_currentPopup);
            _currentPopup.Show();
        }

        public void OpenDialog<TMediator>() where TMediator : IUiDialogMediator
        {
            CloseCurrentDialog();
            
            if (!_mediatorMap.TryGetValue(typeof(TMediator), out var mediator))
            {
                Debug.LogError($"Mediator of type {typeof(TMediator).Name} not found in map!");
                return;
            }
            
            Unsubscribe(_currentDialog);
            _currentDialog = (IUiDialogMediator)mediator;
            
            Subscribe(_currentDialog);
            _currentDialog.Show();
        }

        public void HandleBackAction()
        {
            if (_currentDialog != null)
            {
                CloseCurrentDialog();
                return;
            }
            
            if (_currentPopup != null)
            {
                CloseCurrentPopup();
                return;
            }
            
            if (_windowStack.Count > 1)
                CloseCurrentWindow();
        }

        private void CloseCurrentWindow()
        {
            CloseCurrentPopup();
            
            if (_windowStack.Count <= 0)
                return;
            
            var current = _windowStack.Pop();
            Unsubscribe(current);
            current.Hide();

            if (_windowStack.Count == 0)
            {
                if (_defaultWindowMediator == null)
                    return;
                
                _windowStack.Push(_defaultWindowMediator);
                _defaultWindowMediator.Show();
            }
            else
            {
                _windowStack.Peek().Show();
            }
        }

        private void CloseCurrentDialog()
        {
            if (_currentDialog == null)
                return;
            
            _currentDialog.Hide();
            _currentDialog = null;
        }

        private void CloseCurrentPopup()
        {
            if (_currentPopup == null)
                return;
            
            _currentPopup.Hide();
            _currentPopup = null;
        }
        
        private void Subscribe(IUiMediator mediator) =>
            mediator.RequestClose += HandleMediatorCloseRequest;
        
        private void Unsubscribe(IUiMediator mediator) =>
            mediator.RequestClose -= HandleMediatorCloseRequest;
        
        private void HandleMediatorCloseRequest(IUiMediator mediator)
        {
            if (mediator == _currentDialog)
                CloseCurrentDialog();
            else if (mediator == _currentPopup)
                CloseCurrentPopup();
            else if (_windowStack.Contains(mediator) && _windowStack.Peek() == mediator)
                CloseCurrentWindow();
        }
    }
}