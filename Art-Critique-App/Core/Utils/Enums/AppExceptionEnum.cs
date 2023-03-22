using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Art_Critique.Core.Utils.Enums {
    public enum AppExceptionEnum {
        Undefined = 0,
        EntryTooShort = 1,
        EntryTooLong = 2,
        EntriesDontMatch = 3
    }
}
