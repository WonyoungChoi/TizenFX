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
using System.Collections.Generic;
using Mono.Cecil;

namespace Tizen.Build.Validators
{

    internal class StructValidator
    {
        private readonly List<StructInfo> _structList;

        public StructValidator()
        {
            _structList = new List<StructInfo>();
        }

        public void Validate(IEnumerable<string> targetFiles)
        {
            foreach (var file in targetFiles)
            {
                var asm = AssemblyDefinition.ReadAssembly(file);
                foreach (var module in asm.Modules)
                {
                    foreach (var type in module.Types)
                    {
                        VisitType(type);
                    }
                }
            }

            var checker = new NativeChecker(_structList);
            checker.Run();
        }

        private void VisitType(TypeDefinition type)
        {
            var attr = GetNativeStructAttribute(type);
            if (attr != null)
            {
                var structInfo = new StructInfo();
                structInfo.Name = type.FullName;
                structInfo.Size = CSharpSizeProvider.SizeOfStruct(type);
                structInfo.NativeStruct = (string)attr.ConstructorArguments[0].Value;
                structInfo.PkgConfig = (string)(attr.Properties.FirstOrDefault(p => p.Name == "PkgConfig").Argument.Value ?? string.Empty);
                structInfo.Include = (string)(attr.Properties.FirstOrDefault(p => p.Name == "Include").Argument.Value ?? string.Empty);

                _structList.Add(structInfo);

                Log.Verbose(structInfo.ToString());
            }

            foreach (var nested in type.NestedTypes)
            {
                VisitType(nested);
            }
        }

        private CustomAttribute GetNativeStructAttribute(TypeDefinition type)
        {
            return type.CustomAttributes.FirstOrDefault(a => a.AttributeType.FullName == "Tizen.Internals.NativeStructAttribute");
        }
    }

}