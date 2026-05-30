using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato_De_Pilha
{
    internal record Estado(
        string Nome,
        bool Inicial = false,
        bool Final = false
    );
}
