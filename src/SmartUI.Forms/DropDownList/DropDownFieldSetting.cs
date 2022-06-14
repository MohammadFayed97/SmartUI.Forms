namespace SmartUI.Forms
{
    using Microsoft.AspNetCore.Components;
    using System;

    public class DropDownFieldSetting : BaseComponent
    {
        [CascadingParameter]
        public IInnoDropDownList DropDownParent { get; set; }
        [Parameter]
        public string Value
        {
            get => _value;
            set
            {
                if(_value != value)
                {
                    _value = value;
                    RaiseStateChanged();
                }
            }
        }


        [Parameter]
        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    RaiseStateChanged();
                }
            }
        }

        public event EventHandler OnPropertyChanged;

        private string _value;
        private string _text;

        protected override void OnInitialized()
        {
            if (DropDownParent is null)
                throw new ArgumentNullException(nameof(DropDownParent), $"{nameof(DropDownFieldSetting)} must be include in InnoDropDownList Component");

            DropDownParent.AddFieldSetting(this);
        }
        private void RaiseStateChanged() => OnPropertyChanged?.Invoke(this, new EventArgs());
    }
}
