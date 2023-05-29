using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using SourceCrafter;

namespace SourceCrafter;

public class CollectionTests
{
    [Fact]
    public void DeconstructTest()
    {
        var kvp = new KeyValuePair<int, string>(1, "one");
        (int key, string val) = kvp;
        key.Should().Be(1);
        val.Should().Be("one");
    }

    [Fact]
    public void AddNestedTest()
    {
        var dict = new Dictionary<int, List<string>>();

        dict.AddNested(1, "one");
        dict.Should().HaveCount(1);
        dict[1].Should().HaveCount(1);
        dict[1][0].Should().Be("one");

        dict.AddNested(1, "two");
        dict.Should().HaveCount(1);
        dict[1].Should().HaveCount(2);
        dict[1][1].Should().Be("two");
    }
}
