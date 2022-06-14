namespace SmartUI.Forms
{
    using AntiRap.Core.DynamicFilter;
    using AntiRap.Core.Utilities;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using SmartUI.Forms.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    public partial class SmartDropDownList<TModel, TValue> : BaseInputComponent<TValue>, IInnoDropDownList
        where TModel : class
    {
        #region properties
        [CascadingParameter]
        public EditContext ParentEditContext { get; set; }
        [Parameter]
        public IEnumerable<TModel> DataSource { get; set; }
        [Parameter]
        public string NoRecordsText { get; set; }
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        /// <summary>
        /// Gets or sets the selected value;
        /// </summary>
        [Parameter]
        public TValue Value { get; set; }
        /// <summary>
        /// To Support two way binding with bind-value  
        /// </summary>
        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }
        /// <summary>
        /// Occurs after number has changed.
        /// </summary>
        [Parameter]
        public Expression<Func<TValue>> ValueExpression { get; set; }
        #endregion

        #region fields
        protected override TValue InternalValue { get; set; }
        private List<TModel> entities = new List<TModel>();
        private DropDownFieldSetting _dropDownFieldSetting;
        private DropDownTemplate _dropDownTemplate;
        private FieldIdentifier _fieldIdentifier;
        private StringBuilder _cssClassBuilder = new StringBuilder();
        private bool _isOpend;
        private object CurrentItemText;
        private string CurrentItemTextAsString
        {
            get => ForamtItemTextAsString(CurrentItemText);
            set => InvokeAsync(() => DropDownTextHandler(value));
        }
        private string _fieldStateCssClass => ParentEditContext.FieldCssClass(_fieldIdentifier);
        private async Task DropDownTextHandler(string text)
        {
            ParseValue<object> parsedValue = await ParseItemTextFromString(text);
            if (parsedValue.Success)
                CurrentItemText = parsedValue.ParsedValue;

        }
        private string ForamtItemTextAsString(object obj) => obj?.ToString();
        private Task<ParseValue<object>> ParseItemTextFromString(string itemText)
        {
            object textAsObject = default;
            try
            {
                textAsObject = (object)Convert.ChangeType(itemText, typeof(object));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Task.FromResult(new ParseValue<object>(true, textAsObject, null));
        }
        #endregion

        #region methods
        protected override void OnInitialized()
        {
            if (ValueExpression is not null)
                _fieldIdentifier = FieldIdentifier.Create(ValueExpression);

            if (ParentEditContext is not null)
                ParentEditContext.OnValidationStateChanged += OnValidationChanged;
        }
        protected override void OnParametersSet()
        {
            entities = DataSource?.ToList() ?? new List<TModel>();
            InternalValue = Value;
            if (Value is null)
                return;

            TModel item = entities?.FirstOrDefault(ObjectFilter.Equals.GenerateExpression<TModel>(_dropDownFieldSetting?.Value, Value).Compile());
            OnValueSelected(item);
        }
        public void AddFieldSetting(DropDownFieldSetting dropDownFieldSetting)
        {
            _dropDownFieldSetting = dropDownFieldSetting;
            _dropDownFieldSetting.OnPropertyChanged += StateHasChanged;

            StateHasChanged();
        }
        public void AddDropDownListTemplate(DropDownTemplate dropDownTemplate)
            => _dropDownTemplate = dropDownTemplate;

        private void OnSearchChange(string searchValue)
        {
            entities = DataSource?.Search(_dropDownFieldSetting.Text, searchValue).ToList();

            StateHasChanged();
        }
        private void OnValidationChanged(Object? sender, EventArgs? args)
        {
            _cssClassBuilder.Clear();
            _cssClassBuilder.Append(_fieldStateCssClass);
            if (_fieldStateCssClass.Contains(" invalid"))
                _cssClassBuilder.Append(" is-invalid");
            else if (_fieldStateCssClass.Contains(" valid"))
                _cssClassBuilder.Append(" is-valid");
        }

        private void ToggleDropDown()
        {
            if (Disabled)
                return;

            _isOpend = !_isOpend;
        }
        private void OnValueSelected(TModel item)
        {
            if (item is null)
                return;

            CurrentValueAsString = PropertyHandler.GetPropertyValue(item, _dropDownFieldSetting.Value).ToString();
            CurrentItemTextAsString = PropertyHandler.GetPropertyValue(item, _dropDownFieldSetting.Text).ToString();
            _isOpend = false;
        }

        protected override async Task OnInternalValueChanged(TValue value)
        {
            await ValueChanged.InvokeAsync(value);
            ParentEditContext?.NotifyFieldChanged(_fieldIdentifier);
        }
        protected override Task<ParseValue<TValue>> ParseValueFromString(string value)
        {
            TValue parsedValue = default;
            if (!string.IsNullOrEmpty(value))
            {

                try
                {
                    parsedValue = (TValue)Convert.ChangeType(value, Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return Task.FromResult(new ParseValue<TValue>(true, parsedValue, null));
        }
        #endregion
    }
}
