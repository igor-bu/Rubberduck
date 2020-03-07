﻿using Rubberduck.CodeAnalysis.Inspections.Abstract;
using Rubberduck.Parsing.Grammar;
using Rubberduck.Parsing.Symbols;
using Rubberduck.Parsing.VBA;
using Rubberduck.Parsing.VBA.DeclarationCaching;
using Rubberduck.Resources.Inspections;

namespace Rubberduck.CodeAnalysis.Inspections.Concrete
{
    /// <summary>
    /// Identifies uses of 'IsMissing' involving non-variant, non-optional, or array parameters.
    /// </summary>
    /// <why>
    /// 'IsMissing' only returns True when an optional Variant parameter was not supplied as an argument.
    /// This inspection flags uses that attempt to use 'IsMissing' for other purposes, resulting in conditions that are always False.
    /// </why>
    /// <example hasResult="true">
    /// <![CDATA[
    /// Public Sub DoSomething(ByVal foo As Long = 0)
    ///     If IsMissing(foo) Then Exit Sub ' condition is always false
    ///     ' ...
    /// End Sub
    /// ]]>
    /// </example>
    /// <example hasResult="false">
    /// <![CDATA[
    /// Public Sub DoSomething(Optional ByVal foo As Variant = 0)
    ///     If IsMissing(foo) Then Exit Sub
    ///     ' ...
    /// End Sub
    /// ]]>
    /// </example>
    internal class IsMissingOnInappropriateArgumentInspection : IsMissingInspectionBase
    {
        public IsMissingOnInappropriateArgumentInspection(IDeclarationFinderProvider declarationFinderProvider)
            : base(declarationFinderProvider)
        {}

        protected override (bool isResult, ParameterDeclaration properties) IsUnsuitableArgumentWithAdditionalProperties(ArgumentReference reference, DeclarationFinder finder)
        {
            var parameter = ParameterForReference(reference, finder);

            var isResult = parameter != null
                           && (!parameter.IsOptional
                               || !parameter.AsTypeName.Equals(Tokens.Variant)
                               || !string.IsNullOrEmpty(parameter.DefaultValue)
                               || parameter.IsArray);
            return (isResult, parameter);
        }

        protected override string ResultDescription(IdentifierReference reference, ParameterDeclaration parameter)
        {
            return InspectionResults.IsMissingOnInappropriateArgumentInspection;
        }
    }
}
