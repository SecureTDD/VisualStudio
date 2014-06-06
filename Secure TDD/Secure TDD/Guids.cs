// Guids.cs
// MUST match guids.h
using System;

namespace Secure_TDD
{
    static class GuidList
    {
        public const string guidSecure_TDDPkgString = "582c735b-297a-4d19-addf-aed94fa46908";
        public const string guidSecure_TDDCmdSetString = "634fd512-eb49-44c0-9ad7-76cb05a6746b";

        public static readonly Guid guidSecure_TDDCmdSet = new Guid(guidSecure_TDDCmdSetString);
    };
}