﻿using System.Linq;
using Rubberduck.CodeAnalysis.Inspections.Abstract;
using Rubberduck.Parsing.Annotations;
using Rubberduck.Parsing.Symbols;
using Rubberduck.Parsing.VBA;
using Rubberduck.Parsing.VBA.DeclarationCaching;
using Rubberduck.Resources.Inspections;

namespace Rubberduck.CodeAnalysis.Inspections.Concrete
{
    /// <summary>
    /// Indicates that a user module is missing a @Folder Rubberduck annotation.
    /// </summary>
    /// <why>
    /// Modules without a custom @Folder annotation will be grouped under the default folder in the Code Explorer toolwindow.
    /// By specifying a custom @Folder annotation, modules can be organized by functionality rather than simply listed.
    /// </why>
    /// <example hasResult="true">
    /// <![CDATA[
    /// Option Explicit
    /// ' ...
    /// ]]>
    /// </example>
    /// <example hasResult="false">
    /// <![CDATA[
    /// '@Folder("Foo")
    /// Option Explicit
    /// ' ...
    /// ]]>
    /// </example>
    internal sealed class ModuleWithoutFolderInspection : DeclarationInspectionBase
    {
        public ModuleWithoutFolderInspection(IDeclarationFinderProvider declarationFinderProvider)
            : base(declarationFinderProvider, DeclarationType.Module)
        {}

        protected override bool IsResultDeclaration(Declaration declaration, DeclarationFinder finder)
        {
            return !declaration.Annotations.Any(pta => pta.Annotation is FolderAnnotation);
        }

        protected override string ResultDescription(Declaration declaration)
        {
            return string.Format(InspectionResults.ModuleWithoutFolderInspection, declaration.IdentifierName);
        }
    }
}
