using System.Reflection;

namespace NodeCanvas.Framework
{
    // Simple interface to handle reflected node wrappers
    public interface IReflectedWrapper {

    MemberInfo GetMemberInfo();
  }
}