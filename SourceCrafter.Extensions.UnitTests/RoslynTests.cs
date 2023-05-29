using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCrafter;
using System.Collections.Immutable;

namespace SourceCrafterExtensionsTests;

public class RoslynTests
{
    [Fact]
    public void IsNullableTest()
    {
        // Arrange
        var compilation = CreateCompilation("");
        var nullableType = compilation.GetTypeByMetadataName("System.Nullable`1")!;
        var intType = compilation.GetTypeByMetadataName("System.Int32")!;

        // Act
        var isNullable = nullableType.IsNullable();
        var isIntNullable = intType.IsNullable();

        // Assert
        isNullable.Should().BeTrue();
        isIntNullable.Should().BeFalse();
    }

    [Fact]
    public void AllowsNullTest()
    {
        // Arrange
        var compilation = CreateCompilation("");
        var nullableType = compilation.GetTypeByMetadataName("System.Nullable`1")!;
        var intType = compilation.GetTypeByMetadataName("System.Int32")!;
        var stringType = compilation.GetTypeByMetadataName("System.String")!;

        // Act
        var allowsNullForNullable = nullableType.AllowsNull();
        var allowsNullForInt = intType.AllowsNull();
        var allowsNullForString = stringType.AllowsNull();

        // Assert
        allowsNullForNullable.Should().BeTrue();
        allowsNullForInt.Should().BeFalse();
        allowsNullForString.Should().BeTrue();
    }

    [Fact]
    public void IsAttributeNameTest()
    {
        // Arrange
        var source = @"
using System;

MyClass test;

[MyAttribute]
public class MyClass { }

public class MyAttribute : Attribute { }
";
        var compilation = CreateCompilation(source);
        var tree = compilation.SyntaxTrees.First();
        var model = compilation.GetSemanticModel(tree);
        var varSyntax = tree.GetRoot().DescendantNodes().OfType<VariableDeclarationSyntax>().Single()!;
        var attributeData = model.GetTypeInfo(varSyntax.Type).Type!.GetAttributes().Single()!;

        // Act
        var isMyAttribute = "MyAttribute".IsAttributeName(attributeData);

        // Assert
        isMyAttribute.Should().BeTrue();
    }
    [Fact]
    public void TryGetNameOfFromAttributeArgTest()
    {
        // Arrange
        var source = @"
using System;

MyClass test;

[MyAttribute(nameof(MyClass.MyProperty))]
[MyAttribute(typeof(MyClass))]
public class MyClass
{
    public int MyProperty { get; set; }
}

public class MyAttribute : Attribute
{
    public MyAttribute(string name) { }
}
";
        var compilation = CreateCompilation(source);
        var tree = compilation.SyntaxTrees.First();
        var model = compilation.GetSemanticModel(tree);
        var varSyntax = tree.GetRoot().DescendantNodes().OfType<VariableDeclarationSyntax>().Single()!;
        var attributeData = model.GetTypeInfo(varSyntax.Type).Type!.GetAttributes().ToImmutableArray()!;
        
        // Act
        var success = RoslynExtensions.TryGetNameOfArgumentFromAttributeArgument(attributeData[0]!, model!, 0, out IPropertySymbol symbol);

        // Assert
        success.Should().BeTrue();
        symbol.Should().NotBeNull();
        symbol!.Name.Should().Be("MyProperty");
        
        success = RoslynExtensions.TryGetTypeOfArgumentFromAttributeArgument(attributeData[1]!, model!, 0, out ITypeSymbol typeSymbol);

        // Assert
        success.Should().BeTrue();
        typeSymbol.Should().NotBeNull();
        typeSymbol!.Name.Should().Be("MyClass");
    }

    private static Compilation CreateCompilation(string source)
    {
        return CSharpCompilation.Create("MyCompilation",
            new[] { CSharpSyntaxTree.ParseText(source) },
            new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) },
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
    }
}
