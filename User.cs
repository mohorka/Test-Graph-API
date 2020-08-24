using System;
namespace Test
{
    public class TestUser
    {
        public string Email{ get; private set; }
        public Guid Id{ get; private set; }
        public string FullName{ get; private set; }
    }
}