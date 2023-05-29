using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace SourceCrafter
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("a", "A")]
        [InlineData("abc", "Abc")]
        public void Capitalize_ShouldWork(string input, string expected)
        {
            input.Capitalize().Should().Be(expected);
        }

        [Fact]
        public void Join_ShouldWork()
        {
            var strs = new List<string> { "a", "b", "c" };
            strs.Join(",").Should().Be("a,b,c");
            strs.Join(t => t.ToUpperInvariant(), ",").Should().Be("A,B,C");
        }

        [Theory]
        [InlineData("abc", "abc")]
        [InlineData("AbcDef", "abcDef")]
        [InlineData("abc_def", "abcDef")]
        public void ToCamel_ShouldWork(string input, string expected)
        {
            input.ToCamel().Should().Be(expected);
        }

        [Theory]
        [InlineData("abc", "Abc")]
        [InlineData("AbcDef", "AbcDef")]
        [InlineData("abc_def", "AbcDef")]
        public void ToPascal_ShouldWork(string input, string expected)
        {
            input.ToPascal().Should().Be(expected);
        }

        [Theory]
        [InlineData("abc", "abc")]
        [InlineData("AbcDef", "abc_def")]
        public void ToSnakeLower_ShouldWork(string input, string expected)
        {
            input.ToSnakeLower().Should().Be(expected);
        }

        [Theory]
        [InlineData("abc", "ABC")]
        [InlineData("AbcDef", "ABC_DEF")]
        public void ToSnakeUpper_ShouldWork(string input, string expected)
        {
            input.ToSnakeUpper().Should().Be(expected);
        }

        [Theory]
        [InlineData("abc", "abc")]
        [InlineData("AbcDef", "abc-def")]
        public void ToKebabLower_ShouldWork(string input, string expected)
        {
            input.ToKebabLower().Should().Be(expected);
        }

        [Theory]
        [InlineData("abc", "ABC")]
        [InlineData("AbcDef", "ABC-DEF")]
        public void ToKebabUpper_ShouldWork(string input, string expected)
        {
            input.ToKebabUpper().Should().Be(expected);
        }
    }
}
