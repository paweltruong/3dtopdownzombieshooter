using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Flags]
public enum PhysicsCategories : uint
{
    Nothing = 0u,
    Ground = 1 << 0,
    Custom1 = 1 << 1,
    Custom2 = 1 << 2,
    Custom3 = 1 << 3,
    Custom4 = 1 << 4,
    Custom5 = 1 << 5,
    Custom6 = 1 << 6,
    Custom7 = 1 << 7,
    Everything = ~0u,


}
