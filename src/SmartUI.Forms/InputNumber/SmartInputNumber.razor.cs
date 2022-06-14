namespace SmartUI.Forms
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using SmartUI.Forms.Models;
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    public partial class SmartInputNumber<TValue> : BaseInputComponent<TValue>
    {
        private FieldIdentifier _fieldIdentifier;
        private string FieldStateCssClass => ParentEditContext.FieldCssClass(_fieldIdentifier);
        private StringBuilder _cssClassBuilder = new StringBuilder();

        private Type[] _allowedTypes = new Type[] { typeof(int), typeof(float), typeof(decimal), typeof(double), typeof(UInt16), typeof(UInt32), typeof(UInt64) };
        private Type[] _intgerTypes = new Type[] { typeof(int), typeof(UInt16), typeof(UInt32), typeof(UInt64) };
        private bool _isValueIntger;

        private void OnValidationChanged(Object? sender, EventArgs? args)
        {
            _cssClassBuilder.Clear();
            _cssClassBuilder.Append(FieldStateCssClass);
            if (FieldStateCssClass.Contains(" invalid"))
                _cssClassBuilder.Append(" is-invalid");
            else if (FieldStateCssClass.Contains(" valid"))
                _cssClassBuilder.Append(" is-valid");
        }

        protected override TValue InternalValue { get; set; }

        protected override async Task OnInternalValueChanged(TValue value)
        {
            await ValueChanged.InvokeAsync(value);
            ParentEditContext?.NotifyFieldChanged(_fieldIdentifier);
            await OnChange.InvokeAsync(value);
        }

        protected override Task<ParseValue<TValue>> ParseValueFromString(string value)
        {
            TValue parsedValue = default;

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    if (_isValueIntger)
                        value = GetFirstNumberFromDecimalNumber(value);

                    parsedValue = (TValue)Convert.ChangeType(value, Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue));
                }
                catch (Exception)
                {
                    Console.WriteLine($"Cannot Parse {value} to {nameof(TValue)}");
                }
            }
            return Task.FromResult(new ParseValue<TValue>(true, parsedValue, null));
        }
        private string GetFirstNumberFromDecimalNumber(string value)
        {
            if (value.Contains("."))
                return value.Split('.')[0];

            return value;
        }
        private string BindValueEventName => IsChangeOnKeyPress ? "oninput" : "onchange";

        protected override void OnInitialized()
        {
            if (!IsAllowType(typeof(TValue)))
                throw new Exception($"type of {typeof(TValue)} not allowed in SmInputNumber component");

            if (IsIntger(typeof(TValue)))
                _isValueIntger = true;

            if(ValueExpression is not null)
                _fieldIdentifier = FieldIdentifier.Create(ValueExpression);

            if (ParentEditContext is not null)
                ParentEditContext.OnValidationStateChanged += OnValidationChanged;

        }
        protected override void OnParametersSet() => InternalValue = Value;
        private bool IsAllowType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = Nullable.GetUnderlyingType(type);

            return _allowedTypes.Contains(type);
        }
        private bool IsIntger(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = Nullable.GetUnderlyingType(type);

            return _intgerTypes.Contains(type);
        }
        

        [CascadingParameter]
        public EditContext ParentEditContext { get; set; }
        /// <summary>
        /// Gets or sets the value inside the input field.
        /// </summary>
        [Parameter]
        public TValue Value { get; set; }
        /// <summary>
        /// Occurs after number has changed.
        /// </summary>
        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }
        /// <summary>
        /// Occurs after number has changed.
        /// </summary>
        [Parameter]
        public Expression<Func<TValue>> ValueExpression { get; set; }
        /// <summary>
        /// fire when value inside the input changed
        /// </summary>
        [Parameter]
        public EventCallback<TValue> OnChange { get; set; }
        /// <summary>
        /// If true the event for binding is "oninput" otherwise "onchange"
        /// </summary>
        [Parameter]
        public bool IsChangeOnKeyPress { get; set; }
    }
}
