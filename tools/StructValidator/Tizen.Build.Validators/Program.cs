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
using System.Collections.Generic;
using CommandLine;

namespace Tizen.Build.Validators
{
    internal class Options
    {
        [Option('v', "verbose", Required = false, Default = false, HelpText = "Print verbose messages.")]
        public bool Verbose { get; set; }

        [Value(0, MetaName = "target", Required = true, HelpText = "Target assembly or directory")]
        public string Target { get; set; }
    }

    class Program
    {
        public void Run(Options options)
        {
            var targetFiles = new List<string>();
            var path = Path.GetFullPath(options.Target);
            if (Directory.Exists(path))
            {
                var dirInfo = new DirectoryInfo(path);
                var fileInfos = dirInfo.GetFiles("*.dll", SearchOption.TopDirectoryOnly);
                foreach (var f in fileInfos)
                {
                    targetFiles.Add(f.FullName);
                }
            }
            else if (File.Exists(path))
            {
                if (path.EndsWith(".dll"))
                {
                    targetFiles.Add(path);
                }
            }
            else
            {
                throw new FileNotFoundException($"'{options.Target}' is not found.");
            }

            var validator = new StructValidator();
            validator.Validate(targetFiles);
        }

        static int Main(string[] args)
        {
            var program = new Program();

            try
            {
                Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
                {
                    Log.IsVerbose = o.Verbose;
                    program.Run(o);
                });
            }
            catch (InvalidStructSizeException e)
            {
                Log.Error(e.Message);
                Environment.Exit(1);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
                Environment.Exit(1);
            }

            return 0;
        }
    }
}