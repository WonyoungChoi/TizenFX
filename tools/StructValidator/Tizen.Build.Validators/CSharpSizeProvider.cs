//
// This code blocks are from
//     Gendarme.Rules.Performance.AvoidLargeStructureRule of mono-tools
//
// Authors:
//	Sebastien Pouliot <sebastien@ximian.com>
//
// Copyright (C) 2008 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace Tizen.Build.Validators
{
    internal static class CSharpSizeProvider
    {
        private const int DefaultPaddingSize = 8;
        private const int ReferenceSize = 4;

        static Dictionary<string, int> Primitives = new Dictionary<string, int>(14) {
            { "Byte", 1 },
            { "SByte", 1 },
            { "Boolean", 1 },
            { "Int16", 2 },
            { "UInt16", 2 },
            { "Char", 2 },
            { "Int32", 4 },
            { "UInt32", 4 },
            { "Single", 4 },
            { "Int64", 8 },
            { "UInt64", 8 },
            { "Double", 8 },
            { "IntPtr", ReferenceSize },	// so rule return the same results
			{ "UIntPtr", ReferenceSize },	// on 32 and 64 bits architectures
		};

        private static long SizeOfEnum(TypeDefinition type)
        {
            foreach (FieldDefinition field in type.Fields)
            {
                // we looking for the special value__
                // padding, if required, will be computed by caller
                if (!field.IsStatic)
                    return SizeOf(field.FieldType);
            }
            return 4; // Int32 size
        }


        // Note: Needs to be public since this is being tested by our unit tests
        public static long SizeOfStruct(TypeDefinition type)
        {
            if (type == null)
                return 0;

            long total = 0;
            long largest = ReferenceSize;
            foreach (FieldDefinition field in type.Fields)
            {
                // static fields do not count
                if (field.IsStatic)
                    continue;

                // add size of the type
                long size = SizeOf(field.FieldType);

                // would this be aligned correctly
                if (size > 1)
                {
                    long align = Math.Min(ReferenceSize, size);
                    if (total % align != 0)
                        size += (align - total % align);
                }

                total += size;

                if (largest < size)
                    largest = Math.Min(size, DefaultPaddingSize);
            }

            // minimal size, even for an empty struct, is one byte
            if (total == 0)
                return 1;
            if (total < ReferenceSize)
                return total;

            long remainder = (total % ReferenceSize);
            if (remainder != 0)
                total += (largest - remainder);

            return total;
        }

        private static long SizeOf(TypeReference type)
        {
            if (!type.IsValueType || type.IsArray)
                return ReferenceSize;

            // list based on Type.IsPrimitive
            if (type.Namespace == "System")
            {
                int size;
                if (Primitives.TryGetValue(type.Name, out size))
                    return (long)size;
            }

            TypeDefinition td = type.Resolve();
            // we don't know enough about the type so we assume it's a reference
            if (td == null)
                return ReferenceSize;

            if (td.IsEnum)
                return SizeOfEnum(td);

            return SizeOfStruct(td);
        }
    }
}