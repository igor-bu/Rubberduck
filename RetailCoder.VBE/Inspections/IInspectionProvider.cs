﻿using System.Collections.Generic;
using Rubberduck.Parsing.Inspections.Abstract;

namespace Rubberduck.Inspections
{
    public interface IInspectionProvider
    {
        IEnumerable<IInspection> Inspections { get; }
    }
}