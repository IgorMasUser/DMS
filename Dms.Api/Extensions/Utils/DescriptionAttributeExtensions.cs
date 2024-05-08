﻿using System.ComponentModel;
using System.Linq.Expressions;

namespace Dms.Api.Extensions.Utils
{
    public static class DescriptionAttributeExtensions
    {
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
}
