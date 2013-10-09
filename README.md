###ReSharper.StringFormat

This is a plugin for [ReSharper](http://jetbrains.com/resharper) which provides context action to replace PHP-like strings with embedded variables to `string.Format` method call with these variables as arguments.

###Example

```csharp
class Program
{
  static void Main()
  {
    var a = 1;
    string s = "a is {a}";
  }
} 
```
will be replaced to:
```csharp
class Program
{
  static void Main()
  {
    var a = 1;
    string s = string.Format("a is {0}", a);
  }
} 
```

###Installation

Available in [ReSharper Gallery](http://resharper-plugins.jetbrains.com/packages/ReSharper.StringFormat/)
