/*
 * Copyright (c) 2020 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the License);
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an AS IS BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Linq;
using Mono.Cecil;

namespace Tizen.Build.Validators
{
    internal class StructValidator
    {
        public void Validate(string file)
        {
            var asm = AssemblyDefinition.ReadAssembly(file);
            bool isValid = true;
            foreach (var m in asm.Modules)
            {
                foreach (var t in m.Types)
                {
                    isValid &= ValidateType(t);
                }
            }

            if (!isValid)
            {
                throw new InvalidOperationException("** Invalid struct size is detected.");
            }
        }

        private bool ValidateStruct(TypeDefinition structure)
        {
            var managedSize = CSharpSizeProvider.SizeOfStruct(structure);
            var unmanagedSize = NativeSizeProvider.SizeOf(structure);

            if (managedSize != unmanagedSize)
            {
                var typeName = structure.FullName;
                if (unmanagedSize < 0)
                {
                    Log.Error($"* {typeName} size is missing in native size data.");
                }
                else
                {
                    Log.Error($"* {typeName} size is mismatch. Managed({managedSize}) != Unmanaged({unmanagedSize}).");
                }
                return false;
            }

            return true;
        }

        private bool ValidateType(TypeDefinition type)
        {
            if (HasNativeStructAttribute(type))
            {
                return ValidateStruct(type);
            }

            bool isValid = true;
            foreach (var t in type.NestedTypes)
            {
                isValid &= ValidateType(t);
            }
            return isValid;
        }

        private bool HasNativeStructAttribute(TypeDefinition type)
        {
            var attr = type.CustomAttributes.FirstOrDefault(a => a.AttributeType.FullName == "Tizen.Internals.NativeStructAttribute");
            return attr != null;
        }
    }
}