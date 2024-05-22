using System.ComponentModel;
using System.Linq.Expressions;

namespace Dms.Core.Application.Common.Extensions.Utils;

    public static class DescriptionAttributeExtensions
    {
        public static string GetDescription(this Enum e)
        {
            var name = e.ToString();
            var memberInfo = e.GetType().GetMember(name)[0];
            var descriptionAttributes = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
            if (descriptionAttributes.Any())
            {
                return ((DescriptionAttribute)descriptionAttributes.First()).Description;
            }
            return name;
        }

        public static string GetMemberDescription<T, TProperty>(this T t, Expression<Func<T, TProperty>> property) where T : class, new()
        {
            var memberName = ((MemberExpression)property.Body).Member.Name;
            var memberInfo = typeof(T).GetMember(memberName).FirstOrDefault();
            if (memberInfo != null)
            {
                var descriptionAttributes = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
                if (descriptionAttributes.Any())
                {
                    return ((DescriptionAttribute)descriptionAttributes.First()).Description;
                }
            }
            return memberName;
        }
    }

