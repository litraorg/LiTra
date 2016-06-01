using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiTra.Exceptions {
  public class NoEmptyConstructorException : Exception {
    public NoEmptyConstructorException() { }
    public NoEmptyConstructorException(string message) : base(message) { }
  }
}