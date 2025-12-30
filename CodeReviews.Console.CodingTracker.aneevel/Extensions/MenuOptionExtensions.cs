using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using CodeReviews.Console.CodingTracker.aneevel.Enums;

namespace CodeReviews.Console.CodingTracker.aneevel.Extensions
{
    internal static class MenuOptionExtensions
    {
        extension(MenuOption option)
        {
            public string GetDisplayName()
            {
                var fieldInfo = option.GetType().GetField(option.ToString());

                return (
                        fieldInfo?.GetCustomAttributes(typeof(DisplayAttribute), false)
                            is DisplayAttribute[] { Length: > 0 } attributes
                            ? attributes[0].Name
                            : option.ToString()
                    ) ?? string.Empty;
            }
        }
    }
}
