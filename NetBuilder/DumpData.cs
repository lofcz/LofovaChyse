using System;
using System.Collections.Generic;
using System.Text;

namespace NetBuilder
{
    class DumpData
    {
        private static string[] ReadProperties(string Properties)
        {
            List<string> list = new List<string>();

            return list.ToArray();
        }

        private static Table.Column.TypeData ReadTypeData(string Type)
        {
            string name = Type.Substring(Type.IndexOf("[") + 1, Type.IndexOf("]") - Type.IndexOf("[") - 1);
            int length = -1;
            if (Type.Contains("(") && Type.Contains(")"))
            {
                length = int.Parse(Type.Substring(Type.IndexOf("(") + 1, Type.IndexOf(")") - Type.IndexOf("(") - 1));
            }

            Table.Column.TypeData typeData = new Table.Column.TypeData(name, length);

            return typeData;
        }

        public class Table
        {
            public string Name { get; }
            public Column[] Columns { get; }
            public Contraint[] Contraints { get; }

            public class Column
            {
                public string Name { get; }
                public TypeData Type { get; }
                public string[] Properties { get; }
                public Column(string Name, string Type, string Properties)
                {
                    this.Name = Name;
                    this.Type = ReadTypeData(Type);
                    this.Properties = ReadProperties(Properties);
                }

                public class TypeData
                {
                    public string Type { get; }
                    public int Length { get; }

                    public TypeData(string Type, int Length)
                    {
                        this.Type = Type;
                        this.Length = Length;
                    }
                }
            }
            public class Contraint
            {
                public string ContraintName { get; }
                public string[] Properties { get; }
                public ContraintColumn Data { get; }

                public class ContraintColumn
                {
                    public string Name { get; }
                    public string Method { get; }

                    public ContraintColumn(string Name, string Method)
                    {
                        this.Name = Name;
                        this.Method = Method;
                    }
                }
            }
        }

        public class Data
        {

        }
    }
}
