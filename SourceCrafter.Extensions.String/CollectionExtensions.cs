#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceCrafter;

public static class CollectionExtensions
{
    public static string? Capitalize(this string? name) => 
        name is { Length: > 1 } 
            ? char.ToUpper(name[0]) + name[1..].ToLowerInvariant() 
            : name?.ToUpperInvariant();

    public static string Join<T>(this IEnumerable<T> strs, Func<T, string> formmater, string? separator = "")
    {
        return string.Join(separator, strs.Select(formmater));
    }

    public static string Join<T>(this IEnumerable<T> strs, string? separator = "")
    {
        return strs.Join(t => t?.ToString() ?? "", separator);
    }
    
    public static string ToCamel(this string name)
    {
        var buffer = new char[name.Length];
        var bufferIndex = 0;
        var needUpper = false;

        foreach (char ch in name)
        {
            bool isDigit = char.IsDigit(ch), isLetter = char.IsLetter(ch), isUpper = char.IsUpper(ch);

            if (isLetter)
            {
                buffer[bufferIndex++] = bufferIndex == 1 && isUpper
                    ? char.ToLower(ch)
                    : !isUpper && needUpper && bufferIndex > 1 && !char.IsUpper(buffer[bufferIndex - 2])
                        ? char.ToUpper(ch)
                        : ch;

                needUpper = false;
                continue;
            }
            else if (isDigit)
            {
                if (bufferIndex == 0)
                    (buffer = new char[buffer.Length + 1])[bufferIndex++] = '_';
                buffer[bufferIndex++] = ch;
            }
            needUpper = true;
        }
        return new string(buffer, 0, bufferIndex);
    }
    
    public static string ToPascal(this string name)
    {
        var buffer = new char[name.Length];
        var bufferIndex = 0;
        var needUpper = false;

        foreach (char ch in name)
        {
            bool isDigit = char.IsDigit(ch), isLetter = char.IsLetter(ch), isUpper = char.IsUpper(ch);
            
            if (isLetter)
            {
                buffer[bufferIndex] = ((bufferIndex++ == 0 || needUpper) && !isUpper)
                    ? char.ToUpper(ch)
                    : ch;
                needUpper = false;
                continue;
            }
            else if (isDigit)
            {
                if (bufferIndex == 0)
                    (buffer = new char[buffer.Length + 1])[bufferIndex++] = '_';
                buffer[bufferIndex++] = ch;
            }
            needUpper = true;
        }
        return new string(buffer, 0, bufferIndex);
    }

    public static string ToSnakeLower(this string name) => ToJoined(name, "_");

    public static string ToSnakeUpper(this string name) => ToJoined(name, "_", true);

    public static string ToKebabLower(this string name) => ToJoined(name, "-");

    public static string ToKebabUpper(this string name) => ToJoined(name, "-", true);

    static string ToJoined(this string name, string separator = "-", bool upper = false)
    {
        var buffer = new char[name.Length * separator.Length];
        var bufferIndex = 0;

        foreach (char ch in name)
        {
            bool isDigit = char.IsDigit(ch), isLetter = char.IsLetter(ch), isUpper = char.IsUpper(ch);

            if (isLetter)
            {
                buffer[bufferIndex++] = (upper, isUpper) switch
                {
                    (true, false) => char.ToUpperInvariant(ch),
                    (false, true) => char.ToLowerInvariant(ch),
                    _ => ch
                };
                continue;
            }
            else if (bufferIndex == 0)
                continue;

            separator.CopyTo(0, buffer, bufferIndex, separator.Length);
            bufferIndex += separator.Length;

            if (isDigit)
            {
                buffer[bufferIndex++] = ch;
            }
        }
        return new string(buffer, 0, bufferIndex);
    }

    public static string? ToCamel(this Enum? value) => value?.ToString().ToCamel();

    public static string? ToPascal(this Enum? value) => value?.ToString().ToPascal();

    public static string? ToSnakeLower(this Enum? value) => value?.ToString().ToSnakeLower();

    public static string ToSnakeUpper(this Enum value) => value.ToString().ToSnakeUpper();

}
