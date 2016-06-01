using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiTra.Exceptions {
  public class ResolveOutsideTransformationRuleException : Exception {
    public ResolveOutsideTransformationRuleException() { }
    public ResolveOutsideTransformationRuleException(string message) : base(message) { }
  }  
}