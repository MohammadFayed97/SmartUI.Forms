namespace SmartUI.Forms
{
    using Microsoft.AspNetCore.Components;
    using System;

    public class DropDownTemplate : BaseComponent
    {
        [CascadingParameter]
        public IInnoDropDownList DropDownParent { get; set; }

        [Parameter]
        public RenderFragment<object> TextTemplate { get; set; }

        protected override void OnInitialized()
        {
            if (DropDownParent is null)
                throw new ArgumentNullException(nameof(DropDownParent), $"{nameof(DropDownTemplate)} must be include in InnoDropDownList Component");

            DropDownParent.AddDropDownListTemplate(this);
            base.OnInitialized();
        }
    }
}
