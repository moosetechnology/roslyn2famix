using Fame.Internal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Fame.Parser {
    public class MSEPrinter {

        protected int indentation;
        protected StringBuilder stream = new StringBuilder();
        public static object UNLIMITED = new Object();
        private bool newLineHasBeenWritten;
        private string filepath;

        public MSEPrinter(string filepath) {
            this.filepath = filepath;
        }

        protected void IndentLine() {
            if (!newLineHasBeenWritten) {
                stream.Append('\n');

                for (int n = 0; n < indentation; n++) {
                    stream.Append('\t');
                }
            }

            newLineHasBeenWritten = true;
        }

        public void BeginAttribute(String name) {
            indentation++;
            IndentLine();
            AppendCharacter('(');
            AppendString(name);
        }

        private void AppendString(string name) {
            stream.Append(name);
            newLineHasBeenWritten = false;
        }

        private void AppendCharacter(char v) {
            stream.Append(v);
            newLineHasBeenWritten = false;
        }

        public void BeginDocument() {
            // indentation++;
            AppendCharacter('(');
        }


        public void BeginElement(String name) {
            indentation++;
            IndentLine();
            AppendCharacter('(');
            AppendString(name);
        }


        public void EndAttribute(String name) {
            AppendCharacter(')');
            indentation--;
        }

        public void EndDocument() {
            AppendCharacter(')');
            Close();
        }

        private void Close() {
            string s = stream.ToString();
            if (filepath != null)
                System.IO.File.WriteAllText(filepath, stream.ToString());
        }

        public string GetMSE() {
            return stream.ToString();
        }

        public void EndElement(String name) {
            AppendCharacter(')');
            indentation--;
        }

        public void Primitive(Object value) {
            AppendCharacter(' ');
            if (value == UNLIMITED) {
                AppendCharacter('*');
            } else if (value.GetType() == typeof(string)) {
                string str = (string)value;
                AppendCharacter('\'');
                foreach (char ch in (string)value) {
                    if (ch == '\'') AppendCharacter('\'');
                    AppendCharacter(ch);
                }
                AppendCharacter('\'');
            } else if (value is bool) {
                AppendString(value.ToString().ToLower());
            } else if (Number.IsNumber(value)) {
                AppendString(value.ToString());
            }
        }

        public void Reference(int index) {
            stream.Append(" (ref: "); // must prepend space!
            stream.Append(index);
            stream.Append(')');
        }

        public void Reference(String name) {
            stream.Append(" (ref: "); // must prepend space!
            stream.Append(name);
            stream.Append(')');
        }

        public void Serial(int index) {
            stream.Append(" (id: "); // must prepend space!
            stream.Append(index);
            stream.Append(')');
        }

    }
}
