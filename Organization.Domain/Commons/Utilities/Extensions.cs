using System.Formats.Tar;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Organization.Domain.Commons.Utilities
{
    public static class Extensions
    {
        // The 'this' keyword here creates an extension method for Type 
        public static string GetDbTableName(this Type type)
        {
            string tableName = type.GetCustomAttribute<TableNameAttribute>()?.NameValue ?? string.Empty;

            return tableName;
        }
                
        public static string GetDbTableColumnNames(this Type type, string[] selectedTableColumns)
        {
            if (selectedTableColumns.Length == 0)
            {
                var tableColumnNames = string.Join(",", type.GetProperties()
                                                     .Select(p => p.GetDbColumnName())
                )
                .TrimEnd(',');

                return tableColumnNames;
            }
            else
            {
                var tableColumnNamesWithInvariant = string.Join(",", type.GetProperties()
                                                     .Where(p => selectedTableColumns.ToLowerInvariant()
                                                                        .Contains(p.Name.ToLowerInvariant())
                                                     )
                                                     .Select(p => p.GetDbColumnName())
                )
                .TrimEnd(',');

                return tableColumnNamesWithInvariant;
            }
        }

        public static string GetDbColumnName(this PropertyInfo propertyInfo)
        {
            string tableColumnName = propertyInfo.GetCustomAttribute<ColumnNameAttribute>()?.NameValue ?? string.Empty;

            return tableColumnName;
        }

        public static IEnumerable<string> ToLowerInvariant(this string[] sources)
        {
            foreach (var item in sources)
            {
                // Creates a copy of this string in lower case based on invariant culture & return the copied string
                yield return item.ToLowerInvariant();  
            }
        }

        public static string GetColumnValuesForInsert<TEntity>(this Type type, TEntity entity)
        {
            string columnValues = string.Join(",", 
                                type.GetColumnProperties()
                                    .Select(p => $"'{p.GetValue(entity)}'")
            );

            return columnValues;
        }


        public static IEnumerable<PropertyInfo> GetColumnProperties(this Type type)
        {
            IEnumerable<PropertyInfo> columnPropertyInfos = type.GetProperties()
                .Where(p => p.GetCustomAttribute<NavigationAttribute>() is null);

            return columnPropertyInfos;
        }

        public static string GetColumnValuesForUpdate<TEntity>(this Type type, TEntity entity)
        {
            string columnValues = string.Join(",", 
                                type.GetNonPrimaryColumnProperties()
                                    .Select(p => $"{p.GetDbColumnName()} = '{p.GetValue(entity)}'"));

            return columnValues;
        }

        public static IEnumerable<PropertyInfo> GetNonPrimaryColumnProperties(this Type type)
        {
            IEnumerable<PropertyInfo> columnPropertyInfos = type.GetProperties()
                                                        .Where(p => p.GetCustomAttribute<PrimaryKeyAttribute>() is null 
                                                        && p.GetCustomAttribute<NavigationAttribute>() is null);

            return columnPropertyInfos;
        }

        public static IEnumerable<AssociatedType> GetAssociatedTypes(this Type type)
        {
            IEnumerable<NavigationAttribute?> navigationAttributes = type.GetProperties()
                    .Where(p => p.GetCustomAttribute<NavigationAttribute>() is not null)
                    .Select(p => p.GetCustomAttribute<NavigationAttribute>());

            foreach (var associatedAttribute in navigationAttributes)
            {
                yield return new AssociatedType(associatedAttribute!.AssociatedType,
                    associatedAttribute.AssociatedType.GetProperty(associatedAttribute.AssociatedProperty)!);
            }
        }

        public static string GetDistinctUniqueKeyName(this Type type)
        {
            string uniqueKeyName = type.GetProperties()
                        .Where(p => p.GetCustomAttribute<DistinctUniqueKeyAttribute>() is not null)
                        .FirstOrDefault()!.Name;

            return uniqueKeyName;
        }

        //using REFLECTION
        //SortOrder will reference OrderByCustom( ) to sort the Employees based of the Specified sorting column
        public static IQueryable<IDbEntity> OrderByCustom<IDbEntity>(this IQueryable<IDbEntity> queryableItems, string sortBy, string sortOrder)
        {
            Type entity = typeof(IDbEntity);
            ParameterExpression parameterExpression = Expression.Parameter(entity, "t");
            PropertyInfo propertyInfo = entity.GetProperty(sortBy)!;
            MemberExpression memberExpression = Expression.MakeMemberAccess(parameterExpression, propertyInfo!);
            LambdaExpression lambdaExpression = Expression.Lambda(memberExpression, parameterExpression);

            MethodCallExpression methodCallExpression = Expression.Call(typeof(Queryable),
                                                                        sortOrder == "desc" ? "OrderByDescending" : "OrderBy",
                                                                        new Type[] { entity, propertyInfo!.PropertyType },
                                                                        queryableItems.Expression,
                                                                        Expression.Quote(lambdaExpression)
                                        );

            IQueryable<IDbEntity> iqueryableOfEntities = queryableItems.Provider.CreateQuery<IDbEntity>(methodCallExpression);

            return iqueryableOfEntities;
        }


        public static bool IsValidEmail(this string email)
        {
            Regex regex = new Regex(@"^\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b$");
            bool isEmailRegexMatched = regex.IsMatch(email);

            if (isEmailRegexMatched == true)
            {
                return true;
            }

            return false;
        }


        public static bool IsValidPassword(this string password)
        {
            Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*()-+])(?=\S+$).{8,}$");
            bool isPasswordRegexMatched = regex.IsMatch(password);

            if (isPasswordRegexMatched == true)
            {
                return true;
            }

            return false;
        }

    }
}
