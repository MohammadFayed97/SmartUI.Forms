namespace SmartUI.Forms
{
    using AntiRap.Core.DynamicFilter;
    using System.Collections.Generic;
    using System.Linq;

    public static class ListExtention
    {
        public static IQueryable<TModel> Search<TModel>(this IEnumerable<TModel> source, string propertyName, string serchValue)
            where TModel : class
        {
            if (string.IsNullOrWhiteSpace(serchValue))
                return source.AsQueryable();

            return source.AsQueryable().Where(ObjectFilter.Contains.GenerateExpression<TModel>(propertyName, serchValue, null));
        }
    }
}
