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
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;

namespace Tizen.Build.Validators
{
    internal class NativeChecker
    {
        private readonly IEnumerable<StructInfo> _structList;
        private readonly string _nativeDir;

        public NativeChecker(IEnumerable<StructInfo> structList)
        {
            _structList = structList;

            var asmDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _nativeDir = Path.Join(asmDir, "native");
        }

        public void Run()
        {
            GenerateCheckers();
            GenerateDependencies();

            ExecuteProcess(_nativeDir, "make");
            try {
                ExecuteProcess(_nativeDir, "structcheck");
            } catch (InvalidOperationException e) {
                throw new InvalidStructSizeException(e.Message);
            }
        }

        private void GenerateCheckers()
        {
            var lines = new List<string>();

            var includeLines = new HashSet<string>();
            var checkLines = new List<string>();

            includeLines.Add("#include \"common.h\"");

            foreach (var info in _structList)
            {
                if (!string.IsNullOrEmpty(info.Include))
                {
                    includeLines.Add($"#include <{info.Include}>");
                }

                // CHECK_STRUCT("Interop/Libc/SystemTime", 48, struct tm)
                checkLines.Add($"CHECK_STRUCT(\"{info.Name}\", {info.Size}, {info.NativeStruct})");
            }

            lines.AddRange(includeLines);
            lines.AddRange(checkLines);

            File.WriteAllLines(Path.Join(_nativeDir, "auto-generated.c"), lines);
        }

        private void GenerateDependencies()
        {
            var pkgList = new HashSet<string>();

            foreach (var info in _structList)
            {
                if (!string.IsNullOrEmpty(info.PkgConfig))
                {
                    pkgList.Add(info.PkgConfig);
                }
            }

            string depsText = "DEPS = " + string.Join(' ', pkgList) + "\n";

            File.WriteAllText(Path.Join(_nativeDir, "auto-generated.mk"), depsText);
        }

        private void ExecuteProcess(string workDir, string filename)
        {
            ExecuteProcess(workDir, filename, string.Empty);
        }

        private void ExecuteProcess(string workDir, string filename, string args)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    UseShellExecute = true,
                    WorkingDirectory = workDir,
                    FileName = filename,
                    Arguments = args
                }
            };
            process.Start();

            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"Execute '{filename}'. Exit={process.ExitCode}");
            }
        }
    }

}
