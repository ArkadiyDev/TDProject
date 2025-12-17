using System;
using System.Collections.Generic;
using InputSystem;
using UnityEngine;

namespace UI
{
    public class UIService : IUIService
    {
        private readonly Dictionary<Type, IUiMediator> _mediatorMap = new();
        private readonly Stack<IUiMediator> _windowStack = new();
        
        private IUIPopupMediator _currentPopup;
        private IUIDialogMediator _currentDialog;

        public UIService(List<IUiMediator> allMediators)
        {
            foreach (var mediator in allMediators)
                _mediatorMap[mediator.GetType()] = mediator;

            if (allMediators.Count > 0)
            {
                var defaultWindow = allMediators[0];
                defaultWindow.Show();
                _windowStack.Push(defaultWindow);
            }
        }

        public TMediator OpenWindow<TMediator>() where TMediator : IUiMediator
        {
            if (!_mediatorMap.TryGetValue(typeof(TMediator), out var mediatorToOpen))
            {
                Debug.LogError($"Mediator of type {typeof(TMediator).Name} not found in map!");
                return default;
            }
            
            CloseCurrentDialog();
            CloseCurrentPopup();
            
            if (_windowStack.Count > 0)
            {
                if (_windowStack.Peek() == mediatorToOpen)
                    return default;
                
                var current = _windowStack.Peek();
                current.Hide();
            }
        
            mediatorToOpen.Show();
            _windowStack.Push(mediatorToOpen);
            
            return (TMediator)mediatorToOpen;
        }
        
        public TMediator OpenPopup<TMediator>() where TMediator : IUIPopupMediator
        {
            CloseCurrentPopup();
            
            if (!_mediatorMap.TryGetValue(typeof(TMediator), out var mediator))
            {
                Debug.LogError($"Mediator of type {typeof(TMediator).Name} not found in map!");
                return default;
            }
            
            _currentPopup = (IUIPopupMediator)mediator;
            _currentPopup.Show();
            
            return (TMediator)mediator;
        }

        public TMediator OpenDialog<TMediator>() where TMediator : IUIDialogMediator
        {
            CloseCurrentDialog();
            
            if (!_mediatorMap.TryGetValue(typeof(TMediator), out var mediator))
            {
                Debug.LogError($"Mediator of type {typeof(TMediator).Name} not found in map!");
                return default;
            }
            
            _currentDialog = (IUIDialogMediator)mediator;
            _currentDialog.Show();
            
            return (TMediator)mediator;
        }

        public bool ProcessKeyInput(InputIntent keyCode)
        {
            if (_currentDialog is IInputHandler dialogHandler && dialogHandler.HandleKeyPressed(keyCode))
                return true;
        
            if (_currentPopup is IInputHandler popupHandler && popupHandler.HandleKeyPressed(keyCode))
                return true;

            if (_windowStack.Count <= 0)
                return false;
            
            var currentWindow = _windowStack.Peek();
                
            return currentWindow is IInputHandler windowHandler && windowHandler.HandleKeyPressed(keyCode);
        }

        public void CloseCurrentWindow()
        {
            CloseCurrentDialog();
            CloseCurrentPopup();
            
            if (_windowStack.Count <= 1)
                return;
            
            var current = _windowStack.Pop();
            current.Hide();
            _windowStack.Peek().Show();
        }

        public void CloseCurrentDialog()
        {
            if (_currentDialog == null)
                return;
            
            _currentDialog.Hide();
            _currentDialog = null;
        }

        public void CloseCurrentPopup()
        {
            if (_currentPopup == null)
                return;
            
            _currentPopup.Hide();
            _currentPopup = null;
        }
    }
}