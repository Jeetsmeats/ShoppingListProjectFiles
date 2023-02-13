using System.ComponentModel;
using System.Reflection;

namespace ProductsLibrary.Models
{
    public enum ColesCategories
    {
        [Description("https://www.coles.com.au/browse/meat-seafood")]
        MeatSeafood,
        [Description("https://www.coles.com.au/browse/fruit-vegetables")]
        FruitandVeg,
        [Description("https://www.coles.com.au/browse/dairy-eggs-fridge")]
        DairyEggsFridge,
        [Description("https://www.coles.com.au/browse/bakery")]
        Bakery,
        [Description("https://www.coles.com.au/browse/deli")]
        Deli,
        [Description("https://www.coles.com.au/browse/pantry")]
        Pantry,
        [Description("https://www.coles.com.au/browse/drinks")]
        Drinks,
        [Description("https://www.coles.com.au/browse/frozen")]
        Frozen,
        [Description("https://www.coles.com.au/browse/household")]
        Household,
        [Description("https://www.coles.com.au/browse/health-beauty")]
        HealthBeauty,
        [Description("https://www.coles.com.au/browse/baby")]
        Baby,
        [Description("https://www.coles.com.au/browse/pet")]
        Pet,
        [Description("https://www.coles.com.au/browse/liquor")]
        Liquor,
        [Description("https://www.coles.com.au/browse/back-to-school")]
        Back2School,
        None
    }
    public enum Company { Coles, Woolworths }
    public enum WoolworthsCategories
    {
        FruitVeg,
        MeatSeafoodDeli,
        Bakery,
        DairyEggsFridge,
        Back2School,
        Pantry,
        SnacksConfectionary,
        Frozen,
        Drinks,
        Liquor,
        Pet,
        Baby,
        HealthBeauty,
        Household
    }

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum e)
        {
            var attribute =
                e.GetType()
                    .GetTypeInfo()
                    .GetMember(e.ToString())
                    .FirstOrDefault(member => member.MemberType == MemberTypes.Field)
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .SingleOrDefault()
                    as DescriptionAttribute;

            return attribute?.Description ?? e.ToString();
        }
    }


}
