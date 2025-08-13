using System;

public static class TypeExtensions
{
    public static Type GetBaseTypeGenericArgument(this Type origin)
    {
        var baseType = origin.BaseType;

        if (baseType == null)
        {
            return null;
        }

        if (baseType.GenericTypeArguments.Length == 0)
        {
            while (true)
            {
                var nextBase = baseType.BaseType;

                if (nextBase == null)
                {
                    return null;
                }

                baseType = nextBase;

                if (baseType.GenericTypeArguments.Length > 0)
                {
                    break;
                }
            }

            if (baseType.GenericTypeArguments.Length == 0)
            {
                return null;
            }
        }

        return baseType.GenericTypeArguments[0];
    }
}