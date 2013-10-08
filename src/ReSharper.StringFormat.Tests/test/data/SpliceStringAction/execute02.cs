namespace N
{
  class C
  {
    static void Main(string[] args)
    {
      var A = 1;
      string s = string.Format("a is {{caret}0}", A);
    }
  }
}
