using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

using Mono.Cecil;

namespace Tizen.Build.Validators
{
    internal class NativeSizeProvider
    {
        private static Dictionary<string, int> _sizeInfoTable;

        public static string DataFile { get; set; }

        public static int SizeOf(TypeDefinition type)
        {
            EnsureSizeInfo();
            return _sizeInfoTable.TryGetValue(type.FullName, out int size) ? size : -1;
        }

        private static void EnsureSizeInfo()
        {
            if (_sizeInfoTable != null)
            {
                return;
            }

            _sizeInfoTable = new Dictionary<string, int>();

            if (string.IsNullOrEmpty(DataFile))
            {
                var asmDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                DataFile = Path.Join(asmDir, "native-size.dat");
            }

            var items = File.ReadAllLines(DataFile);
            foreach (var item in items)
            {
                var pair = item.Split(':');
                if (pair.Length == 2)
                {
                    int.TryParse(pair[1], out int size);
                    _sizeInfoTable.Add(pair[0], size);
                }
            }
        }
    }
}
