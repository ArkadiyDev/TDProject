using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Implementations.ConfirmationDialog
{
    public class ConfirmationDialogView : UIView
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private Button _buttonConfirm;
        [SerializeField] private Button _buttonCancel;
        
        public Button ButtonConfirm => _buttonConfirm;
        public Button ButtonCancel => _buttonCancel;
        
        public void SetTitle(string title) =>
            _title.SetText(title);
        
        public void SetDescription(string description) =>
            _description.SetText(description);
    }
}