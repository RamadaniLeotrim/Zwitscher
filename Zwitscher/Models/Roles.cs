using System.ComponentModel;
using System.Reflection;

namespace Zwitscher.Models
{
    
    public enum Roles
    {
        [Description("Administrator")]
        Admin,
        [Description("Benutzer")]
        User
    }

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            try
            {
                var field = value.GetType().GetField(value.ToString());
                var attribute = field.GetCustomAttribute<DescriptionAttribute>();
                return attribute?.Description ?? value.ToString();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error retrieving description: {ex.Message}");

                return value.ToString();
            }


        }
    }
}
